/*
	Copyright (c) 2005 Sergio Rubio, <sergio.rubio@hispalinux.es>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace SimpleP

import Gtk
import System
import Glade
import System.Collections
import System.IO
import Red.ProjectManager
import Red.Gtk
import NLog

class MainWindow (Window):

	[Glade.Widget] treeViewBox as VBox
	[Glade.Widget] itemViewBox as VBox
	[Glade.Widget] projectCombo as ComboBox
	[Glade.Widget] statusbar as Statusbar
	[Glade.Widget] contextMenues as MenuItem
	[Glade.Widget] hpaned as HPaned
	[Glade.Widget] projectsLabel as Label
	[Glade.Widget] projectsLabelBox as EventBox
	iContextMenues as Menu
	projectView as TreeView 
	_lastWidget as Widget 
	
	projectStore as ListStore
	gxml as XML
	log as Logger = LogManager.GetLogger ("MainWindow")

	def constructor ():
		super ("SimpleP")
		WidthRequest = 300
		HeightRequest = 350
		gxml = XML (Globals.Resources, "simplep.glade", "mainBox", null)
		gxml.Autoconnect (self)
		Add (gxml["mainBox"])
		SizeAllocated += SizeChanged
		width = Int32.Parse (Services.Config["Width"])
		height = Int32.Parse (Services.Config["Height"])
		Resize (width, height)
		x, y = Int32.Parse (Services.Config["XPos"]), Int32.Parse (Services.Config["YPos"])
		Move (x, y)
		hpaned.Position = Services.Config.MainConfig.GetInt ("HPanedPosition", 150)
		Init ()

	private def Init ():
		DeleteEvent += WindowDeleted
		Icon = Gdk.Pixbuf (Globals.Resources,"SimpleP-icon-gears.png")

		projectsLabelBox.ModifyBg (StateType.Normal, Gdk.Color (76,76,76))
		
		SetupProjectView ()
		Services.Statusbar.MessagePushed += MessagePushed

		//ProjectCombo
		projectCombo.Model = ListStore ((typeof (string), ))

		for pname as string in Services.ProjectManager.Projects:
			projectCombo.AppendText (pname)
			
		projectCombo.Changed += def (sender, args):
			ChangeProject ()
		
		Services.ProjectManager.ProjectChanged += ProjectChangedHandler
		Services.ProjectManager.ProjectAdded += ProjectAddedHandler
		Services.ProjectManager.ProjectRemoved += ProjectRemovedHandler

	private def WindowDeleted (sender, args as DeleteEventArgs):
		x as int
		y as int
		GetPosition (x, y)
		Services.Config["XPos"] = x.ToString ()
		Services.Config["YPos"] = y.ToString ()
		Services.Config["HPanedPosition"] = hpaned.Position.ToString ()
		currentProject = Services.ProjectManager.CurrentProject
		if currentProject:
			Services.Config["LastProject"] = currentProject.Name
		Services.Config.SaveConfig ()
		Application.Quit ()

	private def SizeChanged (sender, args as SizeAllocatedArgs):
		Services.Config["Width"] = args.Allocation.Width.ToString ()
		Services.Config["Height"] = args.Allocation.Height.ToString ()
		Services.Config.SaveConfig ()

	private def SetupProjectView ():
		projectStore = ContextViewStore ( (typeof (Gdk.Pixbuf), typeof (string)) )	
		menuItems = 0
		iContextMenues = Menu ()
		contextMenues.Submenu = iContextMenues
		for context as IProjectContext in Services.ContextManager.GetAllContexts ():
			projectStore.AppendValues ((context.Icon, context.Name))
			if context.Menu != null:
				mitem = MenuItem (context.Name)
				mitem.Submenu = context.Menu
				iContextMenues.Append (mitem)
				menuItems += 1
		if menuItems == 0:
			contextMenues.Sensitive = false
		
		projectView = TreeView ()
		
		projectView.HeadersVisible = false
		projectView.Model = projectStore
		projectView.Reorderable = false
		projectView.AppendColumn ("Icon", CellRendererPixbuf (), ("pixbuf", 0))
		textRenderer = CellRendererText ()
		textRenderer.Visible = true
		projectView.AppendColumn ("Name", textRenderer, ("text", 1))
			
		projectView.CursorChanged += CursorChanged
		treeViewBox.Add (projectView)

	private def ChangeProject ():
		iter as TreeIter
		if projectCombo.GetActiveIter (iter):
			name = projectCombo.Model.GetValue (iter, 0)
			try:
				Services.ProjectManager.SetActiveProject (name)
				if Services.ProjectManager.MissingFiles != null: 
					DialogFactory.ShowInfoDialog (self,"Missing files", "Some files in the project are missing.")
				Title = "SimpleP - ${name}"

			except ex as InvalidProjectFileException:
				print "MESSAGE: ${ex.Message}"
				print "STACK TRACE:\n" + ex.StackTrace
				DialogFactory.ShowErrorDialog (self, "Invalid project file","The project file is corrupted.")
				
			except ex as Exception:
				print "MESSAGE: ${ex.Message}"
				print "STACK TRACE:\n" + ex.StackTrace
				DialogFactory.ShowErrorDialog (self, "Error changing project","Project file cannot be loaded.")	

		
	private def CursorChanged (sender, args):
		column as TreeViewColumn
		path as TreePath
		iter as TreeIter
		projectView.GetCursor (path, column)
		projectStore.GetIter (iter, path)
		sectionName as string = projectStore.GetValue (iter, 1)
		context = Services.ContextManager.GetContext (sectionName)
		if context:
			if _lastWidget:
				itemViewBox.Remove (_lastWidget)
			context.View.ShowAll ()
			if context.View.Parent:
				context.View.Reparent (itemViewBox)
			else:
				itemViewBox.Add (context.View)
			context.Activate ()
			_lastWidget = context.View
		
	private def NewProjectMenuClicked (sender, args):
		window = NewProjectWindow ()
		window.ShowAll ()
	
	private def NewFileMenuClicked (sender, args):
		pass
	
	private def OpenMenuClicked (sender, args):
		chooser = FileChooserDialog ("Choose the project file", self, FileChooserAction.Open, ("_Open", ResponseType.Ok))
		filter = FileFilter ()
		filter.AddPattern ("*.simplep")
		chooser.Filter = filter
		response = chooser.Run ()
		if response == -5:
			Services.ProjectManager.LoadProject (chooser.Filename)
		chooser.Destroy ()
	
	private def AboutMenuClicked (sender, args):
		AboutWindow ().ShowAll ()

	private def ImportProjectFilesClicked (sender, args):
		project = Services.ProjectManager.CurrentProject
		if project:
			DialogFactory.ShowInfoDialog (self, "Importing notice", "dot files and directories (i.e. .svn or .cvsignore) are discarded when importing files.")
			lastFileCount = project.Files.Count
			project.ImportFiles (true)
			fileCount = project.Files.Count - lastFileCount
			DialogFactory.ShowInfoDialog (self, "Importing files","<b>${fileCount}</b> files imported.")
		else:
			DialogFactory.ShowInfoDialog (self, "No project selected","You should choose a project first, then try to import the files again.")

	private def ReimportProjectClicked (sernder, args):
		currentProject = Services.ProjectManager.CurrentProject
		if currentProject:
			Services.ProjectManager.CurrentProject.Clear ()
			currentProject.ImportFiles (true)
			fileCount = currentProject.Files.Count
			Services.ProjectManager.CurrentProject.Save ()
			DialogFactory.ShowInfoDialog (self, "Importing files","<b>${fileCount}</b> files imported.")
		else:
			DialogFactory.ShowInfoDialog (self, "No project selected", "You should choose a project first, then try to import the files again.")

	private def MessagePushed (sender,args as  MessagePushedArgs):
		statusbar.Push (statusbar.GetContextId (args.Message), args.Message)

	private def ProjectChangedHandler (sender, args):
		if Services.ProjectManager.CurrentProject:
			iter as TreeIter
			model as ListStore
			model = projectCombo.Model
			if projectCombo.GetActiveIter (iter):
				name = projectCombo.Model.GetValue (iter, 0)
				currentName = Services.ProjectManager.CurrentProject.Name
				if name != currentName:
					model.Foreach (ChangedForeach)
			else:
				model.Foreach (ChangedForeach)

	private def ChangedForeach (model as TreeModel, path as TreePath, iter as TreeIter) as bool:
		currentName = Services.ProjectManager.CurrentProject.Name
		name = model.GetValue (iter, 0)
		if name == currentName:
			projectCombo.SetActiveIter (iter)
			return true
		return false
				
	private def ProjectAddedHandler (sender, args as ProjectAddedArgs):
		model = projectCombo.Model
		cast(ListStore,model).Clear ()
		for name as string in Services.ProjectManager.Projects:
			cast(ListStore, model).AppendValues ( (name,) )
	
	private def ProjectRemovedHandler (sender, args as ProjectRemovedArgs):
		model = projectCombo.Model
		cast(ListStore,model).Clear ()
		for name as string in Services.ProjectManager.Projects:
			cast(ListStore, model).AppendValues ( (name,) )
		
	private def RemoveProjectClicked (sender, args):
		currentProject = Services.ProjectManager.CurrentProject
		if currentProject:
			response = DialogFactory.ShowQuestionDialog (self, "Removing project","Do you want to remove the project ${Services.ProjectManager.CurrentProject.Name}?\n\n<b>Note:</b>\nThe project files will not be deleted")
			if response == -8:
				Services.ProjectManager.RemoveCurrentProject ()
		else:
			DialogFactory.ShowInfoDialog (self, "No project selected","Choose a project first and then remove it.")

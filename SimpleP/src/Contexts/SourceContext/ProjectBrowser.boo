/*
	Copyright (c) 2005 Sergio Rubio, <sergio.rubio@hispalinux.es>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace SimpleP

import System
import System.IO
import Gtk
import System.Collections
import Red.ProjectManager
import Red.Gtk
import Glade

class ProjectBrowser (TreeView):

	store as ProjectStore
	sep = System.IO.Path.DirectorySeparatorChar
	gxml as XML

	def constructor (project as Project):
		gxml = XML ("simplep.glade", "navigatorWidget", null)
		gxml.Autoconnect (self)
		store = ProjectStore ( (typeof (string), typeof (Gdk.Pixbuf), typeof (string), typeof (bool)) )
		Model = store
		HeadersVisible = false
		RowActivated += RowActivatedHandler

		
		fileColumn =  TreeViewColumn ()
		fileColumn.Title = "Name"
		
		pixRenderer = CellRendererPixbuf ()
		fileColumn.PackStart (pixRenderer, false)
		fileColumn.AddAttribute (pixRenderer, "pixbuf", store.IconCol)

		textRenderer = CellRendererText ()
		fileColumn.PackStart (textRenderer, false)
		fileColumn.AddAttribute (textRenderer, "text", store.TextCol)
		
		AppendColumn (fileColumn)
		CursorChanged += CursorChangedHandler
		
		CurrentProject = project
		
	protected override def OnButtonPressEvent (evt as Gdk.EventButton):
		super.OnButtonPressEvent (evt)
		if evt.Type == Gdk.EventType.ButtonPress:
			if evt.Button == 3:
				Services.PopupMenu.FileOperations.RemoveFromProjectHandler = RemoveFromProjectClicked
				Services.PopupMenu.FileOperations.RemoveFromDiskHandler = RemoveFromDiskClicked
				Services.PopupMenu.FileOperations.AddNewDirectoryHandler = AddNewDirectoryClicked
				Services.PopupMenu.FileOperations.AddNewFileHandler = AddNewFileClicked
				Services.PopupMenu.FileOperations.Popup ()
		

	private def CursorChangedHandler (sender, args):
		#column as TreeViewColumn
		#path as TreePath
		#iter as TreeIter
		#projectView.GetCursor (path, column)
		#projectStore.GetIter (iter, path)
		#sectionName as string = projectStore.GetValue (iter, 1)
		pass

	_currentProject as Project
	public CurrentProject as Project:
		get:
			return _currentProject
		set:
			if _currentProject:
				_currentProject.ContentsChanged -= ContentChangedHandler
			
			_currentProject = value
			if _currentProject:
				_currentProject.ContentsChanged += ContentChangedHandler
				Reload ()
			else:
				store.Clear ()

	private def Reload ():
		store.Clear ()
		AddFilesToView (_currentProject.GetFilesFromSubdir (_currentProject.Navigator.CurrentPath))
		AddDirectoriesToView ()

	private def RowActivatedHandler (sender, args as RowActivatedArgs):
		iter as TreeIter
		store.GetIter (iter, args.Path)
		name as string = store.GetValue (iter, store.TextCol)
		fullName as string = store.GetValue (iter, store.FullNameCol)
		isDir as bool = store.GetValue (iter, store.IsDirectoryCol)

		if isDir and not name == "../":
			_currentProject.Navigator.MoveToSubdir (fullName)
			Reload ()
			Services.Statusbar.PushMessage (_currentProject.Navigator.CurrentPath)
		elif name == "../":
			_currentProject.Navigator.MoveUp ()
			Reload ()
			Services.Statusbar.PushMessage (_currentProject.Navigator.CurrentPath)
		else:
			if not Services.Mimetype.OpenFile (fullName):
				DialogFactory.ShowInfoDialog (self as Window,"<b>Program missing</b>\nThe program that handles this file is missing, please install it.")

	private def AddDirectoriesToView ():
		for subdir as string in _currentProject.Navigator.CurrentSubdirs:
			subdir = System.IO.Path.Combine (_currentProject.Navigator.CurrentPath, subdir)
			store.AppendValues ( (subdir.Split ( (sep,) )[-1], Gdk.Pixbuf ("images/folder.png"), subdir, true) )
		
		if _currentProject.Navigator.HasParent:
			store.AppendValues ( ("../", Gdk.Pixbuf ("images/folder.png"), _currentProject.Navigator.Parent, true) )
		

	_filters = Globals.SourceExtensions
	private def AddFilesToView (files as ICollection):

		for pfile as ProjectFile in files:
			extension = System.IO.Path.GetExtension (pfile.FullName)
			if extension == string.Empty:
				extension = pfile.FullName
			if not extension in _filters:
				continue
			store.AppendValues ( (pfile.Name, Gdk.Pixbuf ("images/file.png"), pfile.FullName, false) )

	private def RemoveFromDiskClicked ():
		file as string = GetRowValue (store.FullNameCol)
		isDir as bool = GetRowValue (store.IsDirectoryCol)
		currentProject = Services.ProjectManager.CurrentProject
		if isDir:
			msg = "<b>Remove directory</b>\n\nThe directory and its content will be removed from the disk,Are you sure?"
			response = DialogFactory.ShowQuestionDialog (null, msg)
			//This sucks optimize it
			if response == cast(int,ResponseType.Yes):
				currentProject.RemoveDir (file, true)
		else:
			relPath = file.Replace (currentProject.Location, "")
			msg = "<b>Remove file</b>\n\nDo you want to remove the file <b>${relPath}</b> from the disk?"
			response = DialogFactory.ShowQuestionDialog (null, msg)
			if response == cast(int,ResponseType.Yes):
				currentProject.RemoveFile (file, true)

	private def AddNewFileClicked ():
		print "Adding new file"

	private def AddNewDirectoryClicked ():
		print "Adding new folder"

	private def RemoveFromProjectClicked ():
		if not Selection:
			return
		file as string = GetRowValue (store.FullNameCol)
		isDir as bool = GetRowValue (store.IsDirectoryCol)
		if isDir:
			Services.ProjectManager.CurrentProject.RemoveDir (file)
		else:
			Services.ProjectManager.CurrentProject.RemoveFile (file)
	
	private def GetRowValue (col as int):
		column as TreeViewColumn
		path as TreePath
		iter as TreeIter
		GetCursor (path, column)
		store.GetIter (iter, path)
		return store.GetValue (iter, col)

	private def ContentChangedHandler (sender, args):
		Reload ()

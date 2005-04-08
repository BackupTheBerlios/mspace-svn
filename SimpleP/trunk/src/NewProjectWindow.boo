/*
	Copyright (c) 2005 Sergio Rubio, <sergio.rubio@hispalinux.es>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace SimpleP

import Gtk
import Glade
import System
import System.IO
import Red.Gtk

class NewProjectWindow (Window):

	[Glade.Widget] fileImportEntry as Entry
	[Glade.Widget] projectNameEntry as Entry
	[Glade.Widget] projectDirEntry as Entry
	[Glade.Widget] importCheckButton as CheckButton
	gxml as XML
	
	def constructor ():
		super ("")
		gxml = XML (Globals.Resources, "simplep.glade", "newProjectWindow", null)
		gxml.Autoconnect (self)
		home = Environment.GetEnvironmentVariable ("HOME")
		projectsDir = System.IO.Path.Combine (home, "Projects")
		if Directory.Exists (projectsDir):
			projectDirEntry.Text = projectsDir
		else:
			projectDirEntry.Text = home
		self.Raw = gxml["newProjectWindow"].Raw
		importCheckButton.Active = true

	public event Closed as EventHandler

	public ProjectName as string:
		get:
			return projectNameEntry.Text

	public ProjectLocation as string:
		get:
			return projectDirEntry.Text

	def ChooserButtonClicked (sender, args):
		chooser = FileChooserDialog ("Choose the project directory", self, FileChooserAction.SelectFolder, ("_Ok", -5))
		chooser.SetCurrentFolder (projectDirEntry.Text)
		response = chooser.Run ()
		if response == -5:
			projectDirEntry.Text = chooser.CurrentFolder
			chooser.Destroy ()	


	def OnOkClicked (sender, args):
		if projectNameEntry.Text == String.Empty:
			DialogFactory.ShowInfoDialog (self, "<b>Empty project name</b>\nFill the <b>project name</b> field.")
			return
		
		if projectDirEntry.Text == String.Empty or \
			not Directory.Exists (projectDirEntry.Text):
			DialogFactory.ShowInfoDialog (self, "<b>Invalid project directory</b>\nChoose a valid project directory.")
			return

		if Services.ProjectManager.Contains (ProjectName):
			DialogFactory.ShowInfoDialog (self, "<b>Invalid project name</b>\nThe project name already exists. Choose another name.")
			return

		Services.ProjectManager.NewProject (ProjectName, ProjectLocation, false)
		#if importCheckButton.Active:
		#	project.ImportFiles ()

		Closed (self, EventArgs.Empty) if Closed
		Destroy ()
	
	def OnCancelClicked (sender, args):
		Destroy ()

namespace SimpleP

import Gtk
import Glade

class FileOperationsMenu (Menu):

	gxml as XML

	def constructor ():
		gxml = XML (Globals.Resources, "simplep.glade", "fileOperationsMenu", null)
		gxml.Autoconnect (self)
		Raw = gxml["fileOperationsMenu"].Raw

	public RemoveFromProjectHandler as callable ()

	public RemoveFromDiskHandler as callable ()

	public AddNewFileHandler as callable ()

	public AddNewDirectoryHandler as callable ()

	private def RemoveFromProjectClicked (sender, args):
		if not RemoveFromProjectHandler:
			return
		RemoveFromProjectHandler.Call (null)
		RemoveFromProjectHandler = null

	private def RemoveFromDiskClicked (sender, args):
		if not RemoveFromDiskHandler:
			return
		RemoveFromDiskHandler.Call (null)
		RemoveFromDiskHandler = null
	
	private def AddNewFileClicked (sender, args):
		if not AddNewFileHandler:
			return
		AddNewFileHandler.Call (null)
		AddNewFileHandler = null
	
	private def AddNewDirectoryClicked (sender, args):
		if not AddNewDirectoryHandler:
			return
		AddNewDirectoryHandler.Call (null)
		AddNewDirectoryHandler = null

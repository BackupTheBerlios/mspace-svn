/*
	Copyright (c) 2005 Sergio Rubio, <sergio.rubio@hispalinux.es>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace SimpleP

import System
import Gtk
import Red.ProjectManager
import Red.Gtk
import Glade
import System.IO

class GladeContextView (TreeView):

	enum Column:
		Pixbuf
		Name
		FullName

	_store as ListStore
	
	def constructor ():
		_store = ListStore ( (typeof (Gdk.Pixbuf), typeof (string), typeof (string)) )
		Model = _store
		HeadersVisible = false
		
		fileColumn =  TreeViewColumn ()
		fileColumn.Title = "Name"
		
		pixRenderer = CellRendererPixbuf ()
		fileColumn.PackStart (pixRenderer, false)
		fileColumn.AddAttribute (pixRenderer, "pixbuf", cast (int,Column.Pixbuf))

		textRenderer = CellRendererText ()
		fileColumn.PackStart (textRenderer, false)
		fileColumn.AddAttribute (textRenderer, "text", cast (int,Column.Name))
		fileColumn.SetCellDataFunc (textRenderer, cast(TreeCellDataFunc,CellDataFunc))
		AppendColumn (fileColumn)
		
		Services.ProjectManager.ProjectChanged += ProjectChanged

		Selection.Mode = SelectionMode.Multiple

	protected override def OnRowActivated(path as TreePath, column as TreeViewColumn):
		file as string = GetRowValue (cast(int, Column.FullName))
		Services.Mimetype.OpenFile (file)

	protected override def OnCursorChanged ():
		file as string = GetRowValue (cast(int, Column.FullName))
		Services.Statusbar.PushMessage (file)

	protected override def OnButtonReleaseEvent (evt as Gdk.EventButton):
		#super.OnButtonReleaseEvent (evt)
		model as TreeModel
		paths = Selection.GetSelectedRows (model)
		if paths.Length == 0:
			return
		if evt.Button == 3:
			Services.PopupMenu.FileOperations.RemoveFromDiskHandler = RemoveFromDiskClicked
			Services.PopupMenu.FileOperations.RemoveFromProjectHandler = RemoveFromProjectClicked
			Services.PopupMenu.FileOperations.Popup ()
	
	private def Reload ():
		_store.Clear ()
		currentProject = Services.ProjectManager.CurrentProject
		if currentProject:
			files = currentProject.GetFilesWithExtension ([".glade"])
			for file as ProjectFile in files:
				fileText = "${file.Name}\n<span foreground='grey' size='small'>${file.FullName}</span>"
				_store.AppendValues ( (Gdk.Pixbuf ("images/glade-20.png"),
										fileText, file.FullName) )

	private def ProjectChanged (sender, args):
		currentProject = Services.ProjectManager.CurrentProject
		if currentProject:
			currentProject.FileRemoved += FileChangedHandler
			currentProject.FileAdded += FileChangedHandler
			Reload ()
	
	private def GetRowValue (col as int):
		column as TreeViewColumn
		path as TreePath
		iter as TreeIter
		GetCursor (path, column)
		_store.GetIter (iter, path)
		return _store.GetValue (iter, col)
	
	private def RemoveFromDiskClicked ():
		model as TreeModel
		paths = Selection.GetSelectedRows (model)
		if paths.Length == 0:
			return
		file as string = GetRowValue (cast(int, Column.FullName))
		currentProject = Services.ProjectManager.CurrentProject
		relPath = file.Replace (currentProject.Location, "")
		msg = "<b>Remove file</b>\n\nDo you want to remove the file <b>${relPath}</b> from the disk?"
		response = DialogFactory.ShowQuestionDialog (null, msg)
		if response == cast(int,ResponseType.Yes):
			currentProject.RemoveFile (file, true)

	private def RemoveFromProjectClicked ():
		model as TreeModel
		paths = Selection.GetSelectedRows (model)
		if paths.Length == 0:
			return
		file as string = GetRowValue (cast(int, Column.FullName))
		Services.ProjectManager.CurrentProject.RemoveFile (file)

	private def FileChangedHandler (sender, args as FileChangedArgs):
		if args.File.EndsWith (".glade"):
			Reload ()

	private def CellDataFunc (column as TreeViewColumn, renderer as CellRenderer, model as TreeModel, iter as TreeIter):
		(renderer as CellRendererText).Markup = _store.GetValue (iter, cast (int, Column.Name))


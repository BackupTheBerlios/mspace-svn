namespace SimpleP

import Gtk

class SourceContext (IProjectContext):

	public Icon as Gdk.Pixbuf:
		get:
			return Gdk.Pixbuf ("images/Source-icon.png")

	_name = "Source"
	public Name as string:
		get:
			return _name

	_widget as Widget
	public View as Widget:
		get:
			currentProject = Services.ProjectManager.CurrentProject
			if not currentProject:
				CreateFakeWidget ()
			else:
				CreateWidget ()
			return _widget
			
	_menu  as Menu 
	public Menu as Menu:
		get:
			return null
	
	public def Activate ():
		pass
	
	_realWidget as ScrolledWindow 
	_projectBrowser as ProjectBrowser
	private def CreateWidget ():
		if not _realWidget:
			currentProject = Services.ProjectManager.CurrentProject
			scroll = ScrolledWindow ()
			_projectBrowser = ProjectBrowser (currentProject)
			Services.ProjectManager.ProjectChanged += ProjectChangedHandler
			scroll.Add (_projectBrowser)
			_realWidget = scroll
		
		_widget = _realWidget

	_fakeWidget as Label
	private def CreateFakeWidget ():
		if not _fakeWidget:
			label = Label ("Select a project First")
			label.UseMarkup = true
			_fakeWidget = label
		
		_widget = _fakeWidget
	
	private def ProjectChangedHandler (sender, args):
		if _projectBrowser:
			_projectBrowser.CurrentProject = Services.ProjectManager.CurrentProject
		

namespace SimpleP

import Gtk

class StartPageContext (IProjectContext):
		
	_icon = Gdk.Pixbuf (Globals.Resources, "start-page.png")
	public Icon as Gdk.Pixbuf:
		get:
			return _icon

	public Name as string:
		get:
			return "Start page"

	_widget = StartPageWidget ()
	public View as Widget:
		get:
			return _widget

	public Menu as Menu:
		get:
			return null

	def Activate ():
		pass
		

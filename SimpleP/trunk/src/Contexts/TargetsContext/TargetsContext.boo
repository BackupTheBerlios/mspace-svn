namespace SimpleP

import Gtk
import Gdk

class TargetsContext (IProjectContext):

	_icon = Pixbuf (Globals.Resources, "target.png")
	public Icon as Gdk.Pixbuf:
		get:
			return _icon

	_widget = Label ("Not available")
	public View as Widget:
		get:
			return _widget

	_name = "Targets"
	public Name as string:
		get:
			return _name
			
	public Menu as Menu:
		get:
			return null
	
	def Activate ():
		pass

namespace SimpleP

import Gtk
import Gdk

class OthersContext (IProjectContext):

	_icon = Pixbuf ("images/Other-icon.png")
	public Icon as Gdk.Pixbuf:
		get:
			return _icon

	_widget = Label ("Not available")
	public View as Widget:
		get:
			return _widget

	_name = "Other files"
	public Name as string:
		get:
			return _name
			
	public Menu as Menu:
		get:
			return null

	def Activate ():
		pass

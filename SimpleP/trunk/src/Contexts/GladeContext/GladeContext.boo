namespace SimpleP

import Gtk
import Gdk

class GladeContext (IProjectContext):

	_icon = Pixbuf ("images/UI-icon.png")
	public Icon as Gdk.Pixbuf:
		get:
			return _icon

	_widget = GladeContextView ()
	public View as Widget:
		get:
			return _widget

	_name = "Glade"
	public Name as string:
		get:
			return _name
			
	def Activate ():
		_widget.Activate ()

	public Menu as Menu:
		get:
			return null

namespace SimpleP

import Gtk

interface IProjectContext:

	Icon as Gdk.Pixbuf:
		get

	Name as string:
		get

	View as Widget:
		get

	Menu as Menu:
		get

	def Activate ()

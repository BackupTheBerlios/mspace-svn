namespace SimpleP

import Gtk

class StartPageWidget (VBox):
	
	html as HTML
	
	def constructor ():
		html = HTML ("<b>Hello</b>")
		Add (html)

namespace SimpleP

import Gtk
import System.Reflection
import System.IO

class StartPageWidget (VBox):
	
	html as HTML
	
	def constructor ():
		stream = Globals.Resources.GetManifestResourceStream ("welcome.html")
		reader = StreamReader (stream)
		content = reader.ReadToEnd ()
		reader.Close ()
		stream.Close ()
		html = HTML (content)
		Add (html)

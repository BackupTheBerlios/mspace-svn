namespace SimpleP

import Gtk
import System.Reflection
import System.IO

class StartPageWidget (VBox):
	
	html as HTML
	lastProject as string
	
	def constructor ():
		stream = Globals.Resources.GetManifestResourceStream ("welcome.html")
		reader = StreamReader (stream)
		content = reader.ReadToEnd ()
		reader.Close ()
		stream.Close ()
		html = HTML (content)
		html.LinkClicked += HandleLinkClicked
		Add (html)
		lastProject = Services.Config.MainConfig.GetString ("LastProject", null)

	def HandleLinkClicked (sender, args as LinkClickedArgs):
		if args.Url == "OpenLastProject" and lastProject and Services.ProjectManager.Contains (lastProject):
			print "Restoring las projecT"
			Services.ProjectManager.SetActiveProject (lastProject)

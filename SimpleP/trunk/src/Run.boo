/*
	Copyright (c) 2005 Sergio Rubio, <sergio.rubio@hispalinux.es>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace SimpleP

import Gtk
import NLog
import System
import Red.Gtk

log = LogManager.GetLogger ("MainWindow")
try:
	Application.Init ()
	Services.ContextManager.RegisterContext (SourceContext ())
	Services.ContextManager.RegisterContext (GladeContext ())
	Services.ContextManager.RegisterContext (TargetsContext ())
	Services.ContextManager.RegisterContext (OthersContext ())
	MainWindow ().ShowAll ()
	Application.Run ()
except ex as Exception:
	log.Error ("\nUnhandled Exception\n")
	log.Error ("---------------------------------")
	log.Error ("\nException Message\n${ex.Message}")
	log.Error ("---------------------------------")
	log.Error ("\nStackTrace\n${ex.StackTrace}")
	DialogFactory.ShowErrorDialog (null, "<b>Unhandled exception</b>\n\nYou have found a bug in SimpleP. Please send an email to <b>sergio.rubio@hispalinux.es</b> and attach the <b>report.log</b> file in the application folder.\nThanks.")
	

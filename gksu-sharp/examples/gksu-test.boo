import Gksu
import Gksu.UI
import Gtk

Application.Init ()

suDialog = SuDialog ()
suDialog.Title = "Gksu from mono"
suDialog.Message = "Enter the root (or sudo) password"
response = suDialog.Run ()
suDialog.Hide ()
if response == cast (int, ResponseType.Ok):
	suDialog.Hide ()
	context = SuContext ()
	context.Command = "xterm"
	context.Password = suDialog.Password
	context.SudoRun ()


Application.RunIteration ()

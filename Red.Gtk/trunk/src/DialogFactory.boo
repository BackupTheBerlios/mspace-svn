namespace Red.Gtk

import Gtk

class DialogFactory:
	public static def ShowWarningDialog (parent as Window, msg as string):
		dialog = MessageDialog (parent,
						DialogFlags.Modal,
						MessageType.Warning,
						ButtonsType.Close,
						msg,
						(,)
						)
		dialog.Run ()
		dialog.Destroy ()
	
	public static def ShowWarningDialog (parent as Window, description as string, msg as string):
		ShowWarningDialog (parent, "<b><big>${description}</big></b>\n\n${msg}")
	
	public static def ShowQuestionDialog (parent as Window, msg as string) as int:
		dialog = MessageDialog (parent,
						DialogFlags.Modal,
						MessageType.Question,
						ButtonsType.YesNo,
						msg,
						(,)
						)
		response = dialog.Run ()
		dialog.Destroy ()
		return response
	
	public static def ShowQuestionDialog (parent as Window, description as string, msg as string):
		return ShowQuestionDialog (parent, "<b><big>${description}</big></b>\n\n${msg}")
	
	public static def ShowInfoDialog (parent as Window, msg as string):
		dialog = MessageDialog (parent,
						DialogFlags.Modal,
						MessageType.Info,
						ButtonsType.Close,
						msg,
						(,)
						)
		dialog.Run ()
		dialog.Destroy ()
	
	public static def ShowInfoDialog (parent as Window, description as string, msg as string):
		ShowInfoDialog (parent, "<b><big>${description}</big></b>\n\n${msg}")
	
	public static def ShowErrorDialog (parent as Window, msg as string):
		dialog = MessageDialog (parent,
						DialogFlags.Modal,
						MessageType.Error,
						ButtonsType.Close,
						msg,
						(,)
						)
		dialog.Run ()
		dialog.Destroy ()
	
	public static def ShowErrorDialog (parent as Window, description as string, msg as string):
		ShowErrorDialog (parent, "<b><big>${description}</big></b>\n\n${msg}")

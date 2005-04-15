namespace Red.ProjectManager
import System

[Flags]
enum ProjectEventType:
	FileAdded
	FileRemoved
	ContentsChanged
	Cleared
	
interface IProjectEventListener:
	def OnFileAdded(sender as object, args as FileChangedArgs)
	def OnFileRemoved(sender as object, args as FileChangedArgs)
	def OnContentsChanged(sender as object, args as EventArgs)
	def OnCleared(sender as object, args as EventArgs)

class FileChangedArgs(EventArgs):
	def constructor(file as string):
		File = file
	
	public File as string
	



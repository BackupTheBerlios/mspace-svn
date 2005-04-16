namespace Red.Components.Project
import System

[Flags]
enum ProjectEventType:
	ItemAdded
	ItemRemoved
	ContentsChanged
	Cleared
	ProjectSaved
	ProjectLoaded
	
interface IProjectEventListener:
	def OnItemAdded(sender as object, args as ItemEventArgs)
	def OnItemRemoved(sender as object, args as ItemEventArgs)
	def OnContentsChanged(sender as object, args as EventArgs)
	def OnCleared(sender as object, args as EventArgs)
	def OnLoad(sender as object, args as ProjectEventArgs)
	def OnSave(sender as object, args as ProjectEventArgs)


class ItemEventArgs(EventArgs):
	
	def constructor(item as IProjectItem):
		_item = item
	
	_item as IProjectItem
	Item as IProjectItem:
		get:
			return _item

class ProjectEventArgs (EventArgs):
	
	def constructor (project as IProject):
		_project = project
	
	_project as IProject
	Project as IProject:
		get:
			return _project


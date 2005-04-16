namespace Red.Components.Project
	
interface IProjectEventManager:

	def AddListener ([required]listener as IProjectEventListener,
				evt as ProjectEventType)
	def RemoveListener ([required]listener as IProjectEventListener,
					evt as ProjectEventType)

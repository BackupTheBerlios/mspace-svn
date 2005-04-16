namespace Red.Components.Project

interface IProjectSerializer:
	
	def LoadProject (uri as string) as IProject
	def SaveProject (project as IProject, uri as string)

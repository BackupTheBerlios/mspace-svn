namespace Red.ProjectManager

import System.Collections

interface IProjectNavigator:

	/************************
			Properties
	*************************/
	CurrentPath as string:
		get
	
	CurrentSubdirs as IList:
		get
						
	CurrentFiles as IList:
		get

	HasParent as bool:
		get
	
	HasNext as bool:
		get

	HasPrevious as bool:
		get

	Parent as string:
		get
	
	/************************
			Methods	
	*************************/
	def MoveUp ()

	def Next ()

	def Previous ()
		
	def MoveToSubdir ([required]dir as string)


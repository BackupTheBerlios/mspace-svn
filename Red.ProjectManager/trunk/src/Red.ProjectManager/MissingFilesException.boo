namespace Red.ProjectManager

import System
import System.Collections

class MissingFilesException (Exception):
	
	public Files as IList = []
	public Project as Project

	def constructor (msg as string, project as Project):
		super (msg)
		Project = project

	
	

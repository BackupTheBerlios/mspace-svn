namespace Red.ProjectManager

import System
import System.IO

class ProjectFile:

	public Name as string	
	public FullName as string

	def constructor (fullName as string):
		assert fullName; assert Path.IsPathRooted (fullName)
		FullName = fullName
		Name = Path.GetFileName (fullName)

namespace Red.ProjectManager

import System
import System.IO

class ProjectFile (IProjectFile):

	_name as string
	public Name as string:
		get:
			return _name
			
	_fullName as string
	public FullName as string:
		get:
			return _fullName

	def constructor (fullName as string):
		assert fullName; assert Path.IsPathRooted (fullName)
		_fullName = fullName
		_name = Path.GetFileName (fullName)

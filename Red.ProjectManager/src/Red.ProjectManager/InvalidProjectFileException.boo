namespace Red.ProjectManager

import System

class InvalidProjectFileException (Exception):
	def constructor (msg as string):
		super (msg)

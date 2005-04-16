namespace Red.Components.Project

import System

public enum ItemType:
	Icon
	Image
	File
	Directory
	
interface IProjectItem:

	Type as ItemType:
		get
	Location as string:
		get
	Name as string:
		get

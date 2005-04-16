namespace Red.Components.Project

import System
import System.IO
import System.Text.RegularExpressions
import System.Collections

interface IProject (ICollection):
	Name as string:
		get
	
	Items as IList:
		get
	
	def Contains(item as IProjectItem) as bool
	def Clear()
	def AddItem(item as IProjectItem)
	def RemoveItem(item as IProjectItem, delete as bool)
	def GetItem (itemName as string)


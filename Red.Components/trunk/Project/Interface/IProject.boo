namespace Red.ProjectManager

import System
import System.IO
import System.Text.RegularExpressions
import System.Collections

interface IProject(IEnumerable):
	Name as string:
		get
	
	Navigator as IProjectNavigator:
		get
	
	ProjectFile as string:
		get
	
	Location as string:
		get
		set
	
	Files as ICollection:
		get
	
	Folders as ICollection:
		get
	
	def Contains(fileName as string) as bool
	def GetFilesFromSubdir(subdir as string) as IList
	def GetFilesWithExtension(extensions as IList) as IList
	def GetFilesWithoutExtension(extensions as IList) as IList
	def NewFile(relPath as string) as IProjectFile
	def Clear()
	def AddFile(file as IProjectFile)
	def NewFolder(path as string)
	def ImportMatchingFiles(pattern as Regex, discardHidden as bool)
	def ImportFiles(extensions as IList, discardHidden as bool)
	def AddFolder(path as string)
	def ImportFiles(discardHidden as bool)
	def RemoveFile(file as string)
	def RemoveFile(file as string, delete as bool)
	def RemoveDir(dir as string)
	def RemoveDir(dir as string, delete as bool)
	def BeginInit()
	def EndInit()
	def Save()
	def AddListener ([required]listener as IProjectEventListener,
				evt as ProjectEventType)
	def RemoveListener ([required]listener as IProjectEventListener,
					evt as ProjectEventType)


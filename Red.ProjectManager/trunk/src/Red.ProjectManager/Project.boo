namespace Red.ProjectManager

import System
import System.Xml
import System.IO
import System.Collections
import Red.IO from "Red"

class Project (IEnumerable):

	fileCollection as Hashtable
	_folderCollection = []

	_watcher as FileSystemWatcher
	_beginInit = false

	public Name as string

	def constructor ([required]name as string, [required]location as string):
		_location = location
		Name = name
		Init ()
	
	private def Init ():
		file = Path.Combine (Location, (Name + ".simplep").ToLower ())
		if not File.Exists (file):
			stream = File.Create (file)
			stream.Close ()
		fileCollection = Hashtable ()
		_projectFile = file
		_navigator = ProjectNavigator (self)

	public event FileAdded as callable (object, FileChangedArgs)
	public event FileRemoved as callable (object, FileChangedArgs)
	public event DirRemoved as callable (object, DirRemovedArgs)
	public event ContentsChanged as callable (object, EventArgs)
	public event Cleared as EventHandler

	_navigator as ProjectNavigator
	public Navigator as ProjectNavigator:
		get:
			return _navigator

	_projectFile as string
	public ProjectFile as string:
		get:
			assert File.Exists (_projectFile)
			return _projectFile
		
	_location as string
	public Location as string:
		get:
			return _location
		set:
			assert value != null
			assert Directory.Exists (value)
			if value[-1:] == Path.DirectorySeparatorChar.ToString ():
				_location = value[:-1]
			else:
				_location = value

	public Files as ICollection:
		get:
			return fileCollection.Values
	
	public Folders as ICollection:
		get:
			return _folderCollection

	def Contains ([required]fileName as string) as bool:
		return fileCollection.Contains (fileName)

	//Move this out of here
	def GetFilesFromSubdir ([required]subdir as string) as IList:
		dir = Path.Combine (Location, subdir)
		if dir[-1:] == Path.DirectorySeparatorChar.ToString ():
			dir = dir[:-1]
		
		files as IList = []
		for file as ProjectFile in Files:
			dirname = System.IO.Path.GetDirectoryName (file.FullName)
			files.Add (file) if dirname == dir
		return files

	//Move this out of here
	def GetFilesWithExtension (extensions as IList) as IList:
		files as IList = []
		for file as ProjectFile in fileCollection.Values:
			if Path.GetExtension (file.Name) in extensions:
				files.Add (file)
		return files
	
	//Move this out of here
	def GetFilesWithoutExtension (extensions as IList) as IList:
		files as IList = []
		for file as ProjectFile in fileCollection.Values:
			if not (Path.GetExtension (file.Name) in extensions):
				files.Add (file)
		return files

	def Clear ():
		fileCollection.Clear ()
		Cleared (self, EventArgs.Empty) if Cleared
		ContentsChanged (self, EventArgs.Empty) if ContentsChanged
		Save ()

	def NewFile ([required]relPath as string) as ProjectFile:
		fullPath = System.IO.Path.Combine (Location, relPath)
		if File.Exists (fullPath):
			raise ArgumentException ("File already exists")
		stream as FileStream = File.Create (fullPath)
		stream.Close ()
		file as ProjectFile = Red.ProjectManager.ProjectFile(fullPath)
		AddFile (file)
		return file

	def AddFile ([required]file as ProjectFile):
		if not File.Exists (file.FullName):
			raise FileNotFoundException ("File does not exist", file.FullName)
		fileCollection.Add (file.FullName, file)
		if not _beginInit:
			FileAdded (self, FileChangedArgs (file.FullName)) if FileAdded
			ContentsChanged (self, EventArgs.Empty) if ContentsChanged
			Save ()
	
	def NewFolder ([required]path as string):
		if not path.StartsWith (Location):
			raise ArgumentException ("Path is not inside the project tree.")
		if Directory.Exists (path) or File.Exists (path):
			raise IOException ("Cannot create the folder.")
		Directory.CreateDirectory (path)
		_folderCollection.Add (path)
		Save ()
			

	def AddFolder ([required]path as string):
		if not Directory.Exists (path):
			raise FileNotFoundException ("Folder does not exist", path)
		_folderCollection.Add (path)
		//FIXME Add events for folders
		#FileAdded (self, FileChangedArgs (file.FullName)) if FileAdded
		if not _beginInit:
			ContentsChanged (self, EventArgs.Empty) if ContentsChanged
			Save ()
	
	def ImportMatchingFiles (pattern as regex, discardHidden as bool):
		BeginInit ()
		for file as string in FileUtils.GetAllDirectoryFiles (Location, discardHidden):
			//HACK: Dunno how to ask if a File is a directory
			if not Directory.Exists (file):
				if pattern:
					if file =~ pattern:
						if not fileCollection.Contains (file):
							AddFile (Red.ProjectManager.ProjectFile (file))
				else:
					if not fileCollection.Contains (file):
						AddFile (Red.ProjectManager.ProjectFile (file))
		EndInit ()
		Save ()

	// Import a list of files that matches the extension list
	// each extension in the list should not have the dot 
	// (i.e. .txt is wrong, txt is right)
	def ImportFiles ([required]extensions as IList, discardHidden as bool):
		BeginInit ()
		
		for file as string in FileUtils.GetAllDirectoryFiles (Location, discardHidden):
			//HACK: Dunno how to ask if a File is a directory
			if not Directory.Exists (file):
				if not fileCollection.Contains (file):
					//Stript the dot from the extension
					fname = Path.GetFileName (file)
					extension = Path.GetExtension (file)[1:]
					if (extension in extensions) or (fname in extensions):
						AddFile (Red.ProjectManager.ProjectFile (file)) 
		EndInit ()
		Save ()
		
	def ImportFiles (discardHidden as bool):
		ImportMatchingFiles (null, discardHidden)

	def GetEnumerator () as IEnumerator:
		return fileCollection.Values.GetEnumerator ()

	def RemoveFile ([required]file as string):
		RemoveFile (file, false)
	
	def RemoveFile ([required]file as string, delete as bool):
	""" Can raise a IOException
	
	"""
		if not fileCollection.Contains (file):
			raise ArgumentException ("No such file in the project.")
		fileCollection.Remove (file)
		if delete:
			File.Delete (file)
		FileRemoved (self, FileChangedArgs (file)) if FileRemoved
		ContentsChanged (self, EventArgs.Empty) if ContentsChanged
		Save ()

	def RemoveDir ([required]dir as string):
		RemoveDir (dir, false)

	def RemoveDir ([required]dir as string, delete as bool):
		files = []
		for file as ProjectFile in fileCollection.Values:
			if file.FullName.StartsWith (dir):
				files.Add (file.FullName)

		for f as string in files:
			fileCollection.Remove (f)
			if delete:
				File.Delete (f)

		if files.Count > 0:
			DirRemoved (self, DirRemovedArgs(dir, files)) if DirRemoved
			ContentsChanged (self, EventArgs.Empty) if ContentsChanged
		Save ()
	
	def BeginInit ():
		_beginInit = true

	def EndInit ():
		_beginInit = false
		ContentsChanged (self, EventArgs.Empty) if ContentsChanged
		

	def Save ():

		stream = StreamWriter (ProjectFile)
		writer = XmlTextWriter (stream)
		writer.Formatting = Formatting.Indented
		writer.Indentation = 4
		writer.WriteStartDocument ()
		writer.WriteStartElement (null, "Project", null)
		writer.WriteStartElement (null, "Name", null)
		writer.WriteString (Name)
		writer.WriteEndElement ()
		writer.WriteStartElement (null, "FileSet", null)
		if fileCollection.Count == 0:
			writer.WriteString (Environment.NewLine)
		else:
			for file as ProjectFile in self:
				writer.WriteStartElement (null, "File", null)
				writer.WriteStartElement (null, "Path", null)
				writer.WriteString (file.FullName)
				writer.WriteEndElement ()
				writer.WriteEndElement ()

		#fileset
		writer.WriteEndElement ()
		writer.WriteStartElement (null, "FolderSet", null)
		if _folderCollection.Count == 0:
			writer.WriteString (Environment.NewLine)
		else:
			for dir as string in _folderCollection:
				writer.WriteStartElement (null, "Folder", null)
				writer.WriteStartElement (null, "Path", null)
				writer.WriteString (dir)
				writer.WriteEndElement ()
				writer.WriteEndElement ()
		#folderset
		writer.WriteEndElement ()
		#project
		writer.WriteEndElement ()
		writer.WriteEndDocument ()
		writer.Close ()
		stream.Close ()

class FileChangedArgs (EventArgs):
	def constructor ([required]file as string):
		File = file

	File as string = null

class DirRemovedArgs (EventArgs):
	def constructor ([required]dirName as string, [required]files as IList):
		DirName = dirName
		Files = files
	
	DirName as string
	Files as IList

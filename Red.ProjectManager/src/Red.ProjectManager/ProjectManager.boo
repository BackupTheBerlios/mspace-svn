namespace Red.ProjectManager

import System
import System.IO
import System.Collections
import System.Xml
import System.Reflection
import Red.IO from "Red"

class ProjectManager:

	projects as Hashtable = Hashtable ()
	appDir as string = null
	knownProjectsFile as string = null
	final PROJECT_FILE_EXT = ".simplep"

	private def constructor ():
		configDir = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData)
		appDir = Path.Combine (configDir, "SimpleP")
		knownProjectsFile = Path.Combine (appDir, "projects.xml")
		InitConfigDir ()
		LoadKnownProjects ()

	public event ProjectChanged as EventHandler
	public event ProjectAdded as callable (object, ProjectAddedArgs)
	public event ProjectRemoved as callable (object, ProjectRemovedArgs)

	_missingFiles as IList
	public MissingFiles as IList:
		get:
			return _missingFiles

	_missingFolders as IList
	public MissingFolders as IList:
		get:
			return _missingFolders

	_currentProject as Project = null
	public CurrentProject as Project:
		get:
			return _currentProject
		set:
			_currentProject = value
			ProjectChanged (self, EventArgs.Empty) 

	public Projects as ICollection:
		get:
			return projects.Keys

	def GetProjectFile (name as string) as string:
		if not projects[name]:
			raise ArgumentException ("The project does not exist.")
		return projects[name]

	def NewProject ([required]name as string, [required]location as string, createDir as bool) as Project:
		if projects.Contains (name):
			raise ArgumentException ("Project name already exists.")

		project as Project
		fullPath = Path.GetFullPath (location)
		if createDir:
			if Directory.Exists (location):
				raise ArgumentException ("The directory already exists.: " + Path.GetFullPath (location))
			
			Directory.CreateDirectory (location)
		
		else:
			if not Directory.Exists (location):
				raise DirectoryNotFoundException ("The directory does not exists.")
		project = Project (name, fullPath)
		project.Save ()

		projects.Add (project.Name, project.ProjectFile)
		WriteProjectsCache ()
		CurrentProject = project
		ProjectAdded (self, ProjectAddedArgs (project)) 
		return project

	def SetActiveProject ([required]name as string):
		if not Contains (name):
			raise ArgumentException ("Project ${name} does not exits.")
			
		file = GetProjectFile (name)
		CurrentProject = LoadProject (file)

	def Contains (name as string) as bool:
		return projects.Contains (name)
	
	//raises FileNotFoundException if file not found
	/*
	 * Load a project a project file of returns the project
	 * instance if it was already loaded.
	 */
	def LoadProject ([required]file as string) as Project:
		reader = XmlTextReader (file)
		_missingFiles = null
		pName as string
		pLocation  = Path.GetDirectoryName (file)
		files = []
		try:
			lastElement as string
			ms = DateTime.Now.Millisecond
			while reader.Read ():
				reader.MoveToElement ()
				if reader.NodeType == XmlNodeType.Element:
					lastElement = reader.Name
				if reader.NodeType == XmlNodeType.Text and lastElement == "Name":
					pName = reader.Value
				elif reader.NodeType == XmlNodeType.Text and lastElement == "Path":
					files.Add (reader.Value)
			
			reader.Close ()
			ms = Environment.TickCount
			project = Project (pName, pLocation)
			project.BeginInit ()
			for path as string in files:
				try:
					project.AddFile (ProjectFile (path))
				except ex as FileNotFoundException:
					if not _missingFiles:
						_missingFiles= []
					_missingFiles.Add (ex.FileName)
			project.EndInit ()
		
		except ex as Exception:
			raise InvalidProjectFileException ("Error reading FileSet section in project ${pName}")
		
		if not projects.Contains (project.Name):
			projects.Add (project.Name, project.ProjectFile)
			ProjectAdded (self, ProjectAddedArgs (project))
			WriteProjectsCache ()
		
		CurrentProject = project
		return project
		
	def RemoveProject ([required]name as string):
		if not projects.Contains (name):
			raise ArgumentException ("project ${name} does not exist")
		projects.Remove (name)
		WriteProjectsCache ()
		#ProjectRemoved (self, ProjectRemovedArgs (prj) if ProjectRemoved
	
	def RemoveCurrentProject ():
		prj = CurrentProject
		projects.Remove (CurrentProject.Name)
		CurrentProject = null
		WriteProjectsCache ()
		ProjectRemoved (self, ProjectRemovedArgs (prj)) 
			
	private def LoadKnownProjects ():
		document = XmlDocument ()
		if File.Exists (knownProjectsFile):
			document.Load (knownProjectsFile)
			for projectNode as XmlNode in document.SelectNodes ("/Projects/Project"):
				location = projectNode["Location"].InnerText
				name = projectNode["Name"].InnerText
				if File.Exists (location):
					projects.Add (name, location)
					
		
	private def InitConfigDir ():
		if not Directory.Exists (appDir):
			Directory.CreateDirectory (appDir)
		
	private def WriteProjectsCache ():
		stream = StreamWriter (knownProjectsFile)
		writer = XmlTextWriter (stream)
		writer.Formatting = Formatting.Indented
		writer.Indentation = 4
		writer.WriteStartDocument ()
		writer.WriteStartElement (null, "Projects", null)
		
		if projects.Count == 0:
			writer.WriteString (Environment.NewLine)
		else:
			for entry as DictionaryEntry in projects:
				writer.WriteStartElement (null, "Project", null)
				writer.WriteStartElement (null, "Location", null)
				writer.WriteString (entry.Value)
				writer.WriteEndElement ()
				writer.WriteStartElement (null, "Name", null)
				writer.WriteString (entry.Key)
				writer.WriteEndElement ()
				writer.WriteEndElement ()

		writer.WriteEndElement ()
		writer.WriteEndDocument ()
		writer.Close ()
		stream.Close ()

class ProjectAddedArgs (EventArgs):
	Project as Project
	def constructor ([required]prj as Project):
		Project = prj

class ProjectRemovedArgs (EventArgs):
	Project as Project
	def constructor ([required]prj as Project):
		Project = prj

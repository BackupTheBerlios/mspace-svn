namespace Red.IO

import System
import System.IO
import System.Diagnostics
import System.Threading
import System.Collections

public class FileUtils:
		
	static def GetAllSubdirectories (dir as string) as IList:
		return GetAllSubdirectories (dir, ArrayList ())
	
	private	static def GetAllSubdirectories (dir as string, dirs as ArrayList) as ArrayList:
		//Recurse through the directories
		for d in Directory.GetDirectories (dir):
			GetAllSubdirectories (d, dirs)
			dirs.Add (d)
		
		return dirs
	
	private	static def GetAllDirectoryFiles (dir as string, rx as regex, files as ArrayList) as ArrayList:
		//Recurse through the directories
		for d in Directory.GetDirectories (dir):
			for f in Directory.GetFiles (d):
				if rx:
					if f =~ rx:
						files.Add (f)
				else:
					files.Add (f)
			GetAllDirectoryFiles (d, rx, files)
		
		//Add the files from the top directory
		for f in Directory.GetFiles (dir):
			if rx:
				if f =~ rx:
					files.Add (f)
			else:
				files.Add (f)
			
		return files
	

	static def GetAllDirectoryFiles (dir as string) as IList:
		return GetAllDirectoryFiles (dir, null, ArrayList ())
	
	static def GetAllDirectoryFiles (dir as string, rx as regex) as IList:
		return GetAllDirectoryFiles (dir, rx, ArrayList ())

/*
	Copyright (c) 2005 Sergio Rubio, <sergio.rubio@hispalinux.es>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace Red.ProjectManager

import System
import Red.IO from Red
import System.Collections
import System.IO

class ProjectNavigator:

	dirs = {}
	sep = System.IO.Path.DirectorySeparatorChar
	backHistory = Stack ()
	forwardHistory = Stack ()
	_project as Project
	_root as ProjectNode

	private def constructor ():
		pass

	internal def constructor ([required]project as Project):
		_project = project
		_current = _project.Location
		_project.ContentsChanged += ProjectContentsChanged
		Init ()	

	_current as string
	public CurrentPath as string:
		get:
			assert _current != null
			assert _current.StartsWith (_project.Location)
			return _current
	
	public CurrentSubdirs as IList:
		get:
			return GetCurrentNodes ()
						
	public CurrentFiles as IList:
		get:
			return _project.GetFilesFromSubdir (_current)

	public HasParent as bool:
		get:
			return _current != _project.Location

	def MoveUp ():
		if not HasParent:
			return
			
		backHistory.Push (_current)
		MoveToSubdir (GetParent (_current))

	public HasNext as bool:
		get:
			return forwardHistory.Count > 0

	public HasPrevious as bool:
		get:
			return backHistory.Count > 0

	def Next ():
		if not HasNext:
			return
		_current = forwardHistory.Peek () as string
		backHistory.Push (forwardHistory.Pop)

	def Previous ():
		if not HasPrevious:
			return
		_current = backHistory.Peek () as string
		forwardHistory.Push (backHistory.Pop)
		
	def MoveToSubdir ([required]dir as string):
		assert dir != string.Empty
		assert dir.StartsWith (_project.Location)
		backHistory.Push (_current)
		_current = dir 

	public Parent as string:
		get:
			if not HasParent:
				return _project.Location
			return GetParent (_current)

	private def GetParent (path as string) as string:
		path2 as string
		if path.EndsWith (sep.ToString ()):
			path2 = System.IO.Path.GetDirectoryName (path)
			path2 = System.IO.Path.GetDirectoryName (path2)
		else:
			path2 = System.IO.Path.GetDirectoryName (path)
		return path2


	private def Init ():
		BuildNodes ()
		forwardHistory.Clear ()
		backHistory.Clear ()
	
	private def BuildNodes ():
		#time = DateTime.Now.Millisecond
		_root = ProjectNode ("/")
		for file as ProjectFile in _project.Files:
			dirname = Path.GetDirectoryName (file.FullName).Replace (_project.Location, "")
			elements = PathUtils.GetPathElements (dirname)
			currentNode = _root
			for e in elements:
				if currentNode.Childs.Count == 0:
					n = ProjectNode (e)
					currentNode.Childs.Add (n)
					currentNode = n
				else:
					for i in range (currentNode.Childs.Count):
						node = currentNode.Childs[i] as ProjectNode
						if node.Name == e:
							currentNode = node
							break;
						if i == currentNode.Childs.Count -1:
							n = ProjectNode (e)
							currentNode.Childs.Add (n)
							currentNode = n
		#print endTime = DateTime.Now.Millisecond - time
		

	private	def GetCurrentNodes () as IList:
		dirname = CurrentPath.Replace (_project.Location, "")
		elements = PathUtils.GetPathElements (dirname)
		currentNode = _root
		for e in elements:
			for i in range (currentNode.Childs.Count):
				node = currentNode.Childs[i] as ProjectNode
				if node.Name == e:
					currentNode = node
					break;
		list = []
		for node as ProjectNode in currentNode.Childs:
			list.Add (node.Name)
		return list

	private def ProjectContentsChanged (sender, args):
		BuildNodes ()

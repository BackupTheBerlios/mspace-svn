/*
	Copyright (c) 2005 Sergio Rubio, <sergio.rubio@hispalinux.es>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace SimpleP

import System
import Red.IO 
import System.IO
import System.Collections

class PathNavigator:

	dirs as IDictionary
	sep = Path.DirectorySeparatorChar
	backHistory = Stack ()
	forwardHistory = Stack ()

	def constructor ([required]root as string):
		dirs = {}
		Init (root)
		

	internal def constructor ([required]root as string, [required]dic as IDictionary):
		if root == string.Empty:
			raise InvalidPathException ("Path cannot be an empty string.")
		if root.EndsWith (sep.ToString ()):
			_root = root[:-1]
		else:
			_root = root
		_current = _root

		dirs = dic
			
	internal Directories as IDictionary:
		get:
			return dirs

	_root as string
	public Root as string:
		get:
			return _root

	_current as string
	public Current as string:
		get:
			assert _current != null
			return _current
	
	public HasParent as bool:
		get:
			return _current != _root

	def MoveUp ():
		if not HasParent:
			return
			
		backHistory.Push (_current)
		_current = GetParent (_current)

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
	"""dir cannot start with / character
	"""
		assert dir != string.Empty
		assert dir[0] != Path.DirectorySeparatorChar
		if dir.EndsWith (sep.ToString ()):
			dir = dir[:-1]
		relPath = Path.Combine (_current, dir).Replace (_root + sep.ToString (), "")
		sdir = dirs[relPath]
		if not sdir:
			raise InvalidPathException ("Not a valid subdirectory: ${relPath}")
		backHistory.Push (_current)
		_current = sdir

	public Parent as string:
		get:
			if not HasParent:
				return _root
			return GetParent (_current)
	

	private def GetParent (path as string) as string:
		path2 as string
		if path.EndsWith (sep.ToString ()):
			path2 = System.IO.Path.GetDirectoryName (path)
			path2 = System.IO.Path.GetDirectoryName (path2)
		else:
			path2 = System.IO.Path.GetDirectoryName (path)
		return path2


	def Init ([required] root as string):
		if root == string.Empty:
			raise InvalidPathException ("Path cannot be an empty string.")
		if root.EndsWith (sep.ToString ()):
			_root = root[:-1]
		else:
			_root = root
		_current = _root
		
		dirs.Clear ()
		for dir as string in FileUtils.GetAllSubdirectories (root):
			dirs.Add (dir.Replace (root + sep.ToString (), ""), dir)

		forwardHistory.Clear ()
		backHistory.Clear ()

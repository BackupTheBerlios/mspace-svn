namespace Red.IO

import System
import System.IO
import System.Collections

class PathUtils:

	static def CountPathElements ([required]path as string) as int:
		sep = Path.DirectorySeparatorChar
		if path[0] != sep:
			raise ArgumentException ("Path must be a valid full path starting with / or \\")
		return GetPathElements (path).Count

	static def GetPathElement (path as string, index as int) as string:
		elements = GetPathElements (path)
		if index >= elements.Count:
			raise IndexOutOfRangeException ("Index is greater than the number of elements in the path.")
		if index < 0:
			raise ArgumentException ("Index must be >= than 0.")
		return elements[index]


	public static def GetPathElements (path as string) as IList:
		sep = Path.DirectorySeparatorChar
		elements = []
		tokens = path.Trim ().Split ( (sep,) )
		for s as string in tokens:
			if s != string.Empty:
				elements.Add (s)
		return elements
		

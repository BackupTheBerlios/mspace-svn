namespace SimpleP

import Gtk
import System

class ProjectStore (ListStore):
	
	public final TextCol = 0
	public final IconCol = 1
	public final FullNameCol = 2
	public final IsDirectoryCol = 3

	def constructor ( types as (Type) ):
		super (types)
		SetDefaultSortFunc (SortFunc, IntPtr.Zero, null)
		SetSortColumnId (-1, SortType.Ascending)

	private def SortFunc (model as TreeModel, iterA as TreeIter, iterB as TreeIter) as int:
		o1 as string = GetValue (iterA, FullNameCol)
		o2 as string = GetValue (iterB, FullNameCol)
		name1 = GetValue (iterA, TextCol)
		name2 = GetValue (iterB, TextCol)
		isDir1 = GetValue (iterA, IsDirectoryCol)
		isDir2 = GetValue (iterB, IsDirectoryCol)

		if name1 == "../":
			return -1
		if name2 == "../":
			return 1

		fname1 = System.IO.Path.GetFileName (o1)
		fname2 = System.IO.Path.GetFileName (o2)
		
		if isDir1 and not isDir2:
			return -1
		
		elif not isDir1 and isDir2:
			return 1
		else:
			return string.Compare (fname1, fname2, true)


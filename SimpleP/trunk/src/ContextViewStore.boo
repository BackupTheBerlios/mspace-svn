namespace SimpleP

import Gtk
import System

class ContextViewStore (ListStore):
	
	def constructor ( types as (Type) ):
		super (types)
		SetDefaultSortFunc (SortFunc, IntPtr.Zero, null)
		SetSortColumnId (-1, SortType.Ascending)

	private def SortFunc (model as TreeModel, iterA as TreeIter, iterB as TreeIter) as int:
		o1 as string = GetValue (iterA, 1)
		o2 as string = GetValue (iterB, 1)
		if o1 == "Start page":
			return -1
		if o2 == "Start page":
			return 1

/*
 * Copyright (C) 2004 Jorn Baayen <jorn@nl.linux.org>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public
 * License along with this program; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

using System;
using System.Collections;

using Gtk;
using GLib;

/*
 * DataView is a custom TreeView capable of displaying songs in the view
 * that matches a search criteria.
 */
public abstract class DataView : TreeView
{

	protected DataStore store;

	public DataView (DataStore model) : base (model)
	{
	    this.store = model;
	    InitComponent ();
	}

	private void InitComponent ()
	{
	    foreach (TreeViewColumn col in GetColums ())
		AppendColumn (col);
	    HeadersVisible = false;
	    Selection.Mode = SelectionMode.Multiple;
	    Reorderable = true;
	}

	protected abstract TreeViewColumn[] GetColums ();
	
	
	public bool Search (string search)
	{
		//SongsStore.RemoveDelta (l);
		store.Clear ();

		store.Search (search);
		
		TreeIter iter;
		if (Model.GetIterFirst (out iter))
		    Selection.SelectIter (iter);

		return false;
	}

	/**** PROPERTIES ****/
	
	public abstract string Title {get;}

}

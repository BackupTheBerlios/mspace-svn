/*
 * Copyright (C) 2004 Sergio Rubio <sergio.rubio@hispalinux.es>
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

using Gtk;
using System;
using System.Collections;


/*
 * FIXME: This should'n be dependant on Gtk, refactore and use
 * a corlib collections class.
 */
public abstract class DataStore : ListStore
{
	private const int FakeLength = 150;

	public DataStore (params Type[] types) : base (types){}

	/* 
	 * Returns an array containing the media matching the string search
	 */
	public void Search (string search)
	{
	    ICollection list;
	    if (search != string.Empty)
	    {
		ArrayList mediaList = new ArrayList ();

		int max_len = -1;

		/* show max. FakeLength songs if < 3 chars are entered. this is to fake speed. */
		if (search.Length < 3)
			max_len = FakeLength;

		int i = 0;
		if (search.Length > 0) {
			string [] search_bits = search.ToLower ().Split (' ');

			foreach (ISearchable s in Media) {
				if (s.FitsCriteria (search_bits)) {
					mediaList.Add (s);
				
					i++;
					if (max_len > 0 && i >= max_len)
						break;
				}	
			}
		} else {
			foreach (ISearchable s in Media) {
				mediaList.Add (s);
				i++;
				if (max_len > 0 && i >= max_len)
					break;
			}
		}
		list = mediaList;
	    } else
		list = Media; 
	    
	    foreach (object obj in list)
		AppendMedia (obj);
	}
	
	public abstract ICollection Media {get;} 

	public abstract TreeIter AppendMedia (object obj);
}


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

namespace Muine.Sdk.Data
{
    using System;
    using System.Collections;

    public class Album : ISearchable
    {
	    private string name;
	    public string Name {
		    get {
			    return name;
		    }
	    }

	    public ArrayList Songs;

	    private ArrayList artists;
	    public string [] Artists {
		    get {
			    return (string []) artists.ToArray (typeof (string));
		    }
	    }

	    private ArrayList performers;
	    public string [] Performers {
		    get {
			    return (string []) performers.ToArray (typeof (string));
		    }
	    }

	    private string year;
	    public string Year {
		    get {
			    return year;
		    }
	    }

	    private static string [] prefixes = null;

	    public Album (Song initial_song)
	    {
	    }

	    public bool AddSong (Song song)
	    {
		return false;
	    }

	    public bool RemoveSong (Song song)
	    {
		return false;
	    }

	    public bool FitsCriteria (string [] search_bits)
	    {
		return false;
	    }
    }
}

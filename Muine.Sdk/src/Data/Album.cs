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

namespace Muine.Sdk.Data
{
    using System;
    using System.Collections;

    /// 
    /// Readonly copy of an Album in the Music Database.
    /// Modifications to this class are ignored. They are not
    /// persisted into the database.
    /// 
    public class Album : ISearchable, IComparable
    {
	public Album (AlbumMetadata metadata)
	{
	    name = metadata.Name;
	    songs = metadata.Songs;
	    artists = metadata.Artists;
	    performers = metadata.Performers;
	    year = metadata.Year;
	    
	}
	
	private string name = "UNKNOWN";
	public string Name {
		get {
			return name;
		}
	}

	private ArrayList songs = new ArrayList ();
	public ArrayList Songs {
	    get {
		return songs;
	    }
	}

	private ArrayList artists = new ArrayList ();
	public ArrayList Artists {
		get {
			return artists;
		}
	}

	private ArrayList performers = new ArrayList ();
	public ArrayList Performers {
		get {
		    return performers;
		}
	}

	private int year = 1900;
	public int Year {
		get {
			return year;
		}
	}

	private IAlbumComparer comparer = new SimpleAlbumComparer ();
	public IAlbumComparer Comparer {
	    get {
		return comparer;
	    }
	    set {
		comparer = value;	
	    }
	}

	public bool FitsCriteria (string [] search_bits)
	{
	    throw new NotImplementedException ("Not implemented");
	    return false;
	}

	public override bool Equals (object obj)
	{
	    if (obj.GetType () != this.GetType ())
		throw new ArgumentException ("Album:Equals: object is not an Album");

	    if (comparer.Compare (this, (Album)obj) == 0) 
		return true;
	    else return false;
	}

	public int CompareTo (object obj)
	{
	    if (obj.GetType () != this.GetType ())
		throw new ArgumentException ("Album:CompareTo: object is not an Album");

	    return comparer.Compare (this, (Album)obj);
	}

	public static int Compare (Album one, Album two)
	{
	    return new SimpleAlbumComparer ().Compare (one, two);
	}
    }

    public class SimpleAlbumComparer : IAlbumComparer
    {
	public int Compare (Album one, Album two)
	{
	    return one.Name.ToLower ().CompareTo (two.Name.ToLower ());
		
	}
    }
}

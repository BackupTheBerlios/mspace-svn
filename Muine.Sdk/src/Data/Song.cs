/*
 * Copyright (C) 2003, 2004 Jorn Baayen <jorn@nl.linux.org>
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
    using System.IO;

    /// 
    ///	Readonly copy of a Song in the music database.
    /// Modifications to this class are ignored. They are not
    /// persisted into the database
    /// 
    public class Song : ISearchable, IComparable
    {
	public Song (SongMetadata metadata)
	{
	    filename = metadata.Filename;
	    title = metadata.Title;
	    artists = metadata.Artists;
	    performers = metadata.Performers;
	    album = metadata.Album;
	    tracknumber = metadata.TrackNumber;
	    year = metadata.Year;
	    mtime = metadata.MTime;
	    duration = metadata.Duration;
	    gain = metadata.Gain;
	    peak = metadata.Peak;
	    
	    if (filename == null)
		throw new ArgumentException ("Song:Constructor: metadata filename is null");
	}

	public Song (String filename)
	{
	    if (filename == null)
		throw new ArgumentException ("Song:Constructor: metadata filename is null");
	    this.filename = filename;
	}
    
	private string filename;
	public string Filename {
		get {
			return filename;
		}
	}
		
	private string title;
	public string Title {
		get {
			if (title == null)
				return Path.GetFileNameWithoutExtension (filename);
			return title;
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

	private string album = "";
	public string Album {
		get {
			return album;
		}
	}

	private int tracknumber = 0;
	public int TrackNumber {
		get {
			return tracknumber;
		}
	}

	private int year = 1900;
	public int Year {
		get {
			return year;
		}
	}

	private int duration = 0;
	public int Duration {
		get {
			return duration;
		}
	}

	private int mtime = 0;
	public int MTime {
		get {
			return mtime;
		}
	}

	private double gain = 0;
	public double Gain {
		get {
			return gain;
		}
	}

	private double peak = 0;
	public double Peak {
		get {
			return peak;
		}
	}

	private ISongComparer comparer = new SimpleSongComparer ();
	public ISongComparer Comparer {
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
	}

	public override bool Equals (object obj)
	{
	    if (obj.GetType () != this.GetType ())
		throw new ArgumentException ("Song:Equals: object is not a Song");

	    if (comparer.Compare (this, (Song)obj) == 0) 
		return true;
	    else return false;
	}

	public int CompareTo (object obj)
	{
	    if (obj.GetType () != this.GetType ())
		throw new ArgumentException ("Song:CompareTo: object is not a Song");

	    return comparer.Compare (this, (Song)obj);
	}

	public int Compare (Song one, Song two)
	{
	    return new SimpleSongComparer ().Compare(one, two);
	}
    }

    public class SimpleSongComparer : ISongComparer 
    {
	public int Compare (Song one, Song two)
	{
	    if (one.Title.ToLower () == two.Title.ToLower ())
	    {
		if (one.Filename == two.Filename)
		    return 0;
	    } else {
		if (one.Filename == two.Filename)
		    return 0;
	    }
	    return one.Filename.CompareTo (two.Filename);
	}
    }
}

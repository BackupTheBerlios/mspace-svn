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

using System.Collections;
using System;
using GLib;

/* 
 * Custom ArrayList that fires events when the content is modified
 */

public delegate void SongAddedDelegate (object obj, Song song);
public delegate void SongRemovedDelegate (object obj, Song song);
public delegate void ClearedDelegate (object obj);
public delegate void SongChangedDelegate (object obj, Song song);

public class Playlist : ArrayList
{

    private int currentSong = 0;
    
    public Playlist ()
    {
    }

    public void Append (Song song)
    {
	Add (song);
    }

    /*
     * FIXME: Check if the song is overwritten
     */
    public void Prepend (Song song)
    {
	Insert (0, song);
	currentSong++;
    }

    public override int Add (object obj)
    {
	CheckSong (obj);
	if (SongAdded != null)
	    SongAdded (this, obj as Song);
	return base.Add (obj);
    }

    public override void Insert (int index, object obj)
    {
	CheckSong (obj);
	if (SongAdded != null)
	    SongAdded (this, obj as Song);
	base.Insert (index, obj);	
	if (index <= currentSong)
	    currentSong++;
    }

    public override void Remove (object obj)
    {
	int index = IndexOf (obj);
	if (index != -1)
	{
	    if (SongRemoved != null)
		SongRemoved (this, obj as Song);
	    base.RemoveAt (index);
	    if (index == currentSong || Count == 0)
		currentSong = 0;
	    else if (index < currentSong)
		currentSong--;
	}
    }

    public override void Clear ()
    {
	if (Cleared != null)
	    Cleared (this);
	base.Clear ();
	currentSong = 0;
    }

    public override void AddRange (ICollection col)
    {
	try {
	    foreach (Song s in col)
		Add (s);
	} catch {
	    throw new ArgumentException ("You should only append Songs to the playlist");
	}
    }

    public Song Current {
	get {
	    if (Count == 0)
		return null;
	    return this[currentSong] as Song;
	}
	set {
	    int index = IndexOf (value);
	    if (index != -1)
	    {
		currentSong = index;
		if (SongChanged != null)
		    SongChanged (this, value);
	    }
	    else
		Console.WriteLine ("WARNING: Invalid current item in playlist");
		
	}
    }

    public Song First {
	get {
	    if (Count == 0)
		return null;
	    return this[0] as Song;
	}
    }

    /*
     * Forwards to the next song available.
     * If there is no next song, returns null;
     */
    //FIXME: Events
    public Song Next {
	get {
	    if (currentSong == Count - 1 || Count == 0)
		return null;
	    return this[currentSong] as Song;
	}
    }

    /*
     * Back to the previous song.
     * If there is no previous song, returns null;
     */
    public Song Previous {
	get {
	    if (currentSong == 0 || Count == 0)
		return null;
	    return this[currentSong] as Song;
	}
    }

    public Song Last {
	get {
	    if (Count == 0)
		return null;
	    return this[Count -1] as Song;
	}
    }

    private void CheckSong (object obj)
    {
	if (!(obj is Song))
	    throw new ArgumentException ("You should only append Songs to the playlist");
    }

    public event SongAddedDelegate SongAdded;
    public event SongRemovedDelegate SongRemoved;
    public event ClearedDelegate Cleared;
    public event SongChangedDelegate SongChanged;

}


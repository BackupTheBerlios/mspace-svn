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

namespace Player.Playlist
{

    using System.Collections;
    using System;
    using Player.Data;

    /* 
     * Custom ArrayList that fires events when the content is modified
     */

    public delegate void SongAddedEventHandler (object obj, Song song);
    public delegate void SongRemovedEventHandler (object obj, Song song);
    public delegate void ClearedEventHandler (object obj);
    public delegate void SongChangedEventHandler (object obj, Song song);

    public class Playlist : IPlaylist
    {

	private ArrayList list = new ArrayList ();
	private int currentSong = 0;
	
	public int Add (object obj)
	{
	    CheckSong (obj);
	    if (SongAddedEvent != null)
		SongAddedEvent (this, obj as Song);
	    return list.Add (obj);
	}

	public void Insert (int index, object obj)
	{
	    CheckSong (obj);
	    if (SongAddedEvent != null)
		SongAddedEvent (this, obj as Song);
	    list.Insert (index, obj);	
	    if (index <= currentSong)
		currentSong++;
	}

	public void Remove (object obj)
	{
	    int index = IndexOf (obj);
	    if (index != -1)
	    {
		if (SongRemovedEvent != null)
		    SongRemovedEvent (this, obj as Song);
		list.RemoveAt (index);
		if (index == currentSong || list.Count == 0)
		    currentSong = 0;
		else if (index < currentSong)
		    currentSong--;
	    }
	}

	public void RemoveAt (int index)
	{
	    Song song = (Song)list[index];
	    list.RemoveAt (index);
	    if (SongRemovedEvent != null)
		SongRemovedEvent (this, song);
	}

	public void Clear ()
	{
	    if (ClearedEvent != null)
		ClearedEvent (this);
	    list.Clear ();
	    currentSong = 0;
	}

	public bool Contains (object obj)
	{
	    return list.Contains (obj);
	}

	public int IndexOf (object obj)
	{
	    return list.IndexOf (obj);
	}

	public bool IsReadOnly {
	    get {
		return list.IsReadOnly;
	    }
	}

	public bool IsFixedSize {
	    get {
		return list.IsFixedSize;
	    }
	}

	public int Count {
	    get {
		return list.Count;
	    }
	}

	public bool IsSynchronized {
	    get {
		return list.IsSynchronized;
	    }
	}

	public object SyncRoot {
	    get {
		return list.SyncRoot;
	    }
	}

	public void CopyTo (Array array, int index)
	{
	    list.CopyTo (array, index); 
	}

	public IEnumerator GetEnumerator ()
	{
	    return list.GetEnumerator ();
	}

	public Song Current {
	    get {
		if (list.Count == 0)
		    return null;
		return list[currentSong] as Song;
	    }
	    set {
		int index = IndexOf (value);
		if (index != -1)
		{
		    currentSong = index;
		    if (SongChangedEvent != null)
			SongChangedEvent (this, value);
		}
		else
		    Console.WriteLine ("WARNING: Invalid current item in playlist");
	    }
	}

	/*
	 * Forwards to the next song available.
	 * If there is no next song, returns null;
	 */
	//FIXME: Events
	public Song Next {
	    get {
		if (currentSong == list.Count - 1 || list.Count == 0)
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
		if (currentSong == 0 || list.Count == 0)
		    return null;
		return this[currentSong] as Song;
	    }
	}

	private void CheckSong (object obj)
	{
	    if (!(obj is Song))
		throw new ArgumentException ("You should only append Songs to the playlist");
	}

	public object this[int index] {
	    get {
		return list[index];
	    }
	    set {
		throw new NotImplementedException ();
	    }
	}
	public event SongAddedEventHandler SongAddedEvent;
	public event SongRemovedEventHandler SongRemovedEvent;
	public event ClearedEventHandler ClearedEvent;
	public event SongChangedEventHandler SongChangedEvent;

    }

}

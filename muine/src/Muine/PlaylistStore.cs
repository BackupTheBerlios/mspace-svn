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

using System;
using Gtk;
using System.Text;
using System.Collections;


public class PlaylistStore : DataStore
{

	private Playlist playlist = AppContext.Playlist;

	public PlaylistStore () : base (typeof (Gdk.Pixbuf), typeof (string)) 
	{
	    playlist.SongAdded += SongAdded;
	    playlist.SongRemoved += SongRemoved;
	    playlist.SongChanged += SongChanged;
	    playlist.Cleared += Cleared;
	    foreach (Song s in playlist)
		AppendMedia (s);

	}

	public override TreeIter AppendMedia (object obj)
	{
	    Song s = (Song) obj;
	    StringBuilder builder = new StringBuilder ();
	    builder.Append ("<b>");
	    builder.Append (s.Album);
	    builder.Append ("</b>");
	    builder.Append ("\n");
	    builder.Append (StringUtils.JoinHumanReadable (s.Artists));
	    Gdk.Pixbuf pixbuf = new Gdk.Pixbuf (null, "muine-nothing.png");
	    return base.AppendValues (pixbuf, builder.ToString ());
	}

	private void RemoveMedia (object obj)
	{
	    Song s = (Song) obj;
	    int index = playlist.IndexOf (s);
	    if (index != -1)
	    {
		TreeIter iter;
		GetIterFromString (out iter, index.ToString ());
		if (IterIsValid (iter))
		    Remove (ref iter);
		else
		    Console.WriteLine ("WARNING-PlaylistStore:Invalid iter.");
		    
	    }
	    else
		Console.WriteLine ("WARNING-PlaylistStore:Item not found in playlist.");
	}
    
	public override ICollection Media {
	    get {
		return AppContext.Playlist;
	    }

	}

	private void SongAdded (object obj, Song song)
	{
	    AppendMedia (song);
	}

	private void SongChanged (object obj, Song song)
	{
	    
	}
	
	private void SongRemoved (object obj, Song song)
	{
	    RemoveMedia (song);
	}
	
	private void Cleared (object obj)
	{
	    Clear ();
	}
}


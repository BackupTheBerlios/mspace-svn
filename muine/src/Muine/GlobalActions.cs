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
using Gtk.Ext;

public class GlobalActions
{
    public static readonly UIAction PlayPause;
    public static readonly UIAction Next;
    public static readonly UIAction Previous;
    public static readonly UIAction SkipTo;
    public static readonly UIAction SkipForward;
    public static readonly UIAction SkipBackwards;
    public static readonly UIAction AddSong;
    public static readonly UIAction AddAlbum;
    public static readonly UIAction RemoveSong;
    public static readonly UIAction RemovePlayedSongs;
    public static readonly UIAction ClearPlaylist;
    public static readonly UIAction Repeat;
    public static readonly UIAction ImportFolder;
    public static readonly UIAction OpenPlaylist;
    public static readonly UIAction SavePlaylistAs;
    public static readonly UIAction HideWindow;
    public static readonly UIAction Quit;

    static GlobalActions ()
    {
	PlayPause = new PlayAction ();
	Previous = new PreviousAction ();
	Next = new NextAction ();
	SkipTo = new SkipToAction ();
	SkipForward = new SkipDirectionAction (SkipDirectionAction.Direction.Forward);
	SkipBackwards = new SkipDirectionAction (SkipDirectionAction.Direction.Backwards);
	AddSong = new AddSongAction ();
	AddAlbum = new AddAlbumAction ();
	RemoveSong = new RemoveSongAction ();
	RemovePlayedSongs = new RemovePlayedSongsAction ();
	ClearPlaylist = new ClearPlaylistAction ();
	Repeat = new RepeatAction ();

	ImportFolder = new ImportFolderAction ();
	OpenPlaylist = new OpenPlaylistAction ();
	SavePlaylistAs = new SavePlaylistAsAction ();
	HideWindow = new HideWindowAction ();
	Quit = new QuitAction ();
    }
}


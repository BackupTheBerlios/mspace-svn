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

using Gtk.Ext;
using System;
using Gtk;

public class PlayAction : AbstractUIAction
{
    private bool playing = false;

    public PlayAction ()
    {
	StockIcon =  "muine-play";
	Label = AppContext.Catalog.GetString ("Pl_ay");
	AppContext.Playlist.SongChanged += HandleSongChanged;
	UpdateButtons ();
    }
    
    public override void ActionPerformed ()
    {
	if (!Enabled)
	    return;
	if (!playing)
	{
	    Pixbuf = StockIcons.GetPixbuf ("muine-pause");
	    Label = AppContext.Catalog.GetString ("P_ause");
	}
	else
	{
	    Pixbuf = StockIcons.GetPixbuf ("muine-play");
	    Label = AppContext.Catalog.GetString ("Pl_ay");
	}
	playing = !playing;
    }
    
    private void HandleSongChanged (object obj, Song s)
    {
	UpdateButtons ();
    }

    private void UpdateButtons ()
    {
	Enabled = AppContext.Playlist.Next != null;
    }
}


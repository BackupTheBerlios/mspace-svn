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


public class PlaylistView : DataView
{
    public PlaylistView (DataStore model) : base (model)
    {
	Reorderable = true;
	Selection.Mode = SelectionMode.Multiple;
    }

    protected override TreeViewColumn [] GetColums ()
    {
        TreeViewColumn[] columns = new TreeViewColumn[2];
        CellRendererText cr = new CellRendererText ();
	CellRendererPixbuf pixRen = new CellRendererPixbuf ();
	columns[0] = new TreeViewColumn ("Cover", pixRen, "pixbuf", 0);
	columns[0].SetCellDataFunc (pixRen, new TreeCellDataFunc (PixCellDataFunc));
        columns[1] = new TreeViewColumn ("Song", cr, "text", 1);
        columns[1].SetCellDataFunc (cr, new TreeCellDataFunc (CellDataFunc));
        return columns;
    }

    private void CellDataFunc (TreeViewColumn col, CellRenderer renderer,
                                TreeModel model, TreeIter iter)
    {
            CellRendererText r = (CellRendererText) renderer;
            string sinfo = (string)model.GetValue (iter, 1);
            sinfo.Trim ();
            sinfo = sinfo.Replace ("&", "&amp;");
            r.Markup = sinfo;
    }

    private void PixCellDataFunc (TreeViewColumn col, CellRenderer renderer,
                                TreeModel model, TreeIter iter)
    {
    }


    public override string Title {
	get {
	    return AppContext.Catalog.GetString (AppContext.Playlist.Current.Title +
						" - Muine Music Player");
	}
    }
    
    public enum PixbufType {
			    Playing,
			    Paused,
			    None
    }

    public PixbufType PlayingPixbuf {
	set {
	    switch (value)
	    {
		case PixbufType.Playing:
		    ChangePlayingPixbuf (new Gdk.Pixbuf (null, "muine-volume-medium.png"));
		    break;
		case PixbufType.Paused:
		    ChangePlayingPixbuf (new Gdk.Pixbuf (null, "muine-volume-zero.png"));
		    break;
		case PixbufType.None:
		    ChangePlayingPixbuf (new Gdk.Pixbuf (null, "muine-nothing.png"));
		    break;
		default:
		    break;
	    }
	}
    }

    private void ChangePlayingPixbuf (Gdk.Pixbuf pix)
    {
	    if (AppContext.Playlist.Current != null)
	    {
		    int current = AppContext.Playlist.IndexOf (AppContext.Playlist.Current);
		    TreeIter iter;
		    store.GetIterFromString (out iter, current.ToString ());
		    if (store.IterIsValid (iter))
			store.SetValue (iter, 0, pix);
	    }
    }

    protected override void OnRowActivated (TreePath path, TreeViewColumn column)
    {
	base.OnRowActivated (path, column);
	PlayingPixbuf = PixbufType.None;
	int row = Int32.Parse (path.ToString().Split (':')[0]); 
	AppContext.Playlist.Current = (Song)AppContext.Playlist[row];
	PlayingPixbuf = PixbufType.Playing;
	AppContext.PlayerBackend.PlayCurrent ();
    }
}


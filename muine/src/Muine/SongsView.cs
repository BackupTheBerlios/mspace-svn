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


public class SongsView : DataView
{
    public SongsView (DataStore model) : base (model)
    {
	Reorderable = true;
	Selection.Mode = SelectionMode.Multiple;
    }

    protected override TreeViewColumn [] GetColums ()
    {
        TreeViewColumn[] columns = new TreeViewColumn[1];
        CellRendererText cr = new CellRendererText ();
        columns[0] = new TreeViewColumn ("Song", cr, "text", 0);
        columns[0].SetCellDataFunc (cr, new TreeCellDataFunc (CellDataFunc));
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

    public override string Title {
	get {
	    return AppContext.Catalog.GetString (AppContext.Playlist.Current.Title +
						" - Muine Music Player");
	}
    }
    
    protected override void OnRowActivated (TreePath path, TreeViewColumn column)
    {
	/*base.OnRowActivated (path, column);
	PlayingPixbuf = PixbufType.None;
	int row = Int32.Parse (path.ToString().Split (':')[0]); 
	AppContext.Playlist.Current = (Song)AppContext.Playlist[row];
	PlayingPixbuf = PixbufType.Playing;
	AppContext.PlayerBackend.PlayCurrent ();*/
    }
}


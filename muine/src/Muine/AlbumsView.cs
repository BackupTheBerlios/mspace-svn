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

public class AlbumsView : DataView
{
    public AlbumsView (DataStore model) : base (model)
    {
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
                renderer.Width = renderer.Height = CoverDatabase.AlbumCoverSize + 5 * 2; 
    }


    public override string Title {
	get {
	    return AppContext.Catalog.GetString ("Play Album");
	}
    }
}


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
namespace Gtk.Ext 
{
    using Gtk;
    using System;
    using Gdk;

    public abstract class AbstractUIAction : UIAction
    {
	private Pixbuf pixbuf;
	public Pixbuf Pixbuf {
	    get {
		return pixbuf;
	    }
	    set {
		this.pixbuf = value;
	    }
	}

	private bool enabled = true;
	public bool Enabled {
	    get {
		return enabled;
	    }
	    set {
		enabled = value;
	    }
	}

	private string label = "";
	public string Label {
	    get {
		return label;
	    }
	    set {
		label = value;
	    }
	}   

	private IconSize iconSize = IconSize.Button;
	public IconSize IconSize {
	    get {
		return iconSize;
	    }
	    set {
		iconSize = value;
	    }
	}

	private string stock = "About";
	public string StockIcon {
	    get {
		return stock;	
	    }
	    set {
		stock = value;
	    }

	}

	public abstract void ActionPerformed ();
    }
}


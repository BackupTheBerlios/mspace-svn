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

namespace Gtk.Ext {
    
    using Gtk;
    using System;
    
    public class ImageButton : ActionButton
    {
	private Image img;
	private HBox box = new HBox ();

	public ImageButton (UIAction action) : base (action)
	{
	    image = new Image ();
	    image.SetFromStock (action.StockIcon, IconSize.LargeToolbar);
	    InitComponent ();
	}

	private Image image;
	public Image Image
	{
	    get {
		return image;
	    }
	    set {
		image.Destroy ();
		image = value;
		box.PackEnd (image);
	    }
	}

	private Label label;
	public new Label Label {
	    get {
		return label;
	    }
	    set {
		label = value;
		Label.Visible = textEnabled;
	    }
	}

	private bool textEnabled = false;
	public bool TextEnabled {
	    get {
		return textEnabled;
	    }
	    set {
		textEnabled = value;
		Label.Visible = textEnabled;
	    }
	}

	private void InitComponent ()
	{
	    Label = new Label (action.Label);
	    box.PackEnd (Label);
	    box.PackStart (image);
	    //Child.Destroy ();
	    Child = box;
	    box.Show ();
	}
    }
    
}


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

namespace Chicken.Gnome.TrayIcon 
{
    using System;    
    using Gtk;
    using System.Threading;

    public class ShrinkingTrayIcon : AnimatedTrayIcon
    {
	private Image image;
	private Gdk.Pixbuf icon;

	private void InitComponent ()
	{
	    image = new Image (icon);
	    hbox.PackStart (image);
	}

	public ShrinkingTrayIcon (string name, Gdk.Pixbuf icon) : base (name)
	{
	    this.icon = icon;
	    InitComponent ();
	}

	protected override void Render ()
	{
	    while (true)
	    {
		int lower = 12;
		int upper = 24;
		for (int i = upper; i >= lower; i--)
		{
		    Thread.Sleep (Frecuency);
		    Gdk.Threads.Enter ();
		    image.FromPixbuf = icon.ScaleSimple (i, i, Gdk.InterpType.Bilinear);
		    image.WidthRequest = upper;
		    Gdk.Threads.Leave ();
		}
		for (int j = lower; j <= upper ; j++)
		{
		    Thread.Sleep (Frecuency);
		    Gdk.Threads.Enter ();
		    image.FromPixbuf = icon.ScaleSimple (j, j, Gdk.InterpType.Bilinear); 
		    image.WidthRequest = upper;
		    Gdk.Threads.Leave ();
		}
	    }
	}
    }

}


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
    using System.Collections;

    public class BlinkingTrayIcon : AnimatedTrayIcon
    {
	private Gdk.Pixbuf[] pixbufArray;
	private Image image;

	private void InitComponent ()
	{
	    image = new Image (pixbufArray[0]);
	    hbox.PackStart (image);
	}

	public BlinkingTrayIcon (string name, Gdk.Pixbuf[] pixbufArray) : base (name)
	{
	    this.pixbufArray = pixbufArray;
	    if (pixbufArray.Length < 2)
		throw new ArgumentException ("BlinkingTrayIcon: pixbufArray shouldContain at least 2 pixbufs");
	    InitComponent ();
	}

	protected override void Render ()
	{
	    while (true)
	    {
		foreach (Gdk.Pixbuf pix in pixbufArray)
		{
		    Gdk.Threads.Enter ();
		    image.FromPixbuf = pix; 
		    Gdk.Threads.Leave ();
		    Thread.Sleep (Frecuency);
		}
	    }
	}

    }

}


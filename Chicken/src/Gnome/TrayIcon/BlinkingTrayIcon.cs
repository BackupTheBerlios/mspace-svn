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

    public class BlinkingTrayIcon : TrayIcon
    {
	private EventBox eBox;
	private VBox vbox;
	private Gdk.Pixbuf pix1, pix2;
	private Image image;

	public BlinkingTrayIcon (string name, Gdk.Pixbuf pix1, Gdk.Pixbuf pix2) : base (name)
	{
	    
	    eBox = new EventBox ();
	    vbox = new VBox ();
	    eBox.Add (vbox);
	    this.pix1 = pix1;
	    this.pix2 = pix2;
	    image = new Image (this.pix1);
	    vbox.PackStart (image);
	    Add (eBox);
	    
	}

	private int frecuency = 500;
	public int Frecuency {
	    get {
		return frecuency;
	    }
	    set {
		frecuency = value;
	    }
	}
	private void StartBlinking ()
	{
	    while (true)
	    {
		    Gdk.Threads.Enter ();
		    image.FromPixbuf = pix2; 
		    Gdk.Threads.Leave ();
		    Thread.Sleep (frecuency);
		    
		    Gdk.Threads.Enter ();
		    image.FromPixbuf = pix1;
		    Gdk.Threads.Leave ();
		    Thread.Sleep (frecuency);
	    }
	}
	
	public virtual void Run ()
	{
	    ShowAll ();
	    Thread t = new Thread (new ThreadStart (StartBlinking));
	    t.Start ();
	}
    }

}


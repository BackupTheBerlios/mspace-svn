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
	private HBox hbox;
	private Gdk.Pixbuf pix1, pix2;
	private Image image;
	private Thread t;

	/*public BlinkingTrayIcon (string name, Gdk.Pixbuf pix1, Gdk.Pixbuf pix2) : base (name)
	{
	    this.pix1 = pix1;
	    this.pix2 = pix2;
	    InitComponent ();
	}*/

	private void InitComponent ()
	{
	    image = new Image (this.pix1);
	    eBox = new EventBox ();
	    hbox = new HBox ();
	    eBox.Add (hbox);
	    eBox.ButtonPressEvent += ButtonPress;
	    hbox.PackStart (image);
	    Add (eBox);
	}

	public BlinkingTrayIcon (string name, Gdk.Pixbuf trayicon) : base (name)
	{
	    this.pix1 = trayicon;
	    InitComponent ();
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

	private int timeout = 5000;
	public int TimeOut {
	    get {
		return timeout;
	    }
	    set {
		timeout = value;
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

	private void StartShrinking ()
	{
	    while (true)
	    {
		int lower = 12;
		int upper = 24;
		for (int i = upper; i >= lower; i--)
		{
		    Thread.Sleep (75);
		    Gdk.Threads.Enter ();
		    image.FromPixbuf = pix1.ScaleSimple (i, i, Gdk.InterpType.Bilinear);
		    image.WidthRequest = upper;
		    Gdk.Threads.Leave ();
		}
		for (int j = lower; j <= upper ; j++)
		{
		    Thread.Sleep (75);
		    Gdk.Threads.Enter ();
		    image.FromPixbuf = pix1.ScaleSimple (j, j, Gdk.InterpType.Bilinear); 
		    image.WidthRequest = upper;
		    Gdk.Threads.Leave ();
		}
	    }
	}

	public void Stop ()
	{
		t.Abort ();
	}

	private void ButtonPress (object obj, ButtonPressEventArgs args)
	{
	    if (ButtonPressEvent != null)
		ButtonPressEvent (obj, args);
	}

	public new event ButtonPressEventHandler ButtonPressEvent;
	
	public virtual void Run ()
	{
	    ShowAll ();
	    t = new Thread (new ThreadStart (StartShrinking));
	    t.Start ();
	}
    }

}


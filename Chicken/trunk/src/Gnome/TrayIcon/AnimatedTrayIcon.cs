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

    public abstract class AnimatedTrayIcon : TrayIcon
    {
	private EventBox eBox;
	protected HBox hbox;
	private Thread t;

	private void InitComponent ()
	{
	    eBox = new EventBox ();
	    hbox = new HBox ();
	    eBox.Add (hbox);
	    eBox.ButtonPressEvent += ButtonPress;
	    Add (eBox);
	}

	public AnimatedTrayIcon (string name) : base (name)
	{
	    InitComponent ();
	}

	private int frecuency = 75;
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
	    t = new Thread (new ThreadStart (Render));
	    t.Start ();
	}

	protected abstract void Render ();
    }

}


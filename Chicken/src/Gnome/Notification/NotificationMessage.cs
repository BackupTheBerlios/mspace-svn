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

namespace Chicken.Gnome.Notification
{
    using System;    
    using Gecko;
    using Gtk;
    using Chicken.Gnome.TrayIcon;

    public class NotificationMessage
    {
	private WebControl webcontrol;
	private BlinkingTrayIcon icon;
	private Window window;
	
	public NotificationMessage (string htmlcontent, BlinkingTrayIcon icon)
	{
	    this.icon = icon;
	    webcontrol = new WebControl ();
	    webcontrol.LoadUrl (htmlcontent);
	    window = new Window (WindowType.Popup);
	    window.Add (webcontrol);
	    window.DefaultWidth = messageWidth;
	    window.DefaultHeight = messageHeight;
	    PositionWindow ();
	    
	}

	private int messageWidth = 150;
	public int MessageWidth {
	    get {
		return messageWidth;
	    }
	    set {
		messageWidth = value;	
		window.Resize (messageWidth, messageHeight);
	    }
	}

	private int messageHeight = 50;
	public int MessageHeight {
	    get {
		return messageHeight;
	    }
	    set {
		messageHeight = value;
		window.Resize (messageWidth, messageHeight);
	    }
	}

	public void Notify ()
	{
	    icon.Run();
	    window.ShowAll ();
	}
    
	private void PositionWindow ()
	{
	    window.Realize ();
	    int ourWidth;
	    int ourHeight;
	    window.GetSize (out ourWidth, out ourHeight);

	    window.Stick ();
	    // not wrapped self.set_skip_taskbar_hint(gtk.TRUE)
	    // not wrapped self.set_skip_pager_hint(gtk.TRUE)
	    window.TypeHint = Gdk.WindowTypeHint.Dock;

	    // Get the dimensions/position of the widgetToAlignWith
	    icon.Realize();
	    int entryX, entryY;
	    icon.GdkWindow.GetOrigin (out entryX, out entryY);
	    int entryWidth, entryHeight, tmp1, tmp2, tmp3;
	    icon.GdkWindow.GetGeometry (out tmp1, out tmp2, out entryWidth, out entryHeight, out tmp3);

	    // Get the screen dimensions
	    int screenHeight = Gdk.Screen.Default.Height;
	    int screenWidth = Gdk.Screen.Default.Width;

	    int extra = 10;
	    int newX;
	    if ((entryX + ourWidth) < screenWidth)
		// Align to the left of the entry
		newX = entryX - extra;
	    else
		// Align to the right of the entry
		newX = (entryX + entryWidth + extra) - ourWidth;

	    int newY;
	    if (entryY + entryHeight + ourHeight < screenHeight)
		// Align to the bottom of the entry
		newY = entryY + entryHeight + extra;
	    else
		newY = entryY - ourHeight -extra;

	    // -"Coordinates locked in captain."
	    // -"Engage."
	    window.Move(newX, newY);
	}

    }
}


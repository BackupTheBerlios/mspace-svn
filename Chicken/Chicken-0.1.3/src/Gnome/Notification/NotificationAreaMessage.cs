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
    using System.Threading;
    using System.IO;
    using System.Reflection;


    public class NotificationAreaMessage
    {
	private AnimatedTrayIcon icon;
	private String source;
	private NotificationSource sourceType;
	private NotificationContent contentType;
	private NotificationBubble bubble;
	
	public NotificationAreaMessage (string source, NotificationSource sourceType, NotificationContent contentType)
	{
	    this.contentType = contentType;
	    this.sourceType = sourceType;
	    this.source = source;
	    if (source == null)
		throw new ArgumentNullException ("Notification source can't be null.");
	    InitComponent ();
	}

	private void InitComponent ()
	{
	    bubble = new NotificationBubble (source, sourceType, contentType);
	    icon = new ShrinkingTrayIcon ("Message", Gdk.Pixbuf.LoadFromResource ("tray-icon.png"));
	    icon.ButtonPressEvent += ButtonPressed;
	}

	public event TimerEndedHandler TimerEndedEvent;

	private int timeout = 5000;
	public int TimeOut {
	    get {
		return timeout;
	    }
	    set {
		timeout = value;
		bubble.TimeOut = value;
	    }
	}

	private int bubbleWidth = 200;
	public int BubbleWidth {
	    get {
		return bubbleWidth;
	    }
	    set {
		bubbleWidth = value;	
		bubble.BubbleWidth = value;
	    }
	}

	private int bubbleHeight = 50;
	public int BubbleHeight {
	    get {
		return bubbleHeight;
	    }
	    set {
		bubbleHeight = value;
		bubble.BubbleHeight = value;
	    }
	}

	public void Notify ()
	{
	    bubble.Render ();
	    PositionBubble ();
	    bubble.ShowAll ();
	    icon.Run ();
	    Timer timer;
	    timer = new Timer (new TimerCallback (TimerRunner), null, timeout, 0);
	}

	private void TimerRunner (object obj)
	{
	    Gdk.Threads.Enter ();
	    icon.Stop ();
	    Destroy ();
	    Gdk.Threads.Leave ();
	    if (TimerEndedEvent != null)
		TimerEndedEvent ();
	}
	
	private void PositionBubble ()
	{
	    bubble.Realize ();
	    int tmp1, tmp2, tmp3;

	    bubble.Stick ();
	    bubble.SkipTaskbarHint = true;
	    bubble.SkipPagerHint = true;
	    bubble.TypeHint = Gdk.WindowTypeHint.Dock;

	    // Get the dimensions/position of the widgetToAlignWith
	    icon.Realize ();
	    int iconX, iconY;
	    icon.GdkWindow.GetOrigin (out iconX, out iconY);
	    int iconWidth, iconHeight;
	    icon.GdkWindow.GetGeometry (out tmp1, out tmp2, out iconWidth, out iconHeight, out tmp3);

	    // Get the screen dimensions
	    int screenHeight = Gdk.Screen.Default.Height;
	    int screenWidth = Gdk.Screen.Default.Width;

	    int newX;
	    if ((iconX + bubble.BubbleWidth) < screenWidth)
		// Align to the left of the entry
		newX = iconX;
	    else
		// Align to the right of the entry
		newX = (iconX + iconWidth) - bubble.BubbleWidth;

	    int extra = 10;
	    int newY;
	    if (iconY - iconHeight < 0)
		newY = iconY + iconHeight + extra;
	    else
		newY = screenHeight - iconHeight - bubble.BubbleHeight - extra;

	    // -"Coordinates locked in captain."
	    // -"Engage."
	    bubble.Move (newX, newY);
	}

/*	private void PositionBubble ()
	{
	    bubble.Realize ();
	    int ourWidth;
	    int ourHeight;
	    int tmp1, tmp2, tmp3;
	    bubble.GdkWindow.GetGeometry (out tmp1, out tmp2, out ourWidth, out ourHeight, out tmp3);

	    bubble.Stick ();
	    bubble.SkipTaskbarHint = true;
	    bubble.SkipPagerHint = true;
	    bubble.TypeHint = Gdk.WindowTypeHint.Dock;

	    // Get the dimensions/position of the widgetToAlignWith
	    icon.Realize();
	    int entryX, entryY;
	    icon.GdkWindow.GetOrigin (out entryX, out entryY);
	    int entryWidth, entryHeight;
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
	    bubble.Move (newX, newY);
	}*/
	
	private void ButtonPressed (object obj, ButtonPressEventArgs args)
        {
	    icon.Stop ();
	    Destroy ();
	    if (TimerEndedEvent != null)
		TimerEndedEvent ();
	    
        }
	
	private void Destroy ()
	{
	    icon.Destroy ();
	    bubble.Destroy ();
	}
	

    }
}


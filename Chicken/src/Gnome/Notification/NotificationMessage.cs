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

    public enum NotificationType {
	Message,
	Html,
	Svg
    }

    public class NotificationMessage
    {
	private WebControl webcontrol;
	private BlinkingTrayIcon icon;
	private Window window;
	private string text;
	private string header;
	private Stream msgStream;
	
	/*public NotificationMessage (string source, NotificationType type)
	{
	    window = new Window (WindowType.Popup);
	    icon.ButtonPressEvent += ButtonPressed;
	    switch (type)
	    {
		case NotificationType.Message:
		    RenderMessage (source);
		    break;
		case NotificationType.Html:
		    RenderHtml (source);
		    break;
	    }
		    
	    window.DefaultWidth = bubbleWidth;
	    window.DefaultHeight = bubbleHeight;
	    PositionWindow ();
	}*/

	public NotificationMessage (Stream stream, NotificationType type, string header, string text)
	{
	    this.header = header;
	    this.text = text;
	    this.msgStream = stream;
	    InitComponent ();
	    switch (type)
	    {
		case NotificationType.Svg:
		    RenderSvg (stream);
		    break;
	    }
	}

	private void InitComponent ()
	{
	    window = new Window (WindowType.Popup);
	    icon = new BlinkingTrayIcon ("Message", Gdk.Pixbuf.LoadFromResource ("tray-icon.png"));
	    icon.ButtonPressEvent += ButtonPressed;
	    window.DefaultWidth = bubbleWidth;
	    window.DefaultHeight = bubbleHeight;
	    PositionWindow ();
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

	private int bubbleWidth = 150;
	public int BubbleWidth {
	    get {
		return bubbleWidth;
	    }
	    set {
		bubbleWidth = value;	
		window.Resize (bubbleWidth, bubbleHeight);
	    }
	}

	private int bubbleHeight = 50;
	public int BubbleHeight {
	    get {
		return bubbleHeight;
	    }
	    set {
		bubbleHeight = value;
		window.Resize (bubbleWidth, bubbleHeight);
	    }
	}

	public void Notify ()
	{
	    window.ShowAll ();
	    icon.Run ();
	    Timer timer;
	    timer = new Timer (new TimerCallback (StartTimer), null, timeout, 0);
	}

	private void StartTimer (object obj)
	{
	    icon.Stop ();
	    Application.Quit ();
	}

	private void PositionWindow ()
	{
	    window.Realize ();
	    int ourWidth;
	    int ourHeight;
	    int tmp1, tmp2, tmp3;
	    window.GdkWindow.GetGeometry (out tmp1, out tmp2, out ourWidth, out ourHeight, out tmp3);

	    window.Stick ();
	    window.SkipTaskbarHint = true;
	    window.SkipPagerHint = true;
	    window.TypeHint = Gdk.WindowTypeHint.Dock;

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
	    window.Move(newX, newY);
	}
	
	private void ButtonPressed (object obj, ButtonPressEventArgs args)
        {
	    icon.Stop ();
	    Application.Quit ();
        }

	private void RenderHtml (string source)
	{
	    webcontrol = new WebControl ();
	    webcontrol.LoadUrl (source);
	    window.Add (webcontrol);
	}

	private void RenderMessage (string source)
	{
	    HTML html = new HTML ();
	    HTMLStream stream = html.Begin ("text/html");
	    stream.Write (source);
	    html.End (stream, HTMLStreamStatus.Ok);
	    window.Add (html);
	    
	}
	
	private void RenderSvg (Stream stream)
	{
	    StreamReader reader = new StreamReader (stream);
	    string svg = reader.ReadToEnd ();
	    reader.Close ();
	    stream.Close ();
	    svg = svg.Replace ("@HEADER@", header);
	    svg = svg.Replace ("@TEXT@", text);
	    string tmpdir = "/tmp/.chicken_notification_" + Environment.TickCount;
	    string tmpfile = tmpdir + "/tmpsvg.svg";
	    Directory.CreateDirectory (tmpdir);
	    StreamWriter writer = new StreamWriter (new FileStream (tmpfile, FileMode.OpenOrCreate, FileAccess.Write));
	    writer.Write (svg);
	    writer.Close ();
	    Image img = new Image (Rsvg.Pixbuf.FromFile(tmpfile));
	    window.Add (img);
		
	}



    }
}


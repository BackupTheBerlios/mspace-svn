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

    public enum NotificationSource {
	Text,
	File,
	Url
    }

    public enum NotificationContent {
	Svg,
	Html,
	PlainText
    }

    public enum NotificationType {
	Info,
	Warning,
	Error
    }

    public class NotificationMessage
    {
	private WebControl webcontrol;
	private ShrinkingTrayIcon icon;
	private Window window;
	private String source;
	private NotificationSource sourceType;
	private NotificationContent contentType;
	
	public NotificationMessage (string source, NotificationSource sourceType, NotificationContent contentType)
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
	    window = new Window (WindowType.Popup);
	    icon = new ShrinkingTrayIcon ("Message", Gdk.Pixbuf.LoadFromResource ("tray-icon.png"));
	    icon.ButtonPressEvent += ButtonPressed;
	    window.DefaultWidth = bubbleWidth;
	    window.DefaultHeight = bubbleHeight;
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

	private int bubbleWidth = 200;
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
	    switch (contentType)
	    {
		case NotificationContent.Svg:
		    RenderSvg ();
		    break;
		case NotificationContent.Html:
		    RenderHtml ();
		    break;
		case NotificationContent.PlainText:
		    RenderPlainText ();
		    break;
	    }
	    PositionWindow ();
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

	private void RenderHtml ()
	{
	    switch (sourceType)
	    {
		case NotificationSource.File:
		    RenderHtmlFromFile ();
		    break;
		case NotificationSource.Text:
		    RenderHtmlFromText ();
		    break;
		case NotificationSource.Url:
		    RenderHtmlFromUrl ();
		    break;
	    }
	}

	private void RenderHtmlFromFile ()
	{
	    webcontrol = new WebControl ();
	    webcontrol.LoadUrl ("file://" + source);
	    window.Add (webcontrol);
	}

	private void RenderHtmlFromUrl ()
	{
	    webcontrol = new WebControl ();
	    webcontrol.LoadUrl (source);
	    window.Add (webcontrol);
	}

	private void RenderHtmlFromText ()
	{
	    string tmpdir = "/tmp/chicken_notification";
	    if (!Directory.Exists (tmpdir))
		Directory.CreateDirectory (tmpdir);
	    string tmpfile = tmpdir + String.Format ("/tmphtml-{0}.html", Environment.TickCount);
	    StreamWriter writer = new StreamWriter (new FileStream (tmpfile, FileMode.Create, FileAccess.Write));
	    writer.Write (source);
	    writer.Close ();
	    webcontrol = new WebControl ();
	    webcontrol.LoadUrl ("file://" + tmpfile);
	    window.Add (webcontrol);
	    //File.Delete (tmpfile);
	}

	private void RenderPlainText ()
	{
	}
	
	private void RenderSvg ()
	{
	    switch (sourceType)
	    {
		case NotificationSource.File:
		    RenderSvgFromFile ();
		    break;
		case NotificationSource.Text:
		    RenderSvgFromText ();
		    break;
		default:
		    Console.WriteLine ("Svg rendering from source " + sourceType + " not posible. Ignored.");
		    break;
	    }
		
	}

	private void RenderSvgFromText ()
	{
	    string tmpdir = "/tmp/chicken_notification";
	    if (!Directory.Exists (tmpdir))
		Directory.CreateDirectory (tmpdir);
	    string tmpfile = tmpdir + String.Format ("/tmpsvg-{0}.svg", Environment.TickCount);
	    StreamWriter writer = new StreamWriter (new FileStream (tmpfile, FileMode.Create, FileAccess.Write));
	    writer.Write (source);
	    writer.Close ();
	    Image img = new Image (Rsvg.Pixbuf.FromFileAtSize (tmpfile, BubbleWidth, BubbleHeight));
	    File.Delete (tmpfile);
	    window.Add (img);
	}

	private void RenderSvgFromFile ()
	{
	    if (!File.Exists (source))
	    {
		Console.WriteLine ("Svg file does not exists. Ignoring notification.");
		return;
	    }
	    Image img = new Image (Rsvg.Pixbuf.FromFileAtSize (source, BubbleWidth, BubbleHeight));
	    window.Add (img);
	}



    }
}


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
    using System.Threading;
    using System.IO;


    public class NotificationBubble : Window
    {
	private WebControl webcontrol;
	private String source;
	private NotificationSource sourceType;
	private NotificationContent contentType;
	
	public NotificationBubble (string source, NotificationSource sourceType, 
				    NotificationContent contentType)
	    : base (WindowType.Popup)
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
	    DefaultWidth = bubbleWidth;
	    DefaultHeight = bubbleHeight;
	}

	public event TimerEndedHandler TimerEndedEvent;

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
		Resize (bubbleWidth, bubbleHeight);
	    }
	}

	private int bubbleHeight = 50;
	public int BubbleHeight {
	    get {
		return bubbleHeight;
	    }
	    set {
		bubbleHeight = value;
		Resize (bubbleWidth, bubbleHeight);
	    }
	}

	public void RenderWithTimer()
	{
	    Render ();
	    Timer timer;
	    timer = new Timer (new TimerCallback (TimerRunner), null, timeout, 0);
	}

	public void Render ()
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
	}

	private void TimerRunner (object obj)
	{
	    Gdk.Threads.Enter ();
	    Destroy ();
	    Dispose ();
	    Gdk.Threads.Leave ();
	    if (TimerEndedEvent != null)
		TimerEndedEvent ();
	}

	private void RenderHtml ()
	{
	    webcontrol = new WebControl ();
	    Add (webcontrol);
	    webcontrol.Show ();
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
	    webcontrol.LoadUrl ("file://" + source);
	}

	private void RenderHtmlFromUrl ()
	{
	    webcontrol.LoadUrl (source);
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
	    webcontrol.LoadUrl ("file://" + tmpfile);
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
	    Add (img);
	}

	private void RenderSvgFromFile ()
	{
	    if (!File.Exists (source))
	    {
		Console.WriteLine ("Svg file does not exists. Ignoring notification.");
		return;
	    }
	    Image img = new Image (Rsvg.Pixbuf.FromFileAtSize (source, BubbleWidth, BubbleHeight));
	    Add (img);
	}

    }
}


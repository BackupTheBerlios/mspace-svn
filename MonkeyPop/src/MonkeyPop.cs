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

using System;    
using Chicken.Gnome.TrayIcon;
using Chicken.Gnome.Notification;
using Gtk;

public class MonkeyPop
{
    static string message;
    static string html;
    static string width = "200";
    static string height = "80";
    static string timeout = "5000";
    static string svg;


    public static void Main (string[] args)
    {

	for (int i = 0; i < args.Length ; i++)
	{
	    switch (args[i])
	    {
		case "--html":
		    html = args[i+1];
		    break;
		case "--message":
		    message = args[i+1];
		    break;
		case "--width":
		    width = args[i+1];
		    break;
		case "--height":
		    height = args[i+1];
		    break;
		case "--timeout":
		    timeout = args[i+1];
		    break;
		case "--svg":
		    svg = args[i+1];
		    break;
	    }
	}

	if ((html == null) && (message == null) && (svg == null))
	{
	    Console.WriteLine (
		    "Usage: traymsg --html file:///path_to_html_file | --message \"string\" [options]\n"	    +
		    "	--html path_to_file	html file to load in the notification message\n"		    +
		    "	--svg path to svg file	renders a SVG as a message\n"					    +
		    "	--message \"string\"	string to display in the notification message\n"		    +
		    "	--timeout miliseconds	time to wait before message disapears\n"			    +
		    "	--width	pixels		width of the notification message\n"				    +
		    "	--height pixels		height of the notification message\n"				    
		    );
	    Environment.Exit (1);

	}
		
	Application.Init ();
	Gdk.Pixbuf pix1 = new Gdk.Pixbuf (null, "notification.png").ScaleSimple (24, 24, Gdk.InterpType.Nearest);
	Gdk.Pixbuf pix2 = new Gdk.Pixbuf (null, "notification-white.png").ScaleSimple (24,24, Gdk.InterpType.Nearest);
	BlinkingTrayIcon icon = new BlinkingTrayIcon ("test", pix1, pix2);
	NotificationMessage msg;
	if (html != null)
	    msg = new NotificationMessage (html, icon, NotificationType.Html);
	else if (message != null)
	    msg = new NotificationMessage (message, icon, NotificationType.Message);
	else
	    msg = new NotificationMessage (svg, icon, NotificationType.Svg);
	    
	msg.BubbleWidth = Int32.Parse (width);
	msg.BubbleHeight = Int32.Parse (height);
	msg.TimeOut = Int32.Parse (timeout);
	msg.Notify ();
	Application.Run ();
    }
}


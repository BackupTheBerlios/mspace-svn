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
using System.Text;

public class SimpleMonkeyPop 
{
    static StringBuilder text = new StringBuilder ();
    static StringBuilder header = new StringBuilder ();
    static string width = "200";
    static string height = "75";
    static bool insideText = false;

    static string timeout = "5000";
    static string file;
    static NotificationType option;
    static StringBuilder current = new StringBuilder ();


    public static void Main (string[] args)
    {

	for (int i = 0; i < args.Length ; i++)
	{
	    switch (args[i])
	    {
		case "--text":
		    current = text;
		    insideText = true;
		    break;
		case "--header":
		    current = header;
		    insideText = true;
		    break;
		case "--timeout":
		    current = null;
		    timeout = args[i+1];
		    insideText = false;
		    break;
		case "--file":
		    current = null;
		    file = args[i+1];
		    insideText = false;
		    break;
		case "--info":
		    current = null;
		    option = NotificationType.Info;
		    insideText = false;
		    break;
		case "--warning":
		    current = null;
		    option = NotificationType.Warning;
		    insideText = false;
		    break;
		case "--error":
		    current = null;
		    option = NotificationType.Error;
		    insideText = false;
		    break;
		case "--width":
		    current = null;
		    insideText = false;
		    width = args[i+1];
		    break;
		case "--height":
		    current = null;
		    insideText = false;
		    height = args[i+1];
		    break;
		default:
		    if (args[i].StartsWith ("--") && !insideText)
		    {
			ShowHelp ();
		    } else if (current != null)
			current.Append (args[i] + " ");
		    break;
	    }
	}

	if ((text.ToString () == String.Empty) && (header.ToString () == String.Empty))
	{
	    ShowHelp ();
	    Environment.Exit (1);

	}
		
	Application.Init ();
	if (file != null)
	    NotificationFactory.ShowSvgNotification (file, header.ToString (), text.ToString (), Int32.Parse (timeout));
	else
	    NotificationFactory.ShowMessageNotification (header.ToString (), text.ToString (), Int32.Parse (timeout), Int32.Parse (width), Int32.Parse (height), option);
	Application.Run ();
    }

    private static void ShowHelp ()
    {
	    Console.WriteLine (
		    "Usage: monkeypop --text \"string\" --header \"string\" [--timeout miliseconds] [options]\n" +
		    "Where options are:\n" +
		    "--file svgfile	Svg file to render as message\n"	+
		    "--warning		Show a warning message\n"		+
		    "--info		Show an info message\n" +
		    "--error		Show an error message\n"
		    );
	    Environment.Exit (1);
    }

}


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
    static string timeout = "5000";
    static string file;
    static NotificationType option;
    static StringBuilder current = new StringBuilder ();


    public static void Main (string[] args)
    {

	Console.WriteLine (args.Length);
	for (int i = 0; i < args.Length ; i++)
	{
	    switch (args[i])
	    {
		case "--text":
		    current = text;
		    break;
		case "--header":
		    current = header;
		    break;
		case "--timeout":
		    current = null;
		    timeout = args[i+1];
		    break;
		case "--file":
		    current = null;
		    file = args[i+1];
		    break;
		case "--info":
		    current = null;
		    option = NotificationType.Info;
		    break;
		case "--warning":
		    current = null;
		    option = NotificationType.Warning;
		    break;
		case "--error":
		    current = null;
		    option = NotificationType.Error;
		    break;
		default:
		    if (current != null)
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
	    NotificationFactory.ShowMessageNotification (header.ToString (), text.ToString (), Int32.Parse (timeout), option);
	Application.Run ();
    }

    private static void ShowHelp ()
    {
	    Console.WriteLine (
		    "Usage: monkeypop --text \"string\" --header \"string\" [--timeout miliseconds] [options]\n" +
		    "Where options are:\n" +
		    "--file svgfile	Svg file to render as message"	+
		    "--warning		Show a warning message"		+
		    "--info		Show an info message" +
		    "--error		Show an error message"
		    );
	    Environment.Exit (1);
    }

}


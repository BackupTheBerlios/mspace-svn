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
    static NotificationType option;
    static StringBuilder current = new StringBuilder ();


    public static void Main (string[] args)
    {

	    switch (args[0])
	    {
		case "--html":
		    DoHtml (args);
		    break;
		case "--svg":
		    DoSvg (args);
		    break;
		default:
		    ShowHelp ();
		    break;
	    }
    }

    private static void DoHtml (string[] args)
    {
	/* check if we have at least --html and (--content | --file)*/
	if (args.Length < 3)
	{
	    ShowHelp ();
	    return;
	}

	//args[0] = --html
	//args[1] = --content | --file
	//args[2] = html chunk | filename
	StringBuilder content = new StringBuilder ();
	string file;
	switch (args[1])
	{
	    case "--content":
		for (int i = 2; i < args.Length ; i++)
		    content.Append (args[i]);
		break;
	    case "--file":
		file = args[2];
		break;
	    default:
		ShowHelp ();
		break;

	}
	
    }

    private static void DoSvg (string[] args)
    {
	//args[0] = --svg
	//args[1] = --content | --file | (--warning|--error|--info)
	//args[2] = svg chunk | filename | header chunk
	if (args.Length < 3)
	{
	    ShowHelp ();
	    return;
	}
	StringBuilder chunk = new StringBuilder ();
	StringBuilder text = new StringBuilder ();
	StringBuilder header = new StringBuilder ();
	string file = null;
	switch (args[1])
	{
	    case "--content":
		for (int i = 2; i < args.Length ; i++)
		    chunk.Append (args[i]);
		break;

	    case "--file":
		file = args[2];
		break;
		
	    case "--warning":
	    case "--info":
	    case "--error":
		if (args[2] != "--header")
		    ShowHelp ();
		int textpos = -1; 
		for (int i = 3; i < args.Length ; i++)
		{
		    if (args[i] == "--text")
			textpos = i;
		}
		if (textpos != -1)
		{
		    for (int i = textpos + 1; i < args.Length; i++)
			text.Append (args[i] + " ");
		} else
		    textpos = args.Length;

		for (int i = 3; i < textpos; i++)
		    header.Append (args[i] + " ");
		break;
		
	    default:
		ShowHelp ();
		break;
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


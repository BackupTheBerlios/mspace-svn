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
using Gtk;
using System.Reflection;
using Mono.GetOptions;
using Chicken.Gnome.Notification;
using System.Text;

[assembly: AssemblyTitle ("monkeypop")]
[assembly: AssemblyVersion ("0.1")]
[assembly: AssemblyDescription ("Chicken library client example")]
[assembly: AssemblyCopyright ("Sergio Rubio <sergio.rubio@hispalinux.es")]

// This is text that goes after " [options]" in help output.
[assembly: Mono.UsageComplement ("")]

// Attributes visible in " -V"
[assembly: Mono.About("Show desktop notifications from scripts")]
[assembly: Mono.Author ("Sergio Rubio <sergio.rubio@hispalinux.es>")]

internal class SimpleOptions : Options 
{
 
    [Option ("Render Html", "html")]
    public bool html;

    [Option ("Render Svg", "svg")]
    public bool svg;

    [Option ("Use a warning message", "warning")]
    public bool warning;
    [Option ("Use a info message", "info")]
    public bool info;
    [Option ("Use an error message", "error")]
    public bool error;
    
    [Option ("Notification header", "header")]
    public string header;
    [Option ("Notification text", "text")]
    public string text;

    [Option ("Content to render (svg or html)", 'c')]
    public string content = null;
    [Option ("Render html content from Url", 'u')]
    public string url = null;
    [Option ("Render svg or html from file", 'f')]
    public string file = null;

    [Option ("Bubble message width", 'w')]
    public int width = 200;
    [Option ("Bubble message height", 'h')]
    public int height = 75;
    [Option ("Notification timeout in miliseconds", 't')]
    public int timeout = 5000;

    public SimpleOptions ()
    {
	base.ParsingMode = OptionsParsingMode.Both;
    }

}
public class MonkeyPop
{
    public static void Main (string[] args)
    {
	Application.Init ();
	SimpleOptions options = new SimpleOptions ();
	options.ProcessArgs (args);
	bool collecting = false;
	StringBuilder textbuilder = new StringBuilder ();
	StringBuilder headerbuilder = new StringBuilder ();
	foreach (string s in args)
	{
	    if (s == "--text")
	    {
		collecting = true;
		continue;
	    }
	    else if (s.StartsWith ("--") && collecting)
	    {
		collecting = false;
	    }
	    if (collecting)
		textbuilder.Append (s + " ");
	}
	options.text = textbuilder.ToString ();
	foreach (string s in args)
	{
	    
	    if (s == "--header")
	    {
		collecting = true;
		continue;
	    }
	    else if (s.StartsWith ("--") && collecting)
	    {
		collecting = false;
	    }
	    if (collecting)
		headerbuilder.Append (s + " ");
	}
	options.header = headerbuilder.ToString ();
		
	
	if (args.Length == 0)
	{
	    options.DoHelp ();
	    Environment.Exit (1);
	}
	
	if (options.html)
	{
	    if (options.content != null)
	    {
		NotificationFactory.ShowHtmlNotification 
		    (options.content, NotificationSource.Text, options.width, options.height, options.timeout);
		Application.Run ();
	    }
	    else if (options.file != null)
	    {
		NotificationFactory.ShowHtmlNotification 
		    (options.file, NotificationSource.File, options.width, options.height, options.timeout);
		Application.Run ();
	    }
	    else if (options.url != null)
	    {
		NotificationFactory.ShowHtmlNotification 
		    (options.url, NotificationSource.Url, options.width, options.height, options.timeout);
		Application.Run ();
	    }
	    else
		options.DoHelp ();
	}
	else if (options.svg)
	{
	    if (options.file != null)
	    {
		NotificationFactory.ShowSvgNotification 
		    (options.file, options.header, options.text, options.width, options.height, options.timeout);
		Application.Run ();

	    } 
	    else if (options.content != null)
	    {
	    }
	    else if (options.warning)
	    {
		NotificationFactory.ShowMessageNotification (options.header, options.text, options.timeout, options.width, options.height, NotificationType.Warning);
		Application.Run ();
	    }
	    else if (options.info)
	    {
		NotificationFactory.ShowMessageNotification (options.header, options.text, options.timeout, options.width, options.height, NotificationType.Info);
		Application.Run ();
	    } 
	    else if (options.error)
	    {
		NotificationFactory.ShowMessageNotification (options.header, options.text, options.timeout, options.width, options.height, NotificationType.Error);
		Application.Run ();
	    }
	    else
		options.DoHelp ();
	}
	else
	    options.DoHelp ();
    }
    
}


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
		collecting = false;

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
		collecting = false;

	    if (collecting)
		headerbuilder.Append (s + " ");
	}
	options.header = headerbuilder.ToString ();
		
	
	if (args.Length == 0)
	{
	    options.DoHelp ();
	    Environment.Exit (1);
	}
	
	//We are receiving html
	if (options.html)
	{
	    NotificationSource sourceType = NotificationSource.Url;
	    string source = null;
	    if (options.content != null)
	    {
		    sourceType = NotificationSource.Text;
		    source = options.content;
	    }
	    else if (options.file != null)
	    {
		    sourceType = NotificationSource.File;
		    source = options.file;
	    }
	    else if (options.url != null)
	    {
		    sourceType = NotificationSource.Url;
		    source = options.url;
	    }
	    else
		options.DoHelp ();

	    NotificationFactory.ShowHtmlNotification (source, sourceType,
							options.width, options.height,
							options.timeout, new TimerEndedHandler (TimerEnded));
	}
	
	// We are receiving svg
	else if (options.svg)
	{
	    if (options.file != null)
		NotificationFactory.ShowSvgNotification (options.file, options.header,
							options.text, options.width,
							options.height, options.timeout,
							new TimerEndedHandler (TimerEnded));
	    
	    else if (options.warning)
		ShowStandardMsg (NotificationType.Warning, options);
		    
	    else if (options.info)
		ShowStandardMsg (NotificationType.Info, options);
		    
	    else if (options.error)
		ShowStandardMsg (NotificationType.Error, options);

	    else
		options.DoHelp ();
	}
	else
	    options.DoHelp ();
	Application.Run ();
    }

    private static void TimerEnded ()
    {
	Application.Quit ();
    }

    private static void ShowStandardMsg (NotificationType type, SimpleOptions options)
    {
		NotificationFactory.ShowMessageNotification (options.header, options.text,
								options.timeout, options.width,
								options.height, type, new TimerEndedHandler (TimerEnded));
    }
    
}


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
    using System.IO;
    using System.Reflection;
    using System.Text;

    public class NotificationFactory
    {

	public static void ShowSvgNotification (string source,
						NotificationSource nsource,
						NotificationContent ncontent,
						int timeout)
	{
	    NotificationMessage msg = new NotificationMessage (source, nsource, ncontent);	    
	    msg.TimeOut = timeout;
	    msg.Notify ();
	}

	public static void ShowSvgNotification (string source, string header, string body, int timeout)
	{
	    ShowSvgNotification (source, NotificationSource.File, header, body, timeout, 200, 75, NotificationType.Info);
	}

	public static void ShowSvgNotification (string source,
						NotificationSource nsource,
						string header,
						string body,
						int timeout,
						int width,
						int height,
						NotificationType type)
	{
	    Stream stream;
	    string svg = "";
	    if (source == null)
	    {
		NotificationFactory factory = new NotificationFactory ();
		Assembly assembly = Assembly.GetAssembly (factory.GetType ());
		stream = assembly.GetManifestResourceStream (type.ToString () + ".svg");
		StreamReader reader = new StreamReader (stream);
		svg = reader.ReadToEnd ();
		reader.Close ();
		stream.Close ();
	    } else {
		if (nsource == NotificationSource.File) {
		    stream = new FileStream (source, FileMode.Open, FileAccess.Read);
		    StreamReader reader = new StreamReader (stream);
		    svg = reader.ReadToEnd ();
		    reader.Close ();
		    stream.Close ();
		} else if (nsource == NotificationSource.Text)
		    svg = source;
	    }
	     
	    svg = ReplaceMacros (svg, header, body);
	    NotificationMessage msg = new NotificationMessage (svg, NotificationSource.Text, NotificationContent.Svg);
	    msg.TimeOut = timeout;
	    msg.BubbleWidth = width;
	    msg.BubbleHeight = height;
	    msg.Notify ();
	    
	}

	public static void ShowMessageNotification (string header, 
						    string body,
						    int timeout,
						    int width,
						    int height,
						    NotificationType type)
	{
	    ShowSvgNotification (null, NotificationSource.Text, header, body, timeout, width, height, type);
	}

	public static void ShowMessageNotification (string header, string body, int timeout, NotificationType type)
	{
	    NotificationFactory.ShowMessageNotification (header, body, timeout, 200, 75, type); 
	}

	private static string ReplaceMacros (string svg, string header, string text)
	{
	    
	    int linenum = 0;
	    string newsvg = svg;
	    newsvg = newsvg.Replace ("@HEADER@", header);
	    string[] lines = new string[3];
	    int pcount = 0;
	    int tokens = 0;
	    int lasti = 0;
	    
	    StringBuilder builder = new StringBuilder ();
	    for (int i = 0 ; i < text.Length; i++)
	    {
		if (tokens == 2)
		{
		    lasti = i;
		    break;
		}

		if (text[i] != '%' || pcount > 2)
		{
		    builder.Append (text[i]);
		    pcount = 0;
		}
		else {
			pcount++;
			if (pcount == 2 && linenum < 2)
			{
			    tokens++;
			    lines[linenum] = builder.ToString (); 
			    builder = new StringBuilder ();
			    linenum++;
			    pcount = 0;
			}
		}
		
	    }
	    lines[linenum] = text.Substring (lasti); 
	    
	    newsvg = newsvg.Replace ("@LINE1@", lines[0]);
	    newsvg = newsvg.Replace ("@LINE2@", lines[1]);
	    newsvg = newsvg.Replace ("@LINE3@", lines[2]);
	    
	    return newsvg;
	}
    }

}


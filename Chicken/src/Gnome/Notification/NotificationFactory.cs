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
    using System.Collections;

    public class NotificationFactory
    {

	public static void ShowHtmlNotification (string source, NotificationSource nsource, int width, int height, int timeout)
	{
	    NotificationMessage msg = new NotificationMessage (source, nsource, NotificationContent.Html);
	    msg.BubbleWidth = width;
	    msg.BubbleHeight = height;
	    msg.TimeOut = timeout;
	    msg.Notify ();
		    
	}

	public static void ShowSvgNotification (string source, string header, string body, int width, int height, int timeout)
	{
	    ShowSvgNotification (source, NotificationSource.File, header, body, timeout, width, height, NotificationType.Info);
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

	public static void ShowMessageNotification (string header, string body, NotificationType type)
	{
	    NotificationFactory.ShowMessageNotification (header, body, 5000, 200, 75, type); 
	}

	private static string ReplaceMacros (string svg, string header, string text)
	{
	    
	    string newsvg = svg;
	    newsvg = newsvg.Replace ("@HEADER@", header);
	    int pcount = 0;
	    ArrayList tokens = new ArrayList ();
	    
	    StringBuilder builder = new StringBuilder ();
	    for (int i = 0 ; i < text.Length; i++)
	    {
		if (text[i] != '%' || pcount > 2)
		{
		    builder.Append (text[i]);
		    pcount = 0;
		}
		else {
			pcount++;
			if (pcount == 2 && tokens.Count < 3)
			{
			    Token t = new Token ();
			    t.nextTokenAt = i + 1;
			    t.number = tokens.Count + 1;
			    t.content = builder.ToString ();
			    tokens.Add (t);
			    builder = new StringBuilder ();
			}
		}
		
	    }
	    Token last = new Token ();
	    if (tokens.Count == 0)
		last.content = text;
	    else
		last.content = text.Substring (((Token)tokens[tokens.Count -1]).nextTokenAt);
	    tokens.Add (last);

	    while (tokens.Count < 3)
	    {
		Token empty = new Token ();
		empty.content = "";
		tokens.Add (empty);
	    }
	    
	    newsvg = newsvg.Replace ("@LINE1@", ((Token)(tokens[0])).content);
	    newsvg = newsvg.Replace ("@LINE2@", ((Token)(tokens[1])).content);
	    newsvg = newsvg.Replace ("@LINE3@", ((Token)(tokens[2])).content);
	    
	    return newsvg;
	}
    }

    internal class Token {
	public int nextTokenAt;
	public int number;
	public string content;
    }

}


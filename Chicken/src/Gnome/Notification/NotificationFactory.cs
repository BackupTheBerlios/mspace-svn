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
    using Chicken.Gnome.TrayIcon;
    using System.IO;
    using System.Reflection;

    public class NotificationFactory
    {

	public static void ShowSvgNotification (string source, NotificationSource nsource, NotificationContent ncontent, int timeout)
	{
	    NotificationMessage msg = new NotificationMessage (source, nsource, ncontent);	    
	    msg.TimeOut = timeout;
	    msg.Notify ();
	}

	public static void ShowSvgNotification (string source, string header, string body, int timeout)
	{
	    ShowSvgNotification (source, NotificationSource.File, header, body, timeout, 200, 50, NotificationType.Info);
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
	     
	    svg = svg.Replace ("@HEADER@", header);
	    svg = svg.Replace ("@TEXT@", body);
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
	    NotificationFactory.ShowMessageNotification (header, body, timeout, 200, 50, type); 
	}
    }

}


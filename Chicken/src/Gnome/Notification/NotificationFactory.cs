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
	/*public static void ShowMessageNotification (string header, string text, int timeout)
	{
	    BlinkingTrayIcon b = new BlinkingTrayIcon ("Message", new Gdk.Pixbuf (null, "tray-icon.png"));
	    NotificationMessage nm = new NotificationMessage ("simple.svg", b, NotificationType.Svg, header, text);
	    nm.TimeOut = timeout;
	    nm.Notify();
	}*/

	public static void ShowMessageNotification (string svgfile, string header, string text, int timeout)
	{
	    NotificationFactory factory = new NotificationFactory ();
	    Stream stream;

	    if (svgfile == null)
	    {
		Assembly assembly = Assembly.GetAssembly (factory.GetType ());
		Console.WriteLine (assembly);
		stream = assembly.GetManifestResourceStream ("simple.svg");
		Console.WriteLine ("stream: ", stream);
	    } else if (!File.Exists (svgfile)) {
		Console.WriteLine ("Svg file not found.");
		return;
	    } else
		stream = new FileStream (svgfile, FileMode.Open);
		
	    NotificationMessage nm = new NotificationMessage (stream, NotificationType.Svg, header, text);
		nm.Notify ();
	}
    }

}


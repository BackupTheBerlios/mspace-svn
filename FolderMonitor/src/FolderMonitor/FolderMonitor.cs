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

namespace FolderMonitor 
{
    using System;    
    using System.IO;
    using Gtk;
    using Egg;
    using System.Diagnostics;
    using System.Threading;

    public class FolderMonitor
    {
	private TrayIcon icon;
	private EventBox eBox;
	private Image image;
	private string path;

	public FolderMonitor (string folder)
	{
	    this.path = folder;
	    icon = new TrayIcon ("FolderMonitor");
	    image = new Image (new Gdk.Pixbuf (null, "dropbox-changed.png"));
	    eBox = new EventBox ();
	    eBox.Add (image);
	    eBox.ButtonPressEvent += ButtonPressed;
	    icon.Add (eBox);
	    
	    FileSystemWatcher mon = new FileSystemWatcher (folder); 
	    mon.IncludeSubdirectories = true;
	    mon.EnableRaisingEvents = true;
	    mon.Changed += OnChanged;
	    mon.Created += OnChanged;
	    mon.Deleted += OnChanged;
	    mon.Renamed += OnRenamed;
	}

	public static void Main (string[] args)
	{
	    if (args.Length < 1)
	    {
		Console.WriteLine ("Use the path of a folder as an argument");
		Environment.Exit (1);
	    }
	    Gdk.Threads.Init ();
	    Application.Init ();
	    
	    FolderMonitor mon = new FolderMonitor (args[0]);

	    Gdk.Threads.Enter ();
	    Application.Run ();
	    Gdk.Threads.Leave ();
	}

	private void OnChanged (object obj, FileSystemEventArgs args)
	{
	    icon.ShowAll ();
	    Thread t = new Thread (new ThreadStart (ChangeIcon));
	    t.Start ();
	}

	private void OnRenamed (object obj, RenamedEventArgs args)
	{
	    Console.WriteLine ("Changed");
	}

	private void ButtonPressed (object obj, ButtonPressEventArgs args)
        {
		Process.Start ("nautilus", "file://" + path);
		icon.Hide ();
        }

	private void ChangeIcon ()
	{
	    while (true)
	    {
		//for (int i = 16 ; i > 1 ; i--)
		//{
		    Gdk.Threads.Enter ();
		    //image.Pixbuf.ScaleSimple (i, i, Gdk.InterpType.Bilinear);
		    image.FromPixbuf = new Gdk.Pixbuf (null, "dropbox-changed.png");
		    //image.QueueDraw ();
		    Gdk.Threads.Leave ();
		    Thread.Sleep (500);
		//}

		//for (int i = 1 ; i < 15 ; i++)
		//{
		    Gdk.Threads.Enter ();
		    //image.Pixbuf.ScaleSimple (i, i, Gdk.InterpType.Bilinear);
		    image.FromPixbuf = new Gdk.Pixbuf (null, "dropbox-changed-red.png");
		    //image.QueueDraw ();
		    Gdk.Threads.Leave ();
		    Thread.Sleep (500);
		//}
	    }
	}

    }

}


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

namespace WebNotes 
{
    using System;    
    using Gtk;
    using System.IO;
    using System.Diagnostics;
    using System.Reflection;

    public class WebNotes
    {

	private static Process process;

	public static void Main (string[] args)
	{
	    CopyDefaultFiles ();
	    
	    string path = Path.GetDirectoryName (Assembly.GetCallingAssembly ().Location);
	    ProcessStartInfo pinfo;
	    pinfo = new ProcessStartInfo (path + Path.DirectorySeparatorChar + "didiwiki");
	    pinfo.UseShellExecute = true;
	    process = Process.Start (pinfo);
	    
	    Application.Init ();
	    WebViewer wv = new WebViewer ();
	    wv.ShowAll ();
	    Application.Run ();
	}

	private static void CopyDefaultFiles ()
	{
	    string didiwikidir = Environment.GetEnvironmentVariable ("HOME") + 
				    Path.DirectorySeparatorChar + ".didiwiki";
	    if (!Directory.Exists (didiwikidir))
		 Directory.CreateDirectory (didiwikidir);   
	    string path = didiwikidir + Path.DirectorySeparatorChar + "WikiHome";
	    try {
		if (!File.Exists (path))
		{
		    Assembly assembly = System.Reflection.Assembly.GetCallingAssembly ();
		    System.IO.Stream s = assembly.GetManifestResourceStream ("WikiHome");
		    StreamReader reader = new StreamReader (s);
		    string page = reader.ReadToEnd ();
		    reader.Close ();
		    StreamWriter writer = new StreamWriter (File.OpenWrite (path));
		    writer.Write (page);
		    writer.Close ();
		}
	    } catch {
		Console.WriteLine ("Unable to create default files.");
	    }
	}

	public static void Exit ()
	{
	    process.Kill ();
	    Application.Quit ();
	}
    }

}


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

namespace FastOpen
{
    using System;
    using System.IO;
    using System.Reflection;

    public class AppContext
    {

	private static string fastopenrc;

	public static readonly string ConfigDir = Environment.GetEnvironmentVariable ("HOME") + Path.DirectorySeparatorChar +
								+ ".config" + Path.DirectorySeparatorChar +
								+ "fastopen";
	public static readonly string ShortcutsFile = ConfigDir + Path.DirectorySeparatorChar + "fastopenrc";
	

	static AppContext ()
	{
	    LoadDefaultFastopenrc ();
	    try {
		DirectoryInfo dirInfo = new DirectoryInfo (ConfigDir);
		if (!dirInfo.Exists)
		    dirInfo.Create ();
		FileInfo fileInfo = new FileInfo (ShortcutsFile);
		if (!fileInfo.Exists)
		{
		    StreamWriter writer = fileInfo.CreateText ();
		    writer.Write (fastopenrc);
		    writer.Close ();
		}
	    } catch {
		Console.WriteLine ("ERROR: Could not write default settings");
	    }
	}

	public static void Init ()
	{
	}

	public string Fastopenrc
	{
	    get {
		return fastopenrc;
	    }
	}

	private static void LoadDefaultFastopenrc ()
	{
	    Assembly assembly = System.Reflection.Assembly.GetCallingAssembly ();
	    System.IO.Stream s = assembly.GetManifestResourceStream ("fastopenrc");
	    StreamReader reader = new StreamReader (s);
	    fastopenrc = reader.ReadToEnd ();
	}
    }
}

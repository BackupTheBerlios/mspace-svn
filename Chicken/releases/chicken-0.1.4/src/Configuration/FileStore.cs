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

namespace Chicken.Configuration 
{
    using System;
    using System.IO;
    
    public class FileStore
    {
	private string storeName;
	private string location;
	private string home = Environment.GetEnvironmentVariable ("HOME");
	
	public FileStore (string storeName)
	{
	    this.storeName = storeName;
	    string configDir = UserConfigDir ();
	    location = Path.Combine (configDir, storeName);
	    
	    try {
		if ( !Directory.Exists (configDir))
		    Directory.CreateDirectory (configDir);
		
		if (!Directory.Exists (location))
		    Directory.CreateDirectory (location);

	    } catch (Exception) {
		Console.WriteLine ("Error initializing store.");
		Environment.Exit (1);
	    }
	}

	/*
	 * Returns the full path of the file objName or
	 * null if not found.
	 */
	public string this [string objName]
	{
	    get {
		string fpath = location + Path.DirectorySeparatorChar + objName;
		if (File.Exists (fpath))
		    return fpath;
		else
		    return null;
	    }
	}

	/*
	 * Creates a new object in the store and returns the full
	 * path of the new created object.
	 */
	public FileStream CreateFile (string name, bool truncate)
	{
		string fpath = location + Path.DirectorySeparatorChar + name;
		FileStream stream;
		if (!File.Exists (fpath))
		{
		    stream = File.Open (fpath, FileMode.Create, FileAccess.ReadWrite);
		} else {
		    if (truncate)
			stream = File.Open (fpath, FileMode.Truncate, FileAccess.ReadWrite);
		    stream = File.Open (fpath, FileMode.Open, FileAccess.ReadWrite);
		}
		return stream;
	}

	/*
	 * Get or set the location of objects. Location
	 * must be a Directory. If directory does not exist,
	 * it will try to create it using Directory.Create (string).
	 * Directory.Create (string) exceptions are not catched. Client
	 * must handle this case.
	 */
	public string Location {
	    get {
		return location;
	    }

	    set {
		if (!Directory.Exists (location))
		    Directory.CreateDirectory (location);
		location = value;
	    }
	}

	private string UserConfigDir ()
	{
	    return Path.Combine (home, ".config");
	}
    }
}


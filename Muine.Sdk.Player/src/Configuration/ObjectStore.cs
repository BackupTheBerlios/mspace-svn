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

namespace Muine.Sdk.Configuration 
{
    using System;
    using System.IO;
    
    public class ObjectStore
    {
	private string storeName;
	private string location;
	
	public ObjectStore (string storeName)
	{
	    this.storeName = storeName;
	    location = Environment.GetEnvironmentVariable ("HOME") + Path.DirectorySeparatorChar
			+ ".config" + Path.DirectorySeparatorChar + storeName;
	    try {
		if (!Directory.Exists (location))
		    Directory.CreateDirectory (location);
	    } catch (Exception) {
		Console.WriteLine ("Error initializing store.");
	    }
	}

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

	public string CreateObject (string name)
	{
		string fpath = location + Path.DirectorySeparatorChar + name;
		if (!File.Exists (fpath))
		{
		    Stream s = File.Create (fpath);
		    s.Close ();
		}
		return fpath;
	}

	public string Location {
	    get {
		return location;
	    }
	}
    }
}


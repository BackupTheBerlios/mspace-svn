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
    using Nini.Config;
    using System.Reflection;
    using System.IO;

    public class Configuration
    {
	private FileStore objStore;
	private IConfigSource configSource;

	private Configuration ()
	{
	    objStore = new FileStore ("Muine.Sdk");
	    if (objStore["MuineSdk.ini"] == null)
	    {
			CopyDefaultConfigFile ();
	    }
	    configSource = new IniConfigSource (objStore["MuineSdk.ini"]); 
	}

	private void CopyDefaultConfigFile ()
	{
	    Assembly assembly = System.Reflection.Assembly.GetCallingAssembly ();
	    System.IO.Stream s = assembly.GetManifestResourceStream ("MuineSdk.ini");
	    StreamReader reader = new StreamReader (s);
	    string config = reader.ReadToEnd ();
	    reader.Close ();
	    StreamWriter writer = new StreamWriter (objStore.CreateFile ("MuineSdk.ini", false));
	    writer.Write (config);
	    writer.Close ();
	}

	private static Configuration instance;
	public static Configuration GetInstance ()
	{
	    if (instance == null)
	    {
		instance = new Configuration ();
	    }
	    return instance;
	}

	public string UserConfigDir {
	    get {
		return objStore.Location;
	    }
	}

	public string PlayerKitType {
	    set {
	    }

	    get {
			IConfig config = configSource.Configs["Player"];
			return config.GetString ("PlayerKit.Type", "GstPlayer");
	    }
	}
	
	public string PlayerKitAssembly {
		set {
		}
		get {
			IConfig config = configSource.Configs["Player"];
			return config.GetString ("PlayerKit.Assembly", "Gstplayer");
		}
	}

	public string DataKitType {
	    set {
	    }

	    get {
			IConfig config = configSource.Configs["Data"];
			return config.GetString ("DataKit.Type", "SqliteDataKit");
	    }
	}

	public string DataKitAssembly {
		set {
		}
		get {
			IConfig config = configSource.Configs["Data"];
			return config.GetString ("DataKit.Assembly", "SqliteDataKit");
		}
	}

	internal FileStream CreateUserFile (string filename)
	{
	    FileStream f = objStore.CreateFile (filename, false);
	    return f;
	}
	
    }

}


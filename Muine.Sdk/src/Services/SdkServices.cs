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

namespace Player.Services 
{
    using System;    
    using Player.Configuration;
    using Player.Player;
    using Player.Playlist;
    using Player.Data;
    using System.Reflection;
    using System.IO;

    /*
     * Manage Services available in the SDK.
     */
    public class SdkServices
    {
	private static Configuration configuration = Configuration.GetInstance ();
	static SdkServices ()
	{
	    Playlist = new Playlist ();
	    Player = (IPlayer)LoadService ("PlayerKits", configuration.PlayerKitType, configuration.PlayerKitAssembly);
	    MusicDb = (IMusicDb) LoadService ("DataKits", configuration.DataKitType, configuration.DataKitAssembly);
	    Player.Playlist = Playlist;
	}

	public static IPlayer Player;
	public static IMusicDb MusicDb;
	public static IPlaylist Playlist;


	private static object LoadService (string serviceDir, string serviceType, string serviceAssembly)
	{
	    char separator = Path.DirectorySeparatorChar;
	    object serviceObject = null;
	    Assembly asm = null;
	    //First, the service is loaded from the user config dir.
	    //If not, try to the system wide service.
	    try {
			string userConfigDir = Configuration.GetInstance ().UserConfigDir;
			string userServiceLocation = userConfigDir + separator + serviceDir + separator + serviceAssembly + ".dll";
			if (File.Exists (userServiceLocation))
			{
				Console.WriteLine ("Loading {0} from USER services", serviceAssembly);
				asm = Assembly.LoadFrom (userServiceLocation);
				if (asm != null)
					Console.WriteLine ("{0} loaded succesfully", serviceAssembly);
				serviceObject = asm.CreateInstance (serviceType);

			} else {
				Console.WriteLine ("Loading {0} from SYSTEM services", serviceAssembly);
				string sdkDir = Path.GetDirectoryName (Assembly.GetCallingAssembly ().Location);
				string systemServiceLocation = sdkDir + separator + "Player.Sdk" + 
							separator + serviceDir + separator + serviceAssembly + ".dll";
				Console.WriteLine ("Service location: {0}", systemServiceLocation);
				asm = Assembly.LoadFrom (systemServiceLocation);
				if (asm != null)
					Console.WriteLine ("Assembly {0}.dll loaded succesfully", serviceAssembly);
				serviceObject = asm.CreateInstance (serviceType);
			}

			if (serviceObject == null)
			{
				Console.WriteLine ("Error initiating {0}", serviceType);
				throw new ServiceUnavailableException (String.Format ("ERROR: Service {0} unavailable.", serviceType));
			}
			return serviceObject;
					
	    } catch (Exception e) {
			Console.WriteLine (e.StackTrace);
			Console.WriteLine (e.Message);
			throw new ServiceUnavailableException (String.Format ("ERROR: Service {0} unavailable.", serviceType), e);
	    }
	}
	
    }

}


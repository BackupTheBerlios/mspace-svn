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

namespace Muine.Sdk.Services 
{
    using System;    
    using Muine.Sdk.Configuration;
    using Muine.Sdk.Player;
    using Muine.Sdk.Playlist;
    using Muine.Sdk.Data;
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
	    Player = (IPlayer)LoadService ("PlayerKits", configuration.PlayerKit);
	    MusicDb = (IMusicDb) LoadService ("DataKits", configuration.DataKit);
	    Player.Playlist = Playlist;
	}

	public static IPlayer Player;
	public static IMusicDb MusicDb;
	public static IPlaylist Playlist;


	private static object LoadService (string serviceDir, string service)
	{
	    char separator = Path.DirectorySeparatorChar;
	    object serviceObject = null;
	    Assembly asm = null;
	    //First, the service is loaded from the user config dir.
	    //If not, try to the system wide service.
	    try {
		string userConfigDir = Configuration.GetInstance ().UserConfigDir;
		string userServiceLocation = userConfigDir + separator + serviceDir + separator + service + ".dll";
		if (File.Exists (userServiceLocation))
		{
		    asm = Assembly.LoadFrom (userServiceLocation);
		    serviceObject = asm.CreateInstance (service);

		} else {
		    string sdkDir = Path.GetDirectoryName (Assembly.GetCallingAssembly ().Location);
		    string systemServiceLocation = sdkDir + separator + "Muine.Sdk" + 
						    separator + serviceDir + separator + service + ".dll";
		    asm = Assembly.LoadFrom (systemServiceLocation);
		    serviceObject = asm.CreateInstance (service);
		}

		if (serviceObject == null)
		    throw new ServiceUnavailableException (String.Format ("ERROR: Service {0} unavailable.", service));
		return serviceObject;
		    
	    } catch (Exception e) {
		Console.WriteLine (e.StackTrace);
		throw new ServiceUnavailableException (String.Format ("ERROR: Service {0} unavailable.", service), e);
	    }
	}
	
    }

}


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

    public class SimpleEntryParser : IEntryParser
    {
	
	public void Parse (string entry)
	{
	    int index = entry.IndexOf (':');
	    if (index != -1) {
		if ( Array.IndexOf (vfsUrls, entry) != -1 
			|| entry.StartsWith ("http://")
			|| entry.StartsWith ("/"))
		{
		    string sToOpen = entry;
		    if (entry.StartsWith ("/"))
			    sToOpen = "file:" + entry;
		    Gnome.Url.Show (sToOpen);
		} else { 
		    string tmpString;
		    if ((tmpString = GetShortcutURL (entry)) != null) {
			Console.WriteLine ("Sortcut found: " + tmpString);
			Gnome.Url.Show (tmpString);
		    }
		}
	    }
	}
	
	//Returns null if the shortcut is not found
	private string GetShortcutURL (string content)
	{
	    string retvalue = null;
	    int index = content.IndexOf (':');
	    if (index != -1) {
		// command contains :
		string[] stringEntry = content.Split (':');
		string command = stringEntry[0];
		string parameters = stringEntry[1];
		string url = null;
		StreamReader reader = new StreamReader (AppContext.ShortcutsFile);
		string line;
		while ((line = reader.ReadLine ()) != null)
		{
		    string[] tokens = line.Split ('#');
		    if (tokens.Length > 0)
		    {
			//Command found. Replace
			if (tokens[0] == command)
			{
			    url = tokens[1];
			}
		    }
		}
		if (url != null)
		    retvalue = url.Replace ("{@}", parameters);
	    }
	    return retvalue;
	}
	
	private string[] vfsUrls = {
				"preferences:",
				"fonts:",
				"applications:",
				"favorites:",
				"start-here:",
				"system-settings:",
				"server-settings:",
				"burn:",
				"computer:",
				"trash:"
				};
    }

}


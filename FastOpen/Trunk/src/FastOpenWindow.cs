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
    using Gtk;
    using System;
    using System.Diagnostics;
    using System.Collections;
    using System.Text;
    using System.IO;

    public class FastOpenWindow : Window
    {
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

	private StringBuilder buffer = new StringBuilder ();
	private bool ignore = false;

	private Entry entry;

	public FastOpenWindow () : base (WindowType.Toplevel)
	{	
	    Icon = new Gdk.Pixbuf (null, "fastopen.png");
	    entry = new Entry ();
	    entry.Activated += EntryActivated;
	    BorderWidth = 12;
	    Add (entry);
	    WindowPosition = WindowPosition.Center;
	    AppContext.Init ();
	}

	public void EntryActivated (object obj, EventArgs args)
	{
	    ParseEntry (entry.Text);
	    Application.Quit ();
	}

	private void ParseEntry (string entry)
	{
	    if ( Array.IndexOf (vfsUrls, entry) != -1 
		    || entry.StartsWith ("http://")
		    || entry.StartsWith ("/"))
	    {
		string sToOpen = entry;
		if (entry.StartsWith ("/"))
			sToOpen = "file:" + entry;
		Gnome.Url.Show (sToOpen);
	    }
	    else {
		int index = entry.IndexOf (':');
		if (index != -1) {
		    // command contains :
		    string[] stringEntry = entry.Split (':');
		    string command = stringEntry[0];
		    string parameters = stringEntry[1];
		    string url;
		    if ((url = GetShortcutURL (command)) != null)
		    {
			if (url.IndexOf ("{@}") != -1)
			    Gnome.Url.Show (url.Replace ("{@}", parameters));
			else {
			    Gnome.Url.Show (url);
			}
		    }
		} else {
		    try {
			ProcessStartInfo info = new ProcessStartInfo (entry);
			info.UseShellExecute = false;
			Process process = Process.Start (info);
		    } catch {
			Console.WriteLine ("ERROR: Launching process.");
		    }
		}
	    }
	    
	}

	//Returns null if the shortcut is not found
	private string GetShortcutURL (string command)
	{
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
			return tokens[1];
		    }
		}
	    }
	    return null;
	}

	protected override bool OnKeyPressEvent (Gdk.EventKey evt)
	{
	    if (evt.Key == Gdk.Key.Escape)
		Application.Quit ();
	    return base.OnKeyPressEvent (evt);
	}
    }
}

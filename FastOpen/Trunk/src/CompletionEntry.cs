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
    using Gtk;
    using System.IO;
    using System.Collections;

    public class CompletionEntry : Entry
    {
	private int oldLength = 0;
	private bool autocompleting = false;
	private int current = 0;
	public bool explored = false;
	private ArrayList binList;

	public CompletionEntry () : base ()
	{
	    oldLength = Text.Length;
	    //TextInserted += OnTextInserted;
	}
	
	private string AutoComplete (string text)
	{
	    if (text.StartsWith (":") || text == String.Empty)
		return null;
	    if (!explored)
	    {
		string[] paths = Environment.GetEnvironmentVariable ("PATH").Split (System.IO.Path.PathSeparator);
		binList = new ArrayList (1500);
		foreach (string path in paths)
		{
		    if (Directory.Exists (path))
		    {
			string[] files = Directory.GetFiles (path);
			foreach (string file in files)
			    binList.Add (System.IO.Path.GetFileNameWithoutExtension (file));
		    }
		}
		explored = true;
	    }
	    foreach (string s in binList)
	    {
		if (s.StartsWith (text))
		    return s;
	    }
	    return null;
	}

	protected override bool OnKeyPressEvent (Gdk.EventKey evt)
	{
	    base.OnKeyPressEvent (evt);
	    if (evt.Key == Gdk.Key.BackSpace)
		return false;
	    DeleteSelection ();
	    //Fake speed
	    if (Text.Length > 2)
	    {
		string bin = AutoComplete (Text);
		if (bin != null)
		{
		    int oldLength = Text.Length;
		    Text = bin;
		    SelectRegion (oldLength, Text.Length);
		}
	    }
	    return true;
	}

    }
}


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
	private bool explored = false;
	private ArrayList binList;
	private ArrayList pathList;
	private ArrayList completions = new ArrayList ();
	private int completionCounter = 0;
	private char separator = System.IO.Path.DirectorySeparatorChar;
	
	/*
	 * If binary has bin found in PATH matching the text in entry
	 * return true.
	 * Else return false.
	 */
	private bool binFound = false;
	public bool BinFound {
	    get {
		return binFound;
	    }
	}

	//FIXME
	//This can be optimized removing from completions 
	//the entries not needed.
	private void AutoComplete (string text)
	{
	    completions.Clear ();
	    completionCounter = 0;
    
	    //Do it only the first time we search the PATH for binaries.
	    //bins are catched in binList after that
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
			    binList.Add (System.IO.Path.GetFileName (file));
		    }
		}
		explored = true;
	    }
	    
	    //Add to completions the matching binaries
	    foreach (string s in binList)
	    {
		if (s.StartsWith (text))
		    completions.Add (s);
	    }
	}

	private void AutoCompletePath (string path)
	{
	    completions.Clear ();
	    completionCounter = 0;
	    string baseDir = System.IO.Path.GetDirectoryName (path);
	    string file = System.IO.Path.GetFileName (path);
	    if (Directory.Exists (baseDir))
	    {
		string[] files = Directory.GetFileSystemEntries (baseDir);
		foreach (string s in files)
		{
		    string stripped = System.IO.Path.GetFileName (s);
		    if (stripped.StartsWith (file))
			completions.Add (s);
		}
	    }
	}

	protected override bool OnKeyPressEvent (Gdk.EventKey evt)
	{
	    base.OnKeyPressEvent (evt);
	    if (AcceptKey (evt)) {
		DeleteSelection ();
		int oldLength = Text.Length;
		string binName;

		if (evt.Key == Gdk.Key.Tab) {	
		    if (completions.Count > 1) {
			binName = completions[completionCounter] as string;
			Text = binName;
			SelectRegion (oldLength, Text.Length);
			if (completionCounter < completions.Count - 1) completionCounter++;

		    } else if (completions.Count == 1) {
			Text = completions[0] as string;
			Position = Text.Length;	
		    }
		    
		} else if (Text.StartsWith (separator.ToString ())) {
		    AutoCompletePath (Text);
		    if (completions.Count > 0) {
			string first = completions[0] as string;
			Text = first; 
			SelectRegion (oldLength, Text.Length);
		    }
		} else {
		    AutoComplete (Text);
		    if (completions.Count > 0) {
			    binFound = true;
			    string first = completions[0] as string;
			    binName = first;
			    Text = binName;
			    SelectRegion (oldLength, Text.Length);
		    } else
			binFound = false;
		}
		    
	    }
	    return true;
	}

	private bool AcceptKey (Gdk.EventKey evt)
	{
	    uint key = evt.KeyValue;
	    if ((key >= 33 && key <= 126)   || 
		(evt.Key == Gdk.Key.Tab))
		return true;
	    return false;
	}


    }
}


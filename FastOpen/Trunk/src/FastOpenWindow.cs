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
    using System.Collections;
    using System.Text;
    using System.IO;
    using System.Diagnostics;

    public class FastOpenWindow : Window
    {

	private StringBuilder buffer = new StringBuilder ();
	private bool ignore = false;

	private CompletionEntry entry;
	public IEntryParser parser = new SimpleEntryParser ();

	public FastOpenWindow () : base (WindowType.Toplevel)
	{	
	    Icon = new Gdk.Pixbuf (null, "fastopen.png");
	    entry = new CompletionEntry ();
	    entry.Activated += EntryActivated;
	    BorderWidth = 12;
	    DefaultWidth = 300;
	    Add (entry);
	    WindowPosition = WindowPosition.Center;
	    AppContext.Init ();
	}

	public void EntryActivated (object obj, EventArgs args)
	{
	    if (!entry.BinFound)
	    	parser.Parse (entry.Text);
	    else {
		string[] command = entry.Text.Split (new char[]{' '}, 2);
		try {
		    ProcessStartInfo info = new ProcessStartInfo (entry.Text);
		    info.UseShellExecute = true;
		    Process process;
		    if (command.Length == 1)
			process = Process.Start (command[0]);
		    else 
			process = Process.Start (command[0], command[1]);
		} catch {
		    Console.WriteLine ("ERROR: Launching process.");
		}
	    }
	    Application.Quit ();
	}


	protected override bool OnKeyPressEvent (Gdk.EventKey evt)
	{
	    if (evt.Key == Gdk.Key.Escape)
		Application.Quit ();
	    return base.OnKeyPressEvent (evt);
	}

	protected override bool OnDeleteEvent (Gdk.Event evt)
	{
	    Application.Quit ();
	    return false;
	}
    }
}

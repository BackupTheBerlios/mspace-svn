
namespace FastOpen
{
    using Gtk;
    using Glade;
    using System;
    using System.Diagnostics;
    using System.Collections;
    using System.Text;
    using System.IO;

    public class FastOpenWindow
    {

	[Glade.Widget] Window window;
	[Glade.Widget] Entry entry;

	XML gxml;
	CommandList cmdList = new CommandList ();
	private StringBuilder buffer = new StringBuilder ();
	private bool ignore = false;

	public FastOpenWindow ()
	{	
	    InitComponent ();
	}

	public void InitComponent ()
	{
	    gxml = new XML (null, "fastopen.glade", "window", null);
	    gxml.Autoconnect (this);
	    AppContext.Init ();
	}

	public void EntryActivated (object obj, EventArgs args)
	{
	    ParseEntry (entry.Text);
	    Application.Quit ();
	}

	private void ParseEntry (string entry)
	{
	    int index = entry.IndexOf (':');
	    try {
		//Let's see if gnome-vfs handles the uri
		Gnome.Url.Show (entry);
	    } catch (GLib.GException) {
	    
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

	public void Changed (object obj, EventArgs args)
	{
	    entry.FinishEditing ();
	    entry.SelectRegion (0, entry.CursorPosition);
	}
	
	public void TextInserted (object obj, TextInsertedArgs args)
	{
	    ArrayList cmds = cmdList.CompleteCommand (entry.Text);
	    if (cmds.Count == 1)
	    {
		entry.Text = (string)cmds[0];
	    }
		
	}

	public void KeyPressed (object obj, KeyPressEventArgs args)
	{
	    if (args.Event.Key == Gdk.Key.Escape)
		Application.Quit ();
	}
    }
}

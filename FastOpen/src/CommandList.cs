using System;
using System.Collections;

namespace FastOpen
{

    public class CommandList : IEnumerable
    {
	private Hashtable hash = new Hashtable ();

	public CommandList ()
	{
	    InitComponent ();
	}

	public void InitComponent ()
	{
	    foreach (string s in predefinedCommands)
		hash.Add (s, s);
	}

	public ArrayList CompleteCommand (string s)
	{
	    ArrayList list = new ArrayList ();
	    foreach (string cmd in predefinedCommands)
	    {
		if (cmd.StartsWith (s))
		    list.Add (cmd);
	    }
	    return list;
	}

	public IEnumerator GetEnumerator ()
	{
	    return hash.GetEnumerator ();
	}
	
	
	string[] predefinedCommands = {
	    "gnome_terminal",
	    "gaim",
	    "xchat",
	    "mozilla-firefox",
	    "firefox",
	    "mozilla",
	    "galeon",
	    "xterm",
	    "monodoc",
	    "monodevelop",
	    "muine",
	    "blaam",
	    "obconf",
	    "epiphany",
	    "gdesklets",
	    "glade-2",
	    "meld",
	    "glade",
	    "nautilus",
	    "evolution"
	};
    }
    
}

	

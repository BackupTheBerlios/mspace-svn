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


using System;
using Gtk;
using Glade;

[assembly:PluginInfo (
			"MonitorPlugin",
			"Monitor Plugin",
			"Prints all the Muine evets to a tiny gtk interface",
			PluginType.Passive)]


public class MonitorPlugin : IPlugin
{
    
    [Glade.Widget] TextView textview;
    
    private string gladeFile = "monitor-window.glade";

    public MonitorPlugin ()
    {
	InitComponets ();
    }

    private void InitComponets ()
    {
	XML gxml = new XML (null, gladeFile, "window", null);
	gxml.Autoconnect (this);
    }
    
    public IBusFilter Filter { 
	    get {
		    return new TestFilter ();
	    }
	    set {
	    }
    }
	    
     public bool MessagePosted(Message msg)
     {
	    textview.Buffer.Text += "\nEVENT: " + msg.ToString ();
	    return true;
     }
     
     private void ClearClicked (object obj, EventArgs args)
     {
     }

     private class TestFilter : IBusFilter
     {
	    public bool Accept (Message msg)
	    {
		if (msg is PlayerEvent)
		    return true;
		return false;
	    }
     }
     
     public static void Main (string[] args)
     {
	 new MonitorPlugin ();
     }
}

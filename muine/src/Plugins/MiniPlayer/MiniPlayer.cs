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
			"MiniPlayer",
			"Mini Player",
			"Minimal player interface",
			PluginType.Active)]


public class MiniPlayer : IPlugin
{
    
    private string gladeFile = "miniplayer.glade";
    private XML gxml;
    private bool playing = false;

    public MiniPlayer ()
    {
	InitComponets ();
    }

    private void InitComponets ()
    {
	gxml = new XML (null, gladeFile, "window", null);
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
	 return true;
     }
     
     private void PlayPauseClicked (object obj, EventArgs args)
     {
	 PlayerAction.ActionType type = playing ? PlayerAction.ActionType.Play :
						 PlayerAction.ActionType.Pause;
	 AppContext.ABus.PostAction ( new PlayerAction (this, type));
	 Console.WriteLine ("Action sent");
     }
     
     private void CloseClicked (object obj, EventArgs args)
     {
	 gxml["window"].Destroy ();
     }

     private class TestFilter : IBusFilter
     {
	    public bool Accept (Message msg)
	    {
		return false;
	    }
     }
     
}

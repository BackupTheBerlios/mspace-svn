
// created on 7/19/2004 at 9:10 PM
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

[assembly:PluginInfo (
				"TestPlugin",
				"Test Plugin",
				"Description",
				PluginType.Passive)]

public class TestPlugin : IPlugin
{
	public IBusFilter Filter { 
		get {
			return new PlayerEventFilter ();
		}
		set {
		}
	}
		
	 public bool MessagePosted(Message msg)
	 {
	 	Console.WriteLine("PLUGIN: Message arrived. {0}", msg);
	 	return true;
	 }
	 
	 private class TestFilter : IBusFilter
	 {
	 	public bool Accept (Message msg)
	 	{
	 		return true;
	 	}
	 }
}
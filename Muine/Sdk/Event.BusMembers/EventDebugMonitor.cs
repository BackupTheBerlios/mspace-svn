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

/**
 * Class that prints all events to stderr.
 *
 */
public class EventDebugMonitor : IBusMember
{

    

    public EventDebugMonitor()
	{
    }

    /** 
     * Get the filter to that is used to determine if an event should
     * to to the member.
     */
	 /** Filter for all events. */
    private IBusFilter filter;
    public IBusFilter Filter
	{
        get {
			return null;
	}
	set {
			filter = value;
	}
    }
        
    /** 
     * Called when an event is to be posed to the member.
     * 
     */
    public bool MessagePosted(Message evt) 
	{
        Console.WriteLine (evt);
		return true;
    }

}

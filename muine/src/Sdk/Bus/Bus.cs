// created on 7/19/2004 at 12:09 PM
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
using System.Collections;

public abstract class Bus
{
    private ArrayList busMembers = new ArrayList ();

    public void AddMember(IBusMember member) 
    {
    	//Precondition violated
    	if (member == null)
    		throw new ArgumentNullException ("IBusMember is null");
		if (!busMembers.Contains (member))
		{
		    lock (busMembers)
			busMembers.Add (member);
		} else {
		    Console.WriteLine ("WARNING: Member is already in the bus. Skipping");
		}
    }

    public void RemoveMember(IBusMember member)
    {
		lock (busMembers)
		    busMembers.Remove (member);
    }
    
    //This should be done in a thread.???
    /*
     * Buses should call this method to post a message.
     * They should provide a facade to this method.
     * See ActionBus as an example.
     */
    public void PostMessage(Message msg)
    {
		lock (busMembers)
		{
		    foreach (IBusMember member in busMembers)
		    {
		    	/*
		    	 * Safety net.
		    	 * this will safe us from any plugin exception
		    	 */
		    	try {
				    //Accept could be used internally by BusMember
				    if (member.Filter == null || member.Filter.Accept (msg))
				    {
					    if (!member.MessagePosted (msg))
						    //Message has been cancelled. exit the loop.
						    break;
				    }
			    } catch (Exception e) {
#if DEBUG_PLUGINS
					Console.WriteLine ("PLUGIN ERROR: Pluging has thrown a exception.");
#endif			 
				}   
		    }
		}
    }
}

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
 
public class PlayerEvent : EventMessage
{
	public enum EventType {
		Play,
		SongChanged,
		Stop,
		Pause,
		Next,
		Previous,
		Volume,
		Seek
    }
    
    public PlayerEvent (object source, PlayerEvent.EventType type) : base (source)
    {
    	this.type = type;
    }
    
    public PlayerEvent (object source, PlayerEvent.EventType type, Song song)
    	: this (source, type)
    {
    	this.song = song;
    }
    
    public PlayerEvent (object source, PlayerEvent.EventType type, Song song, int pos)
    	: this (source, type, song)
    {
    	this.position = pos;
    }
    
    private Song song;
    public Song Song {
    	get {
    		return Song;
    	}
    }
    
    private int position;
    public int Position {
    	get {
    		return position;
    	}
    }
    
    
    private EventType type;
    public EventType Type {
    	get {
    		return type;
    	}
    }
}


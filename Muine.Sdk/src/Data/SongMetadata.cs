/*
 * Copyright (C) 2004 Jorn Baayen <jorn@nl.linux.org>
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


namespace Muine.Sdk.Data
{
    using System;
    
    public class SongMetadata 
    {
	    private string title;
	    public string Title {
		    get {
			    return title;
		    }
	    }

	    private string [] artists;
	    public string [] Artists {
		    get {
			    return artists;
		    }
	    }

	    private string [] performers;
	    public string [] Performers {
		    get {
			    return performers;
		    }
	    }

	    private string album;
	    public string Album {
		    get {
			    return album;
		    }
	    }

	    private int track_number;
	    public int TrackNumber {
		    get {
			    return track_number;
		    }
	    }

	    private string year;
	    public string Year {
		    get {
			    return year;
		    }
	    }

	    private int duration;
	    public int Duration {
		    get {
			    return duration;
		    }
	    }

	    private string mime_type;
	    public string MimeType {
		    get {
			    return mime_type;
		    }
	    }

	    private int mtime;
	    public int MTime {
		    get {
			    return mtime;
		    }
	    }

	    private double gain;
	    public double Gain {
		    get {
			    return gain;
		    }
	    }

	    private double peak;
	    public double Peak {
		    get {
			    return peak;
		    }
	    }
    }
}

/*
 * Copyright (C) 2003, 2004 Jorn Baayen <jorn@nl.linux.org>
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
    using System.Collections;
    using System.IO;

    public class Song : ISearchable
    {
	    private string filename;
	    public string Filename {
		    get {
			    return filename;
		    }
	    }
		    
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
		    /* we have a setter too, because sometimes we want
		     * to correct the duration. */
		    set {
			    duration = value;
		    }
		    
		    get {
			    return duration;
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

	    private string sort_key = null;
	    public string SortKey {
		    get {
			    if (sort_key == null) {
				    string a = String.Join (" ", artists).ToLower ();
				    string p = String.Join (" ", performers).ToLower ();
				    
			    }
			    
			    return sort_key;
		    }
	    }

	    private string search_key = null;
	    public string SearchKey {
		    get {
			    if (search_key == null) {
				    string a = String.Join (" ", artists).ToLower ();
				    string p = String.Join (" ", performers).ToLower ();
				    
				    search_key = title.ToLower () + " " + a + " " + p + " " + album.ToLower ();
			    }

			    return search_key;
		    }
	    }

	    public SongMetadata Metadata
	    {
		set {
		    SongMetadata metadata = value;
		    if (metadata.Title.Length > 0)
			    title = metadata.Title;
		    else
			    title = Path.GetFileNameWithoutExtension (filename);
		    
		    artists = metadata.Artists;
		    performers = metadata.Performers;
		    album = metadata.Album;
		    track_number = metadata.TrackNumber;
		    year = metadata.Year;
		    duration = metadata.Duration;
		    mtime = metadata.MTime;
		    gain = metadata.Gain;
		    peak = metadata.Peak;

		    sort_key = null;
		    search_key = null;

		    //GetCoverImage (metadata);
		}
	    }

	    public Song (string fn)
	    {
		this.filename = fn;
	    }

	    public bool FitsCriteria (string [] search_bits)
	    {
		    /*int n_matches = 0;
			    
		    foreach (string search_bit in search_bits) {
			    if (SearchKey.IndexOf (search_bit) >= 0) {
				    n_matches++;
				    continue;
			    }
		    }

		    return (n_matches == search_bits.Length);*/
		return false;
	    }
    }
}

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

namespace Player.Data
{
    using System;
    using System.Collections;

    public class SongMetadata
    {
	    public string Filename;
	    public string Title;
	    public ArrayList Artists;
	    public ArrayList Performers;
	    public string Album;
	    public int TrackNumber;
	    public int Year;
	    public int Duration;
	    public int MTime;
	    public double Gain;
	    public double Peak;
    }
}

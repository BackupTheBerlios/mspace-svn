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
using System.Text;
using System.Collections;


public class SongsStore : DataStore
{

	public SongsStore () : base (typeof (string)) 
	{
	}

	public override TreeIter AppendMedia (object obj)
	{
	    Song s = (Song) obj;
	    StringBuilder builder = new StringBuilder ();
	    builder.Append ("<b>");
	    builder.Append (s.Album);
	    builder.Append ("</b>");
	    builder.Append ("\n");
	    builder.Append (StringUtils.JoinHumanReadable (s.Artists));
	    return base.AppendValues (builder.ToString ());
	}

	public override ICollection Media {
	    get {
		return AppContext.DB.Songs.Values;
	    }

	}
}


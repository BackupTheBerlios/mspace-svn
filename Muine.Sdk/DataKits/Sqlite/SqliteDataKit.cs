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

namespace Player.Data.Kits 
{
    using System;
    using Player.Data;
    using Player.Configuration;
    using System.Collections;
    using Mono.Data.SqliteClient;
    using System.IO;
    using System.Text;

    public class SqliteDataKit : IMusicDb
    {
	private SqliteConnection conn;
	private string dbfile = "musicdb.sqlite";
	private bool isNew = false;

	public SqliteDataKit ()
	{
	    conn = new SqliteConnection ();
	    if (!File.Exists (Path.Combine (Configuration.GetInstance ().UserConfigDir, dbfile)))
		isNew = true;
	    conn.ConnectionString = "URI=file:" + Path.Combine (Configuration.GetInstance ().UserConfigDir, dbfile);
	    conn.Open ();
	    if (isNew)
		SetupDb ();
	}

	public ArrayList Songs {
	    get {
		ArrayList songs = new ArrayList ();
		SqliteCommand cmd = new SqliteCommand ();
		cmd.Connection = conn;
		cmd.CommandText = "SELECT * FROM songs";
		SqliteDataReader reader = cmd.ExecuteReader ();
		while (reader.Read ())
		{
		    SongMetadata sm = new SongMetadata ();
		    sm.Filename = reader[1].ToString ();
		    sm.Title = reader[2].ToString ();
		    sm.Artists = SqlStringToArray (reader[3].ToString ());
		    sm.Performers = SqlStringToArray (reader[4].ToString ());
		    sm.Album = reader[5].ToString ();
		    sm.TrackNumber = Int32.Parse (reader[6].ToString ());
		    sm.Year = Int32.Parse (reader[7].ToString ());
		    sm.Duration = Int32.Parse (reader[8].ToString ());
		    sm.MTime = Int32.Parse (reader[9].ToString ());
		    sm.Gain = Double.Parse (reader[10].ToString ());
		    Song s =  new Song (sm);
		    songs.Add (s); 
		}
		cmd.Dispose ();
		return songs;
	    }
	}

	public ArrayList Albums {
	    get {
		ArrayList albums = new ArrayList ();
		SqliteCommand cmd = new SqliteCommand ();
		cmd.Connection = conn;
		cmd.CommandText = "SELECT * FROM albums";
		SqliteDataReader reader = cmd.ExecuteReader ();
		while (reader.Read ())
		{
		    AlbumMetadata am = new AlbumMetadata ();
		    am.Name = reader[1].ToString ();
		    am.Songs = SqlStringToArray (reader[2].ToString ());
		    am.Artists = SqlStringToArray (reader[3].ToString ());
		    am.Performers = SqlStringToArray (reader[4].ToString ());
		    am.Year = Int32.Parse (reader[5].ToString ());
		    Album album = new Album (am);
		    albums.Add (album); 
		}
		cmd.Dispose ();
		return albums;
	    }
	}

	public bool AddSong (Song song)
	{
	    string artists = ArrayToSqlString (song.Artists);
	    string performers = ArrayToSqlString (song.Performers);

	    SqliteCommand cmd = new SqliteCommand ();
	    cmd.Connection = conn;
	    cmd.CommandText = String.Format ("INSERT INTO songs (id, filename, title, artists, performers, album, tracknumber, year, duration, mtime, gain) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')",
						song.GetHashCode (),
						song.Filename,
						song.Title,
						artists,
						performers,
						song.Album,
						song.TrackNumber,
						song.Year,
						song.Duration,
						song.MTime,
						song.Gain);
	    int res = cmd.ExecuteNonQuery ();
	    if (res > 0)
		return true;
	    return false;
	}

	public bool AddAlbum (Album album)
	{

	    string songs = ArrayToSqlString (album.Songs);
	    string artists = ArrayToSqlString (album.Artists);
	    string performers = ArrayToSqlString (album.Performers);
	    
	    SqliteCommand cmd = new SqliteCommand ();
	    cmd.Connection = conn;
	    cmd.CommandText = String.Format ("INSERT INTO albums (id, name, songs, artists, performers, year) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
						album.GetHashCode (),
						album.Name,
						songs,
						artists,
						performers,
						album.Year);
	    int res = cmd.ExecuteNonQuery ();
	    if (res > 0)
		return true;
	    return false;
	}

	public bool RemoveSong (Song song)
	{
	    SqliteCommand cmd = new SqliteCommand ();
	    cmd.Connection = conn;
	    cmd.CommandText = String.Format ("REMOVE FROM songs WHERE id = {0}", song.GetHashCode ());
	    int res = cmd.ExecuteNonQuery ();
	    if (res > 0)
		return true;
	    return false;
	}

	public bool RemoveAlbum (Album album)
	{
	    SqliteCommand cmd = new SqliteCommand ();
	    cmd.Connection = conn;
	    cmd.CommandText = String.Format ("REMOVE FROM albums WHERE id = {0}", album.GetHashCode ());
	    int res = cmd.ExecuteNonQuery ();
	    if (res > 0)
		return true;
	    return false;
	}

	private void SetupDb ()
	{
	    SqliteCommand command = new SqliteCommand ();
	    command.Connection = conn;
	    command.CommandText = 
		 "CREATE TABLE songs (						    " +
                        "       id		    INTEGER PRIMARY KEY NOT NULL,   	" +
                        "       filename	    STRING  NOT NULL,		    	" +
                        "       title		    STRING  NOT NULL,               " +
                        "       artists		    STRING  NOT NULL,               " +
                        "       performers	    STRING  NOT NULL,               " +
                        "       album		    STRING  NOT NULL,               " +
                        "       tracknumber	    INTEGER NOT NULL,               " +
                        "       year		    INTEGER NOT NULL,               " +
                        "       duration	    INTEGER NOT NULL,               " +
                        "       mtime		    INTEGER NOT NULL,               " +
                        "       gain		    FLOAT   NOT NULL                " +
                        ")";
	    command.ExecuteNonQuery ();
	    command.Dispose ();
	    
	    command = new SqliteCommand ();
	    command.Connection = conn;
	    command.CommandText = 
		 "CREATE TABLE albums (						    " +
                        "       id		    INTEGER PRIMARY KEY NOT NULL,	" +
                        "       name		    STRING  NOT NULL,           " +
                        "       artists		    STRING  NOT NULL,           " +
                        "       songs		    STRING  NOT NULL,           " +
                        "       performers	    STRING  NOT NULL,           " +
                        "       year		    INTEGER NOT NULL           " +
                        ")";
	    command.ExecuteNonQuery ();
	    command.Dispose ();
	}
	
	public string ArrayToSqlString (ArrayList list)
	{
	    StringBuilder builder = new StringBuilder ();
	    foreach (string s in list)
	    {
		builder.Append (s);
		builder.Append ("%");
	    }
	    return builder.ToString ();
	}

	public ArrayList SqlStringToArray (string list)
	{
	    string[] tokens = list.Split ('%');
	    ArrayList array = new ArrayList ();
	    foreach (string s in tokens)
	    {
		array.Add (s);	
	    }
	    return array;
	}
    }


}


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

using System;
using System.Runtime.InteropServices;

public class Player : GLib.Object
{

	private bool stopped = true;

	/*
	 * Access to the Song we are playing.
	 */
	private Song song = null;
	public Song Song {
		get {
			return song;
		}

		set {
			stopped = false;
			
			song = value;

			IntPtr error_ptr;

			player_set_file (Raw, song.Filename, out error_ptr);
			if (error_ptr != IntPtr.Zero) {
				string error = GLib.Marshaller.PtrToStringGFree (error_ptr);

				new ErrorDialog (String.Format (AppContext.Catalog.GetString ("Error opening {0}:\n{1}"),
				                                                         song.Filename, error));
			}
			
			player_set_replaygain (Raw, song.Gain, song.Peak);

			if (TickEvent != null)
				TickEvent (0);

			if (playing)
				player_play (Raw);
			/* Posting the event to the Bus */
			PlayerEvent evt = new PlayerEvent 
						(this, PlayerEvent.EventType.SongChanged, Song, 0);
			AppContext.EBus.PostEvent (evt);
		}
	}


	public delegate void StateChangedHandler (bool playing);
	public event StateChangedHandler StateChanged;
	
	/*
	 * Indicates whether the player is playing or paused.
	 */
	private bool playing;
	public bool Playing {
		get {
			return playing;
		}

		set {
			if (playing == value)
				return;
				
			playing = value;

			if (playing)
				player_play (Raw);
			else
				player_pause (Raw);

			if (StateChanged != null)
				StateChanged (playing);
				
			PlayerEvent.EventType t = playing ? PlayerEvent.EventType.Play
												: PlayerEvent.EventType.Pause;
			PlayerEvent evt = new PlayerEvent 
						(this, t, Song, Position);
			AppContext.EBus.PostEvent (evt);
		}
	}


	/*
	 * Stop playing the song.
	 */
	public void Stop ()
	{
		if (stopped)
			return;
			
		player_stop (Raw);
		stopped = true;

		if (playing == false)
			return;

		playing = false;

		if (StateChanged != null)
			StateChanged (playing);
		PlayerEvent evt = new PlayerEvent 
						(this, PlayerEvent.EventType.Stop, Song, Position);
		AppContext.EBus.PostEvent (evt);
	}

	public delegate void TickEventHandler (int pos);
	public event TickEventHandler TickEvent;

	/*
	 * Set or get the position in the song.
	 */
	public int Position {
		get {
			return player_tell (Raw);
		}

		set {
			if (stopped)
				Song = song;
				
			player_seek (Raw, value);

			if (TickEvent != null)
				TickEvent (value);
			PlayerEvent evt = new PlayerEvent 
						(this, PlayerEvent.EventType.Seek, Song, value);
			AppContext.EBus.PostEvent (evt);
		}
	}

	/*
	 * Controls the volume level of the player;
	 */
	public int Volume {
		get {
			return player_get_volume (Raw);
		}

		set {
			player_set_volume (Raw, value);
			PlayerEvent evt = new PlayerEvent 
						(this, PlayerEvent.EventType.Volume, Song, Position);
			AppContext.EBus.PostEvent (evt);
		}
	}

	/*
	 * Main constructor
	 */
	public Player () : base (IntPtr.Zero)
	{
		IntPtr error_ptr;
		
		Raw = player_new (out error_ptr);
		if (error_ptr != IntPtr.Zero) {
			string error = GLib.Marshaller.PtrToStringGFree (error_ptr);

			throw new Exception (error);
		}
		
		ConnectInt.g_signal_connect_data (Raw, "tick", new IntSignalDelegate (TickCallback),
		                                  IntPtr.Zero, IntPtr.Zero, 0);
		Connect.g_signal_connect_data (Raw, "end_of_stream", new SignalDelegate (EosCallback),
				               IntPtr.Zero, IntPtr.Zero, 0);
		ConnectString.g_signal_connect_data (Raw, "error", new StringSignalDelegate (ErrorCallback),
				                     IntPtr.Zero, IntPtr.Zero, 0);

		playing = false;
		song = null;
	}

	~Player ()
	{
		Dispose ();
	}

	private delegate void SignalDelegate (IntPtr obj);
	private delegate void IntSignalDelegate (IntPtr obj, int i);
	private delegate void StringSignalDelegate (IntPtr obj, string s);

	private void TickCallback (IntPtr obj, int pos)
	{	
		if (TickEvent != null)
			TickEvent (pos);
	}

	public delegate void EndOfStreamEventHandler ();
	public event EndOfStreamEventHandler EndOfStreamEvent;

	private void EosCallback (IntPtr obj)
	{
		if (EndOfStreamEvent != null)
			EndOfStreamEvent ();
	}

	private void ErrorCallback (IntPtr obj, string error)
	{
		new ErrorDialog (String.Format (AppContext.Catalog.GetString ("Audio backend error:\n{0}"), error));
	}

	/***********************************************************/	
	/********************** Native calls ***********************/
	/***********************************************************/	
	private class Connect
	{
		[DllImport ("libgobject-2.0-0.dll")]
		public static extern uint g_signal_connect_data (IntPtr obj, string name,
	                                                         SignalDelegate cb, IntPtr data,
							         IntPtr p, int flags);
	}

	private class ConnectInt
	{
		[DllImport ("libgobject-2.0-0.dll")]
		public static extern uint g_signal_connect_data (IntPtr obj, string name,
	                                                         IntSignalDelegate cb, IntPtr data,
							         IntPtr p, int flags);
	}

	private class ConnectString
	{
		[DllImport ("libgobject-2.0-0.dll")]
		public static extern uint g_signal_connect_data (IntPtr obj, string name,
	                                                         StringSignalDelegate cb, IntPtr data,
							         IntPtr p, int flags);
	}

	[DllImport ("libmuine")]
	private static extern bool player_set_file (IntPtr player,
	                                            string filename,
						    out IntPtr error_ptr);
	[DllImport ("libmuine")]
	private static extern IntPtr player_new (out IntPtr error_ptr);
	
	[DllImport ("libmuine")]
	private static extern void player_set_volume (IntPtr player,
						      int volume);
	[DllImport ("libmuine")]
	private static extern int player_get_volume (IntPtr player);
	
	[DllImport ("libmuine")]
	private static extern void player_seek (IntPtr player,
	                                        int t);
	[DllImport ("libmuine")]
	private static extern int player_tell (IntPtr player);
	
	[DllImport ("libmuine")]
	private static extern void player_play (IntPtr player);
	
	[DllImport ("libmuine")]
	private static extern void player_pause (IntPtr player);
	
	[DllImport ("libmuine")]
	private static extern void player_stop (IntPtr player);
	
	[DllImport ("libmuine")]
	private static extern void player_set_replaygain (IntPtr player,
							  double gain,
							  double peak);
}
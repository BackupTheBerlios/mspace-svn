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

using System;
using System.IO;

using Gtk;
using GLib;
using Gdk;

public class Muine : Gnome.Program
{
	private static PlaylistWindow playlist;

	private static MessageConnection conn;

	private static Gnome.Client client;

	private static bool opened_playlist = false;

	public static void Main (string [] args)
	{
		Muine muine = new Muine (args);

		Application.Run ();
	}

	public Muine (string [] args) : base ("muine", About.Version, Gnome.Modules.UI, args)
	{
		AppContext context = new AppContext ();
		/* Create message connection */
		conn = new MessageConnection ();
		if (!conn.IsServer) {
			ProcessCommandLine (args, true);
			conn.Close ();
			Gdk.Global.NotifyStartupComplete ();
			Environment.Exit (0);
		}

		

		/* Register stock icons */

		/* Set default window icon */
		SetDefaultWindowIcon ();

		AppContext.DB.Load ();

		/* Create playlist window */
		playlist = new PlaylistWindow ();

		/* Hook up connection callback */
		conn.SetCallback (new MessageConnection.MessageReceivedHandler (HandleMessageReceived));
		ProcessCommandLine (args, false);

		/* Load playlist */
		if (!opened_playlist)
			playlist.RestorePlaylist ();

		/* Show playlist window */
		playlist.Run ();

		/* Now we load the album covers, and after that start the changes thread */
		AppContext.CoverDB.DoneLoading += new CoverDatabase.DoneLoadingHandler (HandleCoversDoneLoading);
		
		AppContext.CoverDB.Load ();

		/* And finally, check if this is the first start */
		/* FIXME we dont do this for now as the issue isn't sorted out yet */
		//playlist.CheckFirstStartUp ();

		/* Hook up to the session manager */
		client = Gnome.Global.MasterClient ();

		client.Die += new EventHandler (HandleDieEvent);
		client.SaveYourself += new Gnome.SaveYourselfHandler (HandleSaveYourselfEvent);
		
	}

	private void ProcessCommandLine (string [] args, bool use_conn)
	{
		if (args.Length > 0) {
			for (int i = 0; i < args.Length; i++) {
				System.IO.FileInfo finfo = new System.IO.FileInfo (args [i]);
			
				if (finfo.Exists) {
					if (FileUtils.IsPlaylist (args [i])) {
						/* load as playlist */
						if (use_conn)
							conn.Send ("LoadPlaylist " + finfo.FullName);
						else
							playlist.OpenPlaylist (finfo.FullName);
					} else {
						/* load as music file */
						if (use_conn)
							if (i == 0)
								conn.Send ("PlayFile " + finfo.FullName);
							else
								conn.Send ("QueueFile " + finfo.FullName);
						else
							if (i == 0)
								playlist.PlayFile (finfo.FullName);
							else
								playlist.QueueFile (finfo.FullName);
					}

					opened_playlist = true;
				}
			}
		} else if (use_conn)
			conn.Send ("ShowWindow");
	}

	private void SetDefaultWindowIcon ()
	{
		Pixbuf [] default_icon_list = new Pixbuf [1];
		default_icon_list [0] = new Pixbuf (null, "muine.png");
		Gtk.Window.DefaultIconList = default_icon_list;
	}

	private void HandleMessageReceived (string message,
					    IntPtr user_data)
	{
		if (message == "ShowWindow")
			playlist.WindowVisible = true;
		else if (message.StartsWith ("LoadPlaylist "))
			playlist.OpenPlaylist (message.Substring (13));
		else if (message.StartsWith ("PlayFile "))
			playlist.PlayFile (message.Substring (9));
		else if (message.StartsWith ("QueueFile "))
			playlist.QueueFile (message.Substring (10));
	}

	private void HandleCoversDoneLoading ()
	{
		/* covers done loading, start the changes thread */
		AppContext.DB.CheckChanges ();
	}

	private void HandleDieEvent (object o, EventArgs args)
	{
		Exit ();
	}

	private void HandleSaveYourselfEvent (object o, Gnome.SaveYourselfArgs args)
	{
		/* FIXME */
		string [] argv = { "muine" };

		client.SetRestartCommand (1, argv);
	}

	public static void Exit ()
	{
		conn.Close ();

		//Application.Quit ();
		Environment.Exit (0);
	}
}
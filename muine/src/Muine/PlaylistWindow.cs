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
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

using Gtk;
using Gtk.Ext;
using GLib;

public class PlaylistWindow : Window
{
	/* menu widgets */
	[Glade.Widget] private MenuItem songMenu;
	[Glade.Widget] private MenuItem playlistMenu;
	[Glade.Widget] private MenuItem fileMenu;


	/* playlist area */
	[Glade.Widget]
	private Label playlist_label;
	[Glade.Widget]
	private VBox playerBox;

	private PlaylistView playlistView;
	private Playlist playlist = AppContext.Playlist;

	/* other widgets */
	private Tooltips tooltips;

	/* windows */
	SkipToWindow skip_to_window = null;

	/* the playlist filename */
	private string playlist_filename;

	private PlayerView playerView;
	
	private Glade.XML glade_xml;

	public PlaylistWindow () : base (WindowType.Toplevel)
	{
		/* build the interface */
		glade_xml = new Glade.XML (null, "PlaylistWindow.glade", "main_vbox", null);
		glade_xml.Autoconnect (this);
			
		Add (glade_xml ["main_vbox"]);

		//AddAccelGroup (((Menu) glade_xml ["file_menu"]).AccelGroup);

		SetupWindowSize ();
		SetupButtonsAndMenuItems ();
		SetupPlaylist ();

		/* set up playlist filename */
		playlist_filename = Gnome.User.DirGet () + "/muine/playlist.m3u";

		playerView = new PlayerView (AppContext.PlayerBackend);
		playerBox.PackStart (playerView);
		//playerView.ShowAll ();
		playerBox.ShowAll ();

		glade_xml["main_vbox"].ShowAll ();

	}

	public void RestorePlaylist ()
	{
		/* load last playlist */
		System.IO.FileInfo finfo = new System.IO.FileInfo (playlist_filename);
		if (finfo.Exists)
			OpenPlaylist (playlist_filename);
	}

	public void Run ()
	{
		WindowVisible = true;
		/* put on the screen immediately */
		while (MainContext.Pending ())
			Main.Iteration ();
	}

	//{{{
	public void CheckFirstStartUp () 
 	{
		bool first_start;
 		try { 
 			first_start = (bool) AppContext.GConfClient.Get ("/apps/muine/first_start");
 		} catch {
 			first_start = true;
 		}

		if (first_start == false)
			return;

 		string dir = Environment.GetEnvironmentVariable ("HOME");
 		if (dir.EndsWith ("/") == false)
 			dir += "/";
 		
 		DirectoryInfo musicdir = new DirectoryInfo (dir + "Music/");
  
 		if (!musicdir.Exists) {
 			NoMusicFoundWindow w = new NoMusicFoundWindow (this);

	 		AppContext.GConfClient.Set ("/apps/muine/first_start", false);
 		} else { 
 			/* create a playlists directory if it still doesn't exists */
 			DirectoryInfo playlistsdir = new DirectoryInfo (dir + "Music/Playlists/");
 			if (!playlistsdir.Exists)
 				playlistsdir.Create ();

 			ProgressWindow pw = new ProgressWindow (this, musicdir.Name);

 			/* seems to be that $HOME/Music does exists, but user hasn't started Muine before! */
 			AppContext.DB.AddWatchedFolder (musicdir.FullName);

			/* do this here, because the folder is watched now */
	 		AppContext.GConfClient.Set ("/apps/muine/first_start", false);
	
 			HandleDirectory (musicdir, pw);

 			pw.Done ();
  		}
  	}//}}}
	
	//{{{
	private void SetupWindowSize ()
	{
		int width;
		try {
			width = (int) AppContext.GConfClient.Get ("/apps/muine/playlist_window/width");
		} catch {
			width = 500;
		}
		
		int height;
		try {
			height = (int) AppContext.GConfClient.Get ("/apps/muine/playlist_window/height");
		} catch {
			height = 400;
		}

		SetDefaultSize (width, height);

		SizeAllocated += new SizeAllocatedHandler (HandleSizeAllocated);
	}//}}}

	private int last_x = -1;
	private int last_y = -1;

	private bool window_visible;
	public bool WindowVisible {
		set {
			window_visible = value;

			if (window_visible) {
				if (Visible == false && last_x >= 0 && last_y >= 0)
					Move (last_x, last_y);

				Present ();
			} else {
				GetPosition (out last_x, out last_y);

				Visible = false;
			}

		}

		get {
			return window_visible;
		}
	}

	private void SetupButtonsAndMenuItems ()
	{
		
		Menu menu = new Menu ();
		
		/* Song Menu */
		menu.Append (new ActionMenuItem (GlobalActions.PlayPause));
		//Separator
		menu.Append (new SeparatorMenuItem ());
		menu.Append (new ActionMenuItem (GlobalActions.Previous));
		menu.Append (new ActionMenuItem (GlobalActions.Next));
		//Separator
		menu.Append (new SeparatorMenuItem ());
		menu.Append (new ActionMenuItem (GlobalActions.SkipTo));
		menu.Append (new ActionMenuItem (GlobalActions.SkipBackwards));
		menu.Append (new ActionMenuItem (GlobalActions.SkipForward));
		songMenu.Submenu = menu;

		/* Playlist Menu */
		menu = new Menu ();
		menu.Append (new ActionMenuItem (GlobalActions.AddSong));
		menu.Append (new ActionMenuItem (GlobalActions.AddAlbum));
		//Separator
		menu.Append (new SeparatorMenuItem ());
		menu.Append (new ActionMenuItem (GlobalActions.RemoveSong));
		menu.Append (new ActionMenuItem (GlobalActions.RemovePlayedSongs));
		menu.Append (new ActionMenuItem (GlobalActions.ClearPlaylist));
		//Separator
		menu.Append (new SeparatorMenuItem ());
		menu.Append (new ActionMenuItem (GlobalActions.Repeat));
		playlistMenu.Submenu = menu;

		/* File Submenu */
		menu = new Menu ();
		menu.Append (new ActionMenuItem (GlobalActions.ImportFolder));
		//Separator
		menu.Append (new SeparatorMenuItem ());
		menu.Append (new ActionMenuItem (GlobalActions.OpenPlaylist));
		menu.Append (new ActionMenuItem (GlobalActions.SavePlaylistAs));
		//Separator
		menu.Append (new SeparatorMenuItem ());
		menu.Append (new ActionMenuItem (GlobalActions.HideWindow));
		//Separator
		menu.Append (new SeparatorMenuItem ());
		menu.Append (new ActionMenuItem (GlobalActions.Quit));
		fileMenu.Submenu = menu;

	}

	private void SetupPlaylist ()
	{
		playlistView = new PlaylistView (new PlaylistStore ());

		playlistView.Show ();

		((Container) glade_xml ["scrolledwindow"]).Add (playlistView);
		
		MarkupUtils.LabelSetMarkup (playlist_label, 0, StringUtils.GetByteLength (AppContext.Catalog.GetString ("Playlist")),
		                            false, true, false);

	}

	// UpdateTimeLabels {{{
	private void UpdateTimeLabels (int time)
	{
		//FIXME: Move this to PlayerView
		/*if (!player.Playing) {
			time_label.Text = "";
			playlist_label.Text = AppContext.Catalog.GetString ("Playlist");
			return;
		}
		
		String pos = StringUtils.SecondsToString (time);
		String total = StringUtils.SecondsToString (AppContext.Playlist.Current.Duration);

		time_label.Text = pos + " / " + total;

		if (repeat_menu_item.Active) {
			long r_seconds = remaining_songs_time;

			if (r_seconds > 6000) { / 100 minutes
				int hours = (int) Math.Floor ((double) r_seconds / 3600.0 + 0.5);
				playlist_label.Text = String.Format (AppContext.Catalog.GetPluralString ("Playlist (Repeating {0} hour)", "Playlist (Repeating {0} hours)", hours), hours);
			} else if (r_seconds > 60) {
				int minutes = (int) Math.Floor ((double) r_seconds / 60.0 + 0.5);
				playlist_label.Text = String.Format (AppContext.Catalog.GetPluralString ("Playlist (Repeating {0} minute)", "Playlist (Repeating {0} minutes)", minutes), minutes);
			} else if (r_seconds > 0) {
				playlist_label.Text = AppContext.Catalog.GetString ("Playlist (Repeating)");
			} else {
				playlist_label.Text = AppContext.Catalog.GetString ("Playlist");
			}
		} else {
			long r_seconds = remaining_songs_time + song.Duration - time;
			
			if (r_seconds > 6000) { // 100 minutes 
				int hours = (int) Math.Floor ((double) r_seconds / 3600.0 + 0.5);
				playlist_label.Text = String.Format (AppContext.Catalog.GetPluralString ("Playlist ({0} hour remaining)", "Playlist ({0} hours remaining)", hours), hours);
			} else if (r_seconds > 60) {
				int minutes = (int) Math.Floor ((double) r_seconds / 60.0 + 0.5);
				playlist_label.Text = String.Format (AppContext.Catalog.GetPluralString ("Playlist ({0} minute remaining)", "Playlist ({0} minutes remaining)", minutes), minutes);
			} else if (r_seconds > 0) {
				playlist_label.Text = AppContext.Catalog.GetString ("Playlist (Less than one minute remaining)");
			} else {
				playlist_label.Text = AppContext.Catalog.GetString ("Playlist");
			}
		} */
	}///}}}

	//OpenPlaylist {{{
	public void OpenPlaylist (string fn)
	{
		//FIXME:
		//StreamReader reader;
		//
		//try {
		//	reader = new StreamReader (fn);
		//} catch {
		//	new ErrorDialog (String.Format (AppContext.Catalog.GetString ("Failed to open {0} for reading"), fn), this);
		//	return;
		//}

		//string line = null;

		//bool playing_song = false;

		//while ((line = reader.ReadLine ()) != null) {
		//	if (line.Length == 0)
		//		continue;

		//	if (line[0] == '#') {
		//		if (line == "# PLAYING")
		//			playing_song = true;

		//		continue;
		//	}

		//	/* DOS-to-UNIX */
		//	line.Replace ('\\', '/');

		//	string basename = "";

		//	try {
		//		basename = System.IO.Path.GetFileName (line);
		//	} catch {
		//		continue;
		//	}

		//	Song song = (Song) AppContext.DB.Songs [line];
		//	if (song == null) {
		//		/* not found, lets see if we can find it anyway.. */
		//		foreach (string key in AppContext.DB.Songs.Keys) {
		//			string key_basename = System.IO.Path.GetFileName (key);

		//			if (basename == key_basename) {
		//				song = (Song) AppContext.DB.Songs [key];
		//				break;
		//			}
		//		}
		//	}

		//	if (song == null) {
		//		try {
		//			song = new Song (line);
		//		} catch {
		//			song = null;
		//		}

		//		if (song != null)
		//			song.Orphan = true;
		//	}

		//	if (song != null) {
		//		AddSong (song);

		//		if (playing_song) {
		//			AppContext.Playlist.Current = song;
		//			//FIXME
		//			//playlistView.Select (song.Handle);

		//			SongChanged (true);

		//			playing_song = false;
		//		}
		//	}
		//}

		//reader.Close ();

		//EnsurePlaying ();

		//NSongsChanged ();
	}//}}}

	//SavePlaylist {{{
	private void SavePlaylist (string fn, bool exclude_played, bool store_playing)
	{
		StreamWriter writer;
		
		try {
			writer = new StreamWriter (fn, false);
		} catch {
			new ErrorDialog (String.Format (AppContext.Catalog.GetString ("Failed to open {0} for writing"), fn), this);
			return;
		}

		bool had_playing_song = false;
		foreach (Song song in AppContext.Playlist) {

			if (exclude_played) {
				if (song == AppContext.Playlist.Current) {
					had_playing_song = true;

					//if (had_last_eos)
					//	continue;
				}

				if (!had_playing_song)
					continue;
			}
			
			if (store_playing) {
				if (song == AppContext.Playlist.Current)
					writer.WriteLine ("# PLAYING");
			}
			
			writer.WriteLine (song.Filename);
		}

		writer.Close ();
	}//}}}

	private void HandleWindowStateEvent (object o, WindowStateEventArgs args)
	{
		if (!Visible)
			return;
			
		bool old_window_visible = window_visible;
		window_visible = ((args.Event.NewWindowState != Gdk.WindowState.Iconified) &&
				  (args.Event.NewWindowState != Gdk.WindowState.Withdrawn));

	}

	private void HandleWindowVisibilityNotifyEvent (object o, VisibilityNotifyEventArgs args)
	{
		if (!Visible ||
		    GdkWindow.State == Gdk.WindowState.Iconified ||
		    GdkWindow.State == Gdk.WindowState.Withdrawn)
		    return;

		bool old_window_visible = window_visible;
		window_visible = (args.Event.State != Gdk.VisibilityState.FullyObscured);

		args.RetVal = false;
	}

	private void HandleDeleteEvent (object o, DeleteEventArgs args)
	{
		//AppContext.Exit ();
	}

	private void HandleSizeAllocated (object o, SizeAllocatedArgs args)
	{
		int width, height;

		GetSize (out width, out height);

		AppContext.GConfClient.Set ("/apps/muine/playlist_window/width", width);
		AppContext.GConfClient.Set ("/apps/muine/playlist_window/height", height);
	}

	private void HandleToggleWindowVisibilityCommand (object o, EventArgs args)
	{
		WindowVisible = !WindowVisible;
	}

	//HandleDirectory {{{
	private bool HandleDirectory (DirectoryInfo info,
				      ProgressWindow pw)
	{
	    	//System.IO.FileInfo [] finfos;
		//
		//try {
		//	finfos = info.GetFiles ();
		//} catch {
		//	return true;
		//}
		//
		//foreach (System.IO.FileInfo finfo in finfos) {
		//	Song song;

		//	song = (Song) AppContext.DB.Songs [finfo.FullName];
		//	if (song == null) {
		//		bool ret = pw.ReportFile (finfo.Name);
		//		if (ret == false)
		//			return false;

		//		try {
		//			song = new Song (finfo.FullName);
		//		} catch {
		//			continue;
		//		}

		//		AppContext.DB.AddSong (song);
		//	}
		//}

		//DirectoryInfo [] dinfos;
		//
		//try {
		//	dinfos = info.GetDirectories ();
		//} catch {
		//	return true;
		//}

		//foreach (DirectoryInfo dinfo in dinfos) {
		//	bool ret = HandleDirectory (dinfo, pw);
		//	if (ret == false)
		//		return false;
		//}

		return true;
	}//}}}

	//HandleImportFolderCommand {{{
	private void HandleImportFolderCommand (object o, EventArgs args) 
	{
		//FileSelection fs;
		//
		//fs = new FileSelection (AppContext.Catalog.GetString ("Import Folder"));
		//fs.HideFileopButtons ();
		//fs.HistoryPulldown.Visible = false;
		//fs.FileList.Parent.Visible = false;
		//fs.SetDefaultSize (350, 250);

		//string start_dir;
		//try {
		//	start_dir = (string) AppContext.GConfClient.Get ("/apps/muine/default_import_folder");
		//} catch {
		//	start_dir = "~";
		//}

		//start_dir.Replace ("~", Environment.GetEnvironmentVariable ("HOME"));

		//if (start_dir.EndsWith ("/") == false)
		//	start_dir += "/";

		//fs.Filename = start_dir;

		//if (fs.Run () != (int) ResponseType.Ok) {
		//	fs.Destroy ();

		//	return;
		//}

		//fs.Visible = false;

		//AppContext.GConfClient.Set ("/apps/muine/default_import_folder", fs.Filename);

		//DirectoryInfo dinfo = new DirectoryInfo (fs.Filename);
		//	
		//if (dinfo.Exists) {
		//	ProgressWindow pw = new ProgressWindow (this, dinfo.Name);

		//	AppContext.DB.AddWatchedFolder (dinfo.FullName);
		//	HandleDirectory (dinfo, pw);

		//	pw.Done ();
		//}

		//fs.Destroy ();
	}//}}}

	// HandleOpenPlaylistCommand {{{
	private void HandleOpenPlaylistCommand (object o, EventArgs args)
	{
		//FileSelector sel = new FileSelector (AppContext.Catalog.GetString ("Open Playlist"),
		//				     "/apps/muine/default_playlist_folder");

		//string fn = sel.GetFile ();

		//if (fn.Length == 0 || !FileUtils.IsPlaylist (fn))
		//	return;

		//System.IO.FileInfo finfo = new System.IO.FileInfo (fn);

		//if (finfo.Exists)
		//	OpenPlaylist (fn);
	}//}}}

	//HandleSavePlaylistAsCommand {{{
	private void HandleSavePlaylistAsCommand (object o, EventArgs args)
	{
		FileSelector sel = new FileSelector (AppContext.Catalog.GetString ("Save Playlist"),
						     "/apps/muine/default_playlist_folder");

		string fn = sel.GetFile ();

		if (fn.Length == 0)
			return;

		/* make sure the extension is ".m3u" */
		if (!FileUtils.IsPlaylist (fn))
			fn += ".m3u";

		System.IO.FileInfo finfo = new System.IO.FileInfo (fn);

		if (finfo.Exists) {
			YesNoDialog d = new YesNoDialog (String.Format (AppContext.Catalog.GetString ("File {0} will be overwritten.\nIf you choose yes, the contents will be lost.\n\nDo you want to continue?"), fn), this);
			if (d.GetAnswer () == true)
				SavePlaylist (fn, false, false);
		} else
			SavePlaylist (fn, false, false);
	}//}}}

	private void HandleRepeatCommand (object o, EventArgs args)
	{
		//if (setting_repeat_menu_item)
		//	return;

		//AppContext.GConfClient.Set ("/apps/muine/repeat", repeat_menu_item.Active);
	}

	private void HandleHideWindowCommand (object o, EventArgs args)
	{
		WindowVisible = false;
	}

	private void HandlePlaylistRowActivated (IntPtr handle)
	{
	}

	private void HandlePlaylistSelectionChanged ()
	{
	}

	private void HandleQuitCommand (object o, EventArgs args)
	{
		//AppContext.Exit ();
	}

	private void HandleAboutCommand (object o, EventArgs args)
	{
		About.ShowWindow (this);
	}

	public void AddChildWindowIfVisible (Window window)
	{
		if (WindowVisible)
			window.TransientFor = this;
		else
			window.TransientFor = null;
	}

	private void HandlePlayPauseCommand(object obj, EventArgs args)
	{
	}

	private void HandlePreviousCommand (object obj, EventArgs args)
	{
	}
	private void HandleNextCommand (object obj, EventArgs args)
	{
	}
	private void HandleSkipToCommand (object obj, EventArgs args)
	{
	}
	private void HandleSkipBackwardsCommand (object obj, EventArgs args)
	{
	}
	private void HandleSkipForwardCommand (object obj, EventArgs args)
	{
	}
	private void HandleInformationCommand (object obj, EventArgs args)
	{
	}
	private void HandleAddSongCommand (object obj, EventArgs args)
	{
	}
	private void HandleAddAlbumCommand (object obj, EventArgs args)
	{
	}
	private void HandleRemoveSongCommand (object obj, EventArgs args)
	{
	}
	private void HandleRemovePlayedSongsCommand (object obj, EventArgs args)
	{
	}
	private void HandleClearPlaylistCommand (object obj, EventArgs args)
	{
	}

}

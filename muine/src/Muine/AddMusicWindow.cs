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

using Gtk;
using GLib;

public class AddMusicWindow : Window
{
	[Glade.Widget]
	Window window;
	[Glade.Widget]
	Entry search_entry;
	[Glade.Widget]
	Button play_button;
	[Glade.Widget]
	Image play_button_image;
	[Glade.Widget]
	Button queue_button;
	[Glade.Widget]
	Image queue_button_image;
	[Glade.Widget]
	ScrolledWindow scrolledwindow;
	DataView dataView;

	public AddMusicWindow (DataView dataView) : base (IntPtr.Zero)
	{
		this.dataView = dataView;
		Glade.XML gxml = new Glade.XML (null, "AddWindow.glade", "window", null);
		gxml.Autoconnect (this);

		Raw = window.Handle;

		window.Title = dataView.Title;

		int width;
			width = 350;

		int height;
			height = 300;

		window.SetDefaultSize (width, height);

		play_button_image.SetFromStock ("muine-play", IconSize.Button);
		queue_button_image.SetFromStock ("muine-queue", IconSize.Button);

		scrolledwindow.Add (dataView);

		dataView.Realize ();
		dataView.Show ();
		dataView.Search (String.Empty);

	}

	public void Run ()
	{
		search_entry.GrabFocus ();

		//FIXME
		//view.SelectFirst ();

		window.Present ();
	}

	private void HandleWindowResponse (object o, EventArgs a)
	{
		ResponseArgs args = (ResponseArgs) a;

		switch ((int) args.ResponseId) {
		//Fill the playlist
		case 1: /* Play */
			window.Visible = false;
			//FIXME: Add songs to the playlist
			//if (PlaySongsEvent != null)
			//	PlaySongsEvent (view.SelectedPointers);
			Reset ();
			break;
		case 2: /* Queue */
			//FIXME: Queue songs to the playlist
			//if (QueueSongsEvent != null)
			//	QueueSongsEvent (view.SelectedPointers);
			search_entry.GrabFocus ();
			break;
		default:
			window.Visible = false;
			Reset ();
			break;
		}
	}

	private void HandleWindowDeleteEvent (object o, EventArgs a)
	{
		window.Visible = false;
		DeleteEventArgs args = (DeleteEventArgs) a;
		args.RetVal = true;
		Reset ();
	}


	private uint search_idle_id = 0;

	private void HandleSearchEntryChanged (object o, EventArgs args)
	{
			dataView.Search (search_entry.Text);
	}

	private void HandleSearchEntryKeyPressEvent (object o, EventArgs a)
	{
		/*KeyPressEventArgs args = (KeyPressEventArgs) a;

		args.RetVal = view.ForwardKeyPress (search_entry, args.Event);*/
	}

	private void Reset ()
	{
		search_entry.Text = "";
	}

	public void QueueClicked (object obj, EventArgs args)
	{
	}
	public void CloseClicked (object obj, EventArgs args)
	{
	}
	public void PlayClicked (object obj, EventArgs args)
	{
	}

}

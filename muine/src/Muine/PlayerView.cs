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

using Gtk;
using Glade;
using System;
using Gtk.Ext;

public class PlayerView : VBox
{
	private Player player;
	private VolumeButton volumeButton;
	private XML glade_xml;
	private string glade_file = "Player.glade";
	private CoverImage cover_image;

	private EllipsizingLabel title_label;
	private EllipsizingLabel artist_label;

	[Glade.Widget] private VBox volumeBtnBox;
	[Glade.Widget] private HBox  nextBox;
	[Glade.Widget] private HBox  prevBox;
	[Glade.Widget] private HBox playPauseBox;
	[Glade.Widget] private HBox coverBox;
	[Glade.Widget] private HBox playAlbumBox;
	[Glade.Widget] private HBox playSongBox;
	[Glade.Widget] private VBox titleBox;
	[Glade.Widget] private VBox artistBox;


    
    public PlayerView (Player player) : base ()
    {
	this.player = player;
	InitComponent ();
    }
    
    private void InitComponent ()
    {
	glade_xml = new XML (null, glade_file, "mainBox", null);
	glade_xml.Autoconnect (this);
	Add (glade_xml["mainBox"]);

	/* Event Handling */
	player.EndOfStreamEvent += HandleEndOfStreamEvent;
	player.TickEvent += HandleTickEvent;
	player.StateChanged += HandleStateChanged;

	title_label = new EllipsizingLabel ("");
	title_label.Visible = true;
	title_label.Xalign = 0.0f;
	title_label.Selectable = true;
	titleBox.Add (title_label);

	artist_label = new EllipsizingLabel ("");
	artist_label.Visible = true;
	artist_label.Xalign = 0.0f;
	artist_label.Selectable = true;
	artistBox.Add (artist_label);

	cover_image = new CoverImage ();
	coverBox.Add (cover_image);

	volumeButton = new VolumeButton ();
	volumeButton.Visible = true;
	volumeButton.VolumeChanged += HandleVolumeChanged;
	volumeBtnBox.Add (volumeButton);

	ImageButton imgBtn;

	imgBtn = new ImageButton (GlobalActions.PlayPause);
	playPauseBox.PackStart (imgBtn);
	imgBtn.TextEnabled = false;
	imgBtn.Label.Text = "";
	
	imgBtn = new ImageButton (GlobalActions.Previous);
	prevBox.Add (imgBtn);
	imgBtn.TextEnabled = false;
	imgBtn.Label.Text = "";

	imgBtn = new ImageButton (GlobalActions.Next);
	nextBox.Add (imgBtn);
	imgBtn.TextEnabled = false;
	imgBtn.Label.Text = "";

	imgBtn = new ImageButton (GlobalActions.AddSong);
	playSongBox.Add (imgBtn);
	imgBtn.TextEnabled = true;
	imgBtn.Label.Text = AppContext.Catalog.GetString ("Play Song");

	imgBtn = new ImageButton (GlobalActions.AddAlbum);
	playAlbumBox.Add (imgBtn);
	imgBtn.TextEnabled = true;
	imgBtn.Label.Text = AppContext.Catalog.GetString ("Play Album");

    }
	private void HandleVolumeChanged (int vol)
        {
                player.Volume = vol;
                AppContext.GConfClient.Set ("/apps/muine/volume", vol);
        }

	private void HandleTickEvent (int pos)
	{
	}

	private void HandleStateChanged (bool playing)
	{
	}

	private void HandleEndOfStreamEvent ()
	{
	}

}

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

namespace WebNotes 
{
    using System; 
    using System.IO;
    using Gecko;
    using Gtk;
    using Egg;
    using Glade;
    using System.Collections;

    public class WebViewer : Window
    {
	
	private TrayIcon trayIcon;
	private WebControl wc;
	private XML xml;
	private ArrayList list = new ArrayList ();
	private Menu currentMenu;
	[Glade.Widget] private VBox mainBox;
	[Glade.Widget] private Frame geckoBox;
	[Glade.Widget] private OptionMenu optionMenu;
	

	public WebViewer () : base (WindowType.Toplevel)
	{
	    xml = new XML (null, "MainWindow.glade", "mainBox", null);
	    xml.Autoconnect (this);
	    Gdk.Pixbuf pix = new Gdk.Pixbuf (null, "webnotes-16.png");
	    
	    //Window settings
	    Title = "WebNotes";
	    WindowPosition = WindowPosition.Center;
	    Icon = pix;
	    Resize (650,600);
	    
	    //Trayicon stuff
	    trayIcon = new TrayIcon ("WebNotes");
	    EventBox ebox = new EventBox ();
	    ebox.ButtonPressEvent += ButtonPressed;
	    Image image = new Image (pix);
	    ebox.Add (image);
	    trayIcon.Add (ebox);
	    trayIcon.ShowAll ();

	    //Gecko webcontrol
	    wc = new WebControl ();
	    wc.LoadUrl ("http://localhost:8000");
	    geckoBox.Add (wc); 
	    
	    optionMenu.Changed += OptionChanged;
	    BuildMenu ();
	    optionMenu.Menu = currentMenu;
	    int firstPage = list.IndexOf ("WikiHome");
	    if (firstPage != -1)
		optionMenu.SetHistory ((uint)firstPage);
	    
	    Add (mainBox);
	}

	private void BuildMenu ()
	{
	    list.Clear ();
	    if (optionMenu.Menu != null)
		optionMenu.Menu.Destroy ();
	    Menu menu = new Menu ();
	    string didiwikidir = Environment.GetEnvironmentVariable ("HOME");
	    didiwikidir += System.IO.Path.DirectorySeparatorChar + ".didiwiki";
	    string[] files = Directory.GetFiles (didiwikidir);
	    foreach (string file in files)
	    {
		if (!file.EndsWith (".css"))
		{
		    string fname = System.IO.Path.GetFileName (file);
		    MenuItem item = new MenuItem (fname);
		    item.Activated += ItemActivated;
		    menu.Append (item);
		    list.Add (fname);
		}
	    }
	    menu.Append (new SeparatorMenuItem ());
	    ImageMenuItem closeItem = new ImageMenuItem ("_Quit");
	    closeItem.Image = new Image (Gtk.Stock.Quit, Gtk.IconSize.Menu);
	    closeItem.Activated += Quit;
	    menu.Append (closeItem);
	    optionMenu.Menu = menu;
	    menu.ShowAll ();
	    currentMenu = menu;
	}

	private void ButtonPressed (object obj, ButtonPressEventArgs args)
	{
	    switch (args.Event.Button)
	    {
		case 1:
		    if (args.Event.Type == Gdk.EventType.TwoButtonPress)
			Present ();
		    break;
		case 3:
		    currentMenu.Popup (null, null, null, IntPtr.Zero, 3, Gtk.Global.CurrentEventTime);
		    break;
	    }
	}

	protected override bool OnKeyPressEvent (Gdk.EventKey evt)
	{
	    if (evt.Key == Gdk.Key.Escape)
		Visible = false;
	    else if (evt.State == Gdk.ModifierType.ControlMask
			&& evt.Key == Gdk.Key.r)
		BuildMenu ();
	    return base.OnKeyPressEvent (evt);
	}

	protected override bool OnDeleteEvent (Gdk.Event evt)
	{
	    /*bool retVal = base.OnDeleteEvent (evt);
	    WebNotes.Exit ();
	    return retVal;
		Hide ();
		return base.OnDeleteEvent (evt);
		*/
		Hide ();
		return true;
	}

	private void OptionChanged (object obj, EventArgs handler)
	{
		if (optionMenu.History < list.Count)
	    		wc.LoadUrl ("http://localhost:8000/" + list[optionMenu.History]);
	}

	private void HandleRefresh (object obj, EventArgs args)
	{
	    BuildMenu ();
	}

	private void ItemActivated (object obj, EventArgs args)
	{
	          Present ();
	}
	private void Quit (object sender, EventArgs args)
	{
	    WebNotes.Exit ();
	}
    }
}


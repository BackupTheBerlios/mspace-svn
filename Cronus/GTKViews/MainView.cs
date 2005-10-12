/* Monotimer
 * Copyright (C) 2003 GNOME Foundation
 *
 * AUTHORS:
 *      Luis Bosque <luis.bosque@hispalinux.es>
 *
 * This Library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Library General Public License as
 * published by the Free Software Foundation; either version 2 of the
 * License, or (at your option) any later version.
 *
 * This Library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Library General Public License for more details.
 *
 * You should have received a copy of the GNU Library General Public
 * License along with this Library; see the file COPYING.LIB.  If not,
 * write to the Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */

using Gtk;
using GLib;
using Glade;
using System;
using libmonotimer;
using Control;
using System.Threading;

namespace Views{

public class MainView{
	
	[Glade.Widget] private Gtk.Window main;
	[Glade.Widget] private Gtk.Button btnOK;
	[Glade.Widget] private Gtk.Button btnLoad;
	[Glade.Widget] private Gtk.Label label1;
	[Glade.Widget] private Gtk.TextView lista_proy;
	[Glade.Widget] private Gtk.Entry id_proy;
	[Glade.Widget] private Gtk.Entry name_proy;
	Gtk.TextBuffer buffer;
	
	Controller app;
	int counter;
	
	public void deleteEventCb (object o, DeleteEventArgs args) {
		Application.Quit ();
	}

	public void OKButtonCb (object o, EventArgs args) {
		print("ok");
		label1.Text = "OK";
		//app.incCounter();
		//System.Threading.Thread counter = new System.Threading.Thread ( new ThreadStart (app.StartTimer) );
		//counter.Start ();
		
		//app.StartTimer ();
	}
		
	public void LoadButtonCb (object o, EventArgs args) {
		print("Project Loaded");
	}
		
	public MainView(Controller app){
		this.app = app;
		Glade.XML gxml;
		gxml = new Glade.XML("./data/main.glade","main",null);
		gxml.Autoconnect(this);
		System.Console.WriteLine("Vista creada");
		counter = 0;
		buffer = lista_proy.Buffer;
		
		if (main == null){
			print("Ventana principal [NULL]");
		}
		if (btnOK == null){
			print("btnOK [NULL]");
		}
		
		
		app.timer += new EventHandler (OnTimer);
		
		
		main.DeleteEvent += new DeleteEventHandler (deleteEventCb);
		btnOK.Clicked += new EventHandler (OKButtonCb);
		btnLoad.Clicked += new EventHandler (LoadButtonCb);
	}

	public void print (string texto){
		Console.WriteLine(texto);
	}

	public void OnTimer (object sender, EventArgs args) {
		counter++;
		label1.Text = counter.ToString();;
	}


}




}

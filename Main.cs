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

using System;
using Views;
using Gtk;
using libmonotimer;
using Control;

public class Mainclass{
	
	Views.MainView view;

	
	public Mainclass(){

		Controller app = new Controller();
		
		Application.Init();
		this.view = new MainView(app);
		Application.Run();
	}


	public static void Main(){
		new Mainclass();
	}

}

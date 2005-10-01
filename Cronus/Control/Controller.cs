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
using libmonotimer;
using System.Threading;

namespace Control{

	
	public struct CollabProject {
		public int id;
		public string nombre;
	}
	
	public sealed class Controller{
		
		CollabProject[] Array_Projects;
		public event EventHandler timer;
		
		public Controller(string login, string password) {
			
			Array_Projects = Communication.login (login, password);
			if (Array_Projects.Length == 0)
				Console.WriteLine ("Autenticacion fallida");
		}	
	}
	
}



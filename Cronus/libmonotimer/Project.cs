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
using System.Threading;

namespace libmonotimer
{

	public class Crono {
		public int counter = 0;
		public Timer timer;
	}
	
	public sealed class Project{
	
		private static Project instance;
		private string name;
		private int id;
		[NonSerialized()]
		private Timer timer;
		private int counter;
		private static int contador = 0;
		
		private Project(){
			contador ++;
			counter = 0;
			id = contador;
			Crono crono = new Crono ();
			TimerCallback timerDelegate = new TimerCallback (Tick);
			timer = new Timer (timerDelegate, crono, 0, 1000);
   			/*
			crono.timer = timer;

			while (crono.timer != null)
				Thread.Sleep(0);
			Console.WriteLine("Final del contador");
			*/
		}
		
		public static Project Instance {
			get {
				if ( instance == null ) {
					instance = new Project ();
				}
				return instance;
			}
		}
		
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}
		public int Id {
			get {
				return id;
			}
		}
		
		
		private static void Tick (Object state) {
			Crono crono = (Crono) state;
			crono.counter++;
			Console.WriteLine(crono.counter);
		}

	}
}

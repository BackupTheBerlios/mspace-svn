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
using Mono.Unix;

namespace Control{

	
//class TimerExampleState {
//	int counter = 0;
//	public Timer tmr;
//}
public struct CollabProject {
	public int id;
	public string nombre;
}

public class Controller{
	
	string login = "";
	string password = "";
	
	proyecto[] Array_Projects;
	Project[] lista_proyectos;
	
	private Project currentProject;
	public event EventHandler timer;
	
	public Controller(){
		
		Projects = Communication.login (login, password);
		if (Projects.Length == 0) {
			Console.WriteLine ("Autenticacion fallida");
		}
		else {
			//lista_proyectos = new Proyect [Projects.Length];
			foreach ( proyecto aux in Projects ) {
				
			}			
		}


		
		
		
		
		
		//this.counter = 0;

		/*TimerExampleState s = new TimerExampleState();
		// Create the delegate that invokes methods for the timer.
		TimerCallback timerDelegate = new TimerCallback(CheckStatus);
		// Create a timer that waits one second, then invokes every second.
		Timer timer = new Timer(timerDelegate, s,0, 1000);
   
		// Keep a handle to the timer, so it can be disposed.
		s.tmr = timer;
		// The main thread does nothing until the timer is disposed.
		while (s.tmr != null)
		Thread.Sleep(0);
		Console.WriteLine("Timer example done.");
		*/
	}
	/*
	public string CurrentProject{
		get{
			return currentProject;
		}
		set{
			currentProject = value;
		}
	}
	*/
	
	public void incCounter(){
		//this.counter++;
		//this.label.Text = this.counter.ToString();;
	}

	public void StartTimer (){
		for (int i = 0; i < 10; i++) {
			Syscall.sleep (1);
			Console.WriteLine (i);
			timer (this, EventArgs.Empty);
		}
		
	}

	public void stop(){
	}
	
	/*static void CheckStatus(Object state) {
		TimerExampleState s = (TimerExampleState) state;
		s.counter++;
		Console.WriteLine(s.counter);

	}*/
	
	
}

}



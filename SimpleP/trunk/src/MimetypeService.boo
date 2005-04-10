/*
	Copyright (c) 2005 Sergio Rubio, <sergio.rubio@hispalinux.es>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace SimpleP

import System
import System.IO
import System.Diagnostics
import System.Collections
import System.Threading

class MimetypeService:

	mimeTypesActions = {
						'.glade': ['glade-2']
						}

	_serverProcess as Process

	def OpenFile (file as string) as bool:
		extension = Path.GetExtension (file)
		if mimeTypesActions.Contains (extension):
			l as Boo.Lang.List = mimeTypesActions[extension]
			cmd = l[0]
			args = l[1:]
			args.Add (file)
			argsString = args.Join (' ')
			print argsString
			info = ProcessStartInfo (cmd, argsString)
			info.UseShellExecute = false
			try:
				process = Process.Start (info)
			except:
				print "Can't start the process: ${cmd}"
		else:
			ThreadPool.QueueUserWorkItem (OpenInVim, file)

		return true

	def OpenInVim (state):
		info2 = ProcessStartInfo ("gvim", "--servername simplep --remote-send ':e ${state}<CR>'")
		info2.UseShellExecute = false
		try:
			if not _serverProcess:
				StartServer ()
				
			exitCode = 1
			iteration = 0
			while exitCode != 0:
				Thread.Sleep (100)
				process = Process.Start (info2)
				exitCode = process.ExitCode
				if iteration >= 10:
					StartServer ()
					iteration = 0
					continue
				iteration += 1
		except ex as Exception:
			print "Can't start the process: " + ex.Message
	
	def StartServer ():
		info = ProcessStartInfo ("gvim", "-geometry 85x30+320+30 --servername simplep")
		info.UseShellExecute = false 
		info.RedirectStandardInput = true
		_serverProcess = Process.Start (info)
		//WaitForServer ()
	
	def WaitForServer ():
		info = ProcessStartInfo ("gvim", "--serverlist")
		info.UseShellExecute = false
		info.RedirectStandardOutput = true
		process = Process.Start (info)
		line as string
		while (line = process.StandardOutput.ReadLine ()) != "SIMPLEP":
			process.Start ()
			if line == "SIMPLEP":
				print "FOUND"
			Thread.Sleep (10)
		
	def Exited (args, sender):
		_serverStarted = false

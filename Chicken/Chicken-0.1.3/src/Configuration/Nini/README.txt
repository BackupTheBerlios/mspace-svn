---------------------------------------------------
Nini - Configuration Library for the .NET Framework
---------------------------------------------------

Homepage: http://nini.sourceforge.net/
Author:   Brent R. Matzelle


ABOUT
-----
Nini is a software configuration library written in 100% C# for the .NET 
Framework. This software contains a powerful API for abstracting the access 
of multiple configuration types: INI, .NET config files, Windows(tm) Registry, 
XML files, and command-line options. Nini also includes an INI parser library. 
It compiles with Mono (http://www.go-mono.org/) as well.  Nini is absolutely 
free to use in any projects, including commericial ones.  To learn more about 
the license read the LICENSE.txt file.


INSTALL
-------
* Take the DLL for your .NET Framework version out of the Bin directory 

  * Bin\net\1.0 - Library built with the MS .NET Framework 1.0

  * Bin\net\1.1 - Library built with the MS .NET Framework 1.0

  * Bin\mono\1.0 - Library built with Mono 1.0
  
* Add the DLL as a reference in your project.  In Visual Studio right-click on 
  the References item in the project menu, click on the Browse button and 
  select Nini.dll.
  
* You can also add Nini to all projects on your machine by adding it to the
  global assembly cache.  To do this run the following command:
  $ gacutil.exe /i Nini.dll

* To check if your install was successful add the following to a .NET project 
  file (this assumes C#, use the appropriate version for C++/VB.NET, etc):

  using Nini.Config;

  If it compiles without any errors then you've succeeded.


BUILDING
--------
There are several methods to build Nini.

* NAnt (http://nant.sourceforge.net)
  In the Source directory there is a Cyrus.build NAnt file.  To build .NET 
  (Add some notes here to split up the release/debug/etc with NAnt

* Visual Studio .NET
  At this time only a Visual Studio .NET 2003 solution file and project 
  file are supplied.  They are located in the Source directory (Nini.sln).
  
* Note: If you would like to run the unit test then download and install 
  NUnit (http://nunit.org/).


DOCUMENTATION
-------------
* You can find all documentation in the Docs directory.  Here is a description
  of all directories beneath this directory:

  * Docs\Manual - Contains the Nini manual files.  If you'd like to quickly get 
                  started with Nini then read this first.

  * Docs\Reference\chm - Contains the compiled reference documentation.
  
  * Docs\Reference\html - Contains the individual HTML-based reference.
  
  * Docs\Reference\xml - Contains the XML source for the reference 
                         documentation.  If you'd like to add more documentation
						 to the project then start here.
  

QUESTIONS, HELP, & SUGGESTIONS
------------------------------
Go to the following places for help using Nini or if you'd like to request 
new features for Nini:

* Help Forum
  http://sourceforge.net/forum/forum.php?forum_id=379750

* Mailing List
  http://lists.sourceforge.net/lists/listinfo/nini-general


Thank you for trying Nini!

-------------------------------------
Copyright (c) 2004 Brent R. Matzelle
-------------------------------------

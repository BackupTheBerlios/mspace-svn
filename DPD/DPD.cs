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

using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

public class DPD
{
    private Hashtable md5List = new Hashtable ();
    private string baseDir;
    private bool recursive;
    public static string RED = (char)27 + "[31;1m";
    public static string BLUE = (char)27 + "[34;1m";
    public static string GREEN = (char)27 + "[32;1m";
    public static string RESET = (char)27 + "[0m";
    
    public DPD ()
    {
    }

    public void LoadAndCompare (string f1, string f2)
    {
	StreamReader reader1 = new StreamReader (f1);
	StreamReader reader2 = new StreamReader (f2);
	ArrayList list1, list2;
	list1 = new ArrayList ();
	list2 = new ArrayList ();
	bool duplicated = false;
	string hash1, hash2, file1, file2;
	hash1 = "";
	hash2 = "";
	file1= "";
	file2 = "";
	char[] separators = {' '};
	
	string s;
	while ((s = reader1.ReadLine ()) != null)
	{
	    list1.Add (s);
	}
	while ((s = reader2.ReadLine ()) != null)
	{
	    list2.Add (s);
	}
	reader1.Close ();
	reader2.Close ();

	foreach (string line1 in list1)
	{
	    hash1 = line1.Split (separators, 2)[0].Trim ();
	    file1 = line1.Split (separators, 2)[1].Trim ();
	    foreach (string line2 in list2)
	    {
		hash2 = line2.Split (separators, 2)[0].Trim ();
		file2 = line2.Split (separators, 2)[1].Trim ();
		if (hash1 == hash2)
		{
		    //The movie exists
		    duplicated = true;
		    break;
		}
	    }
	    if (!duplicated)
	    {
		md5List.Add (hash1, file1);
	    }
	    duplicated = false;
	}
	//PrintDiferences (md5List);
	//FIXME
    }

    public void PrintDiferences (Hashtable collection)
    {
	foreach (DictionaryEntry file in collection)
	    Console.WriteLine ("File: {0} {1}", file.Key, file.Value);
    }

    public void CopyFiles (string location)
    {
	foreach (DictionaryEntry file in md5List)
	{
	    try {
		string[] tokens = ((string)file.Value).Trim ().Split ('/');
		string fname = ((string) file.Value).Trim ();
		string fnameTokenized = tokens[tokens.Length -1];
		Console.WriteLine (RED + "Copying file.. {0} {1} to {2} {3}",
				    GREEN , file.Value, location, BLUE, fnameTokenized);
		File.Copy (fname,
			    location.Trim () + Path.DirectorySeparatorChar + fnameTokenized,
			    false);
	    } catch (FileNotFoundException e) {
		Console.WriteLine (e.Message);
		Console.WriteLine ("ERROR: File not found: {0}", file.Value);
	    }
	}
	Console.WriteLine (RESET);
    }

    public static void Main (string[] args)
    {
	if (args.Length == 3)
	{
	    //Comparing lists
	    DPD dpd = new DPD ();
	    dpd.LoadAndCompare (args[0], args[1]);
	    dpd.CopyFiles (args[2]);
	} else {
	    Console.WriteLine (args.Length);
	    Console.WriteLine ("Usage: dpd <source-hash> <dest-hash> <dest dir>");
	}

    }
}


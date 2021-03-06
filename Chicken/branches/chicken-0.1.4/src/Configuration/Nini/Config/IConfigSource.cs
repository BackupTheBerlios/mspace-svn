#region Copyright
//
// Nini Configuration Project.
// Copyright (C) 2004 Brent R. Matzelle.  All rights reserved.
//
// This software is published under the terms of the MIT X11 license, a copy of 
// which has been included with this distribution in the LICENSE.txt file.
// 
#endregion

using System;

namespace Nini.Config
{
	/// <include file='IConfigSource.xml' path='//Interface[@name="IConfigSource"]/docs/*' />
	public interface IConfigSource
	{
		/// <include file='IConfigSource.xml' path='//Property[@name="Configs"]/docs/*' />
		ConfigCollection Configs { get; }

		/// <include file='IConfigSource.xml' path='//Property[@name="IsReadOnly"]/docs/*' />
		bool IsReadOnly { get; }
		
		/// <include file='IConfigSource.xml' path='//Method[@name="Merge"]/docs/*' />
		void Merge (IConfigSource source);
		
		/// <include file='IConfigSource.xml' path='//Method[@name="Save"]/docs/*' />
		void Save ();
		
		/// <include file='IConfigSource.xml' path='//Method[@name="SaveAll"]/docs/*' />
		void SaveAll ();
	}
}
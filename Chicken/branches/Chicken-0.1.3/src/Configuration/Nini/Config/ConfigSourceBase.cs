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
using System.Collections;

namespace Nini.Config
{
	/// <include file='IConfigSource.xml' path='//Class[@name="IConfigSource"]/docs/*' />
	public abstract class ConfigSourceBase
	{
		#region Private variables
		ArrayList sourceList = new ArrayList ();
		ConfigCollection configList = new ConfigCollection ();
		#endregion

		#region Constructors
		#endregion
		
		#region Public properties
		/// <include file='IConfigSource.xml' path='//Property[@name="Configs"]/docs/*' />
		public ConfigCollection Configs
		{
			get { return configList; }
		}
		#endregion
		
		#region Public methods
		/// <include file='IConfigSource.xml' path='//Method[@name="Merge"]/docs/*' />
		public void Merge (IConfigSource source)
		{
			if (!sourceList.Contains (source))  {
				sourceList.Add (source);
			}
			
			foreach (IConfig config in source.Configs)
			{
				this.Configs.Add (config);
			}
		}
		
		/// <include file='IConfigSource.xml' path='//Method[@name="SaveAll"]/docs/*' />
		public void SaveAll ()
		{
			foreach (IConfigSource source in sourceList)
			{
				if (!source.IsReadOnly) {
					source.Save ();
				}
			}
		}
		#endregion

		#region Private methods
		#endregion
	}
}

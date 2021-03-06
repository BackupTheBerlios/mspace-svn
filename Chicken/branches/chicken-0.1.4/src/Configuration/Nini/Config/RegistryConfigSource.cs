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
using Microsoft.Win32;
using Nini.Ini;

namespace Nini.Config
{
	/// <include file='RegistryConfigSource.xml' path='//Enum[@name="RegistryRecurse"]/docs/*' />
	public enum RegistryRecurse
	{
		/// <include file='RegistryConfigSource.xml' path='//Enum[@name="RegistryRecurse"]/Value[@name="None"]/docs/*' />
		None,
		/// <include file='RegistryConfigSource.xml' path='//Enum[@name="RegistryRecurse"]/Value[@name="Flattened"]/docs/*' />
		Flattened,
		/// <include file='RegistryConfigSource.xml' path='//Enum[@name="RegistryRecurse"]/Value[@name="Namespacing"]/docs/*' />
		Namespacing
	}

	/// <include file='RegistryConfigSource.xml' path='//Class[@name="RegistryConfigSource"]/docs/*' />
	public class RegistryConfigSource : ConfigSourceBase, IConfigSource
	{
		#region Private variables
		#endregion
		
		#region Public properties
		/// <include file='IConfigSource.xml' path='//Property[@name="IsReadOnly"]/docs/*' />
		public bool IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region Constructors
		#endregion
		
		#region Public methods
		/// <include file='RegistryConfigSource.xml' path='//Method[@name="AddMapping"]/docs/*' />
		public void AddMapping (RegistryKey registryKey, string path)
		{
			RegistryKey key = registryKey.OpenSubKey (path, true);
			
			if (key == null) {
				throw new ArgumentException ("The specified key does not exist");
			}
			
			LoadKeyValues (key, ShortKeyName (key));
		}
		
		/// <include file='RegistryConfigSource.xml' path='//Method[@name="AddMappingRecurse"]/docs/*' />
		public void AddMapping (RegistryKey registryKey, 
								string path, 
								RegistryRecurse recurse)
		{
			RegistryKey key = registryKey.OpenSubKey (path, true);
			
			if (key == null) {
				throw new ArgumentException ("The specified key does not exist");
			}
			
			if (recurse == RegistryRecurse.Namespacing) {
				Console.WriteLine (key.Name + " " + path);
				LoadKeyValues (key, path);
			} else {
				LoadKeyValues (key, ShortKeyName (key));
			}
			
			string[] subKeys = key.GetSubKeyNames ();
			for (int i = 0; i < subKeys.Length; i++)
			{
				switch (recurse)
				{
				case RegistryRecurse.None:
					// no recursion
					break;
				case RegistryRecurse.Namespacing:
					AddMapping (registryKey, path + "\\" + subKeys[i], recurse);
					break;
				case RegistryRecurse.Flattened:
					AddMapping (key, subKeys[i], recurse);
					break;
				}
			}
		}
		
		/// <include file='IConfigSource.xml' path='//Method[@name="Save"]/docs/*' />
		public void Save ()
		{
			if (this.IsReadOnly) {
				throw new Exception ("Source is read only");
			}

			foreach (RegistryConfig config in this.Configs)
			{
				string[] keys = config.GetKeys ();
				
				for (int i = 0; i < keys.Length; i++)
				{
					 config.Key.SetValue (keys[i], config.Get (keys[i]));
				}
			}
		}
		#endregion
		
		#region Private methods
		/// <summary>
		/// Loads all values in a Registry keyS
		/// </summary>
		private void LoadKeyValues (RegistryKey key, string keyName)
		{
			string[] values = key.GetValueNames ();

			RegistryConfig config = new RegistryConfig (keyName, this);
			config.Key = key;

			foreach (string value in values)
			{
				config.Add (value, key.GetValue (value).ToString ());
			}
			this.Configs.Add (config);
		}
		
		/// <summary>
		/// Returns the key name without the fully qualified path.
		/// e.g. no HKEY_LOCAL_MACHINE\\MyKey, just MyKey
		/// </summary>
		private string ShortKeyName (RegistryKey key)
		{
			int index = key.Name.LastIndexOf ("\\");

			return (index == -1) ? key.Name : key.Name.Substring (index + 1);
		}
		
		/// <summary>
		/// Registry Config class.
		/// </summary>
		private class RegistryConfig : ConfigBase
		{
			RegistryKey key = null;
			
			/// <summary>
			/// Constructor.
			/// </summary>
			public RegistryConfig (string name, IConfigSource source)
				: base (name, source)
			{
			}
			
			/// <summary>
			/// Registry key for the Config.
			/// </summary>
			public RegistryKey Key
			{
				get { return key; }
				set { key = value; }
			}
		}
		#endregion
	}
}

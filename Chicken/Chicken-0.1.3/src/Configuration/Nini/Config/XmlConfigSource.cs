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
using System.Xml;
using System.Reflection;
using System.Collections;
using System.Configuration;
using System.Collections.Specialized;

namespace Nini.Config
{
	/// <include file='XmlConfigSource.xml' path='//Class[@name="XmlConfigSource"]/docs/*' />
	public class XmlConfigSource : ConfigSourceBase, IConfigSource
	{
		#region Private variables
		bool isReadOnly = false;
		XmlDocument configDoc = null;
		string savePath = null;
		#endregion

		#region Constructors
		/// <include file='XmlConfigSource.xml' path='//Constructor[@name="ConstructorPath"]/docs/*' />
		public XmlConfigSource (string path)
		{
			savePath = path;
			XmlDocument document = new XmlDocument ();
			document.Load (path);
			PerformLoad (document);
		}

		/// <include file='XmlConfigSource.xml' path='//Constructor[@name="ConstructorXmlDoc"]/docs/*' />
		public XmlConfigSource (XmlDocument document)
		{
			isReadOnly = true;
			PerformLoad (document);
		}
		#endregion
		
		#region Public properties
		/// <include file='IConfigSource.xml' path='//Property[@name="IsReadOnly"]/docs/*' />
		public bool IsReadOnly
		{
			get { return isReadOnly; }
		}
		#endregion
		
		#region Public methods
		/// <include file='IConfigSource.xml' path='//Method[@name="Save"]/docs/*' />
		public void Save ()
		{
			if (this.IsReadOnly) {
				throw new Exception ("Source is read only");
			}

			foreach (IConfig config in this.Configs)
			{
				string[] keys = config.GetKeys ();
				
				for (int i = 0; i < keys.Length; i++)
				{
					SetKey (config.Name, keys[i], config.Get (keys[i]));
				}
			}
			
			configDoc.Save (savePath);
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Loads all sections and keys.
		/// </summary>
		private void PerformLoad (XmlDocument doc)
		{
			this.Merge (this); // required for SaveAll
			configDoc = doc;
			
			XmlNode rootNode = configDoc.SelectSingleNode ("/Nini");
			
			if (rootNode == null) {
				throw new Exception ("Did not find NiniXml root node");
			}
			
			LoadSections (rootNode);
		}
		
		/// <summary>
		/// Loads all configuration sections.
		/// </summary>
		private void LoadSections (XmlNode rootNode)
		{
			XmlNodeList nodeList = rootNode.SelectNodes ("Section");
			ConfigBase config = null;
			
			for (int i = 0; i < nodeList.Count; i++)
			{
				config = new ConfigBase (nodeList[i].Attributes["Name"].Value, this);
				
				this.Configs.Add (config);
				LoadKeys (nodeList[i], config);
			}
		}
		
		/// <summary>
		/// Loads all keys for a config.
		/// </summary>
		private void LoadKeys (XmlNode node, ConfigBase config)
		{
			XmlNodeList nodeList = node.SelectNodes ("Key");

			for (int i = 0; i < nodeList.Count; i++)
			{
				config.Add (nodeList[i].Attributes["Name"].Value,
							nodeList[i].Attributes["Value"].Value);
			}
		}
		
		/// <summary>
		/// Sets an XML key.
		/// </summary>
		private void SetKey (string section, string key, string value)
		{
			string search = "Nini/Section[@Name='" + section 
							+ "']/Key[@Name='" + key + "']";
			
			XmlNode node = configDoc.SelectSingleNode (search);
			
			if (node != null) {
				node.Attributes["Value"].Value = value;
			}
		}
		#endregion
	}
}
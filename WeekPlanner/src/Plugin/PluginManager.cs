namespace WeekPlanner
{
		using System.Collections;
		using System;
		using System.Xml;
		using System.IO;
		
		public class PluginManager : IEnumerable
		{
				private ArrayList list;
				private string pluginsFile = App.AppDir + Path.DirectorySeparatorChar + "plugins.xml";

				public PluginManager ()
				{
						LoadPlugins ();
				}
				
				//FIXME
				//Finish implementation
				private void LoadPlugins ()
				{
						XmlDocument doc = new XmlDocument ();
						doc.Load (new FileStream (pluginsFile, FileMode.Open));
						XmlNode docElement = doc.DocumentElement;
						XmlNodeList result = docElement.SelectNodes ("/plugins/plugin");
						foreach (XmlNode node in result)
								Console.WriteLine ("plugin found");
				}

				public IEnumerator GetEnumerator ()
				{
						return list.GetEnumerator ();
				}
		}
}

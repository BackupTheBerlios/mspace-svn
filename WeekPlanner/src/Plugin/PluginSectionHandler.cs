namespace WeekPlanner
{
	using System.Configuration;
	using System.Xml;
	using System;
	public class PluginSectionHandler
	{

	// Iterate through all the child nodes
	//   of the XMLNode that was passed in and create instances
   //   of the specified Types by reading the attribite values of the nodes
   //   we use a try/Catch here because some of the nodes
   //   might contain an invalid reference to a plugin type
		   public object Create(object parent, 
				 object configContext, 
				 System.Xml.XmlNode section)
		   {
			  foreach(XmlNode node in section.ChildNodes)
			  {
				 //Code goes here to instantiate
				 //and invoke the plugins
				 Console.WriteLine ("Plugin");
			  }
			  return null;
		   }
	}
}

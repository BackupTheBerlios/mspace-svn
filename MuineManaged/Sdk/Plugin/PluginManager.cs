// created on 7/19/2004 at 7:30 PM
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
using System.Collections;
using System.IO;
using System.Reflection;

/*
 * Load the plugins dynamically when muine is started.
 * Every plugin in $HOME/.gnome2/muine/plugins will be loaded if the assembly
 * is decorated with the PluginInfo Attribute.
 */
 
public class PluginManager
{
	//This info is sensible to the platform. If muine is ported to other OS
	//this should be changed.
	private string pluginsDir = Environment.GetEnvironmentVariable ("HOME") +
								+ Path.DirectorySeparatorChar + ".gnome2" +
								+ Path.DirectorySeparatorChar + "muine" +
								+ Path.DirectorySeparatorChar + "plugins";
	
	//This will hold the list of plugins loaded.
	private ArrayList pluginList = new ArrayList ();
	
	public PluginManager ()
	{
		InitPluginsDir ();
	}
	
	
	
	private bool enabled = true;
	/*
	 * If Enabled == false, the plugin manager will not load any plugin
	 */
	public bool Enabled {
		get {
			return enabled;
		}
		set {
			enabled = value;
		}
	}
	
	// FIXME: should we use a thread to run each plugin?
	// We should handle the case when there is a plugin that cannot
	// be loaded (Notifiying the user and so).
	// Should we verify that Plugin Names are unique?
	/*
	 * Try to load as plugins all the assemblies found in pluginsDir.
	 * The PluginInfo attribute must be present into the assembly.
	 * If PluginInfo is not found the plugin is not loaded.
	 */
	public void LoadPlugins ()
	{
		if (enabled)
		{
			string[] files = Directory.GetFiles (pluginsDir);
			foreach (string assemblie in files)
			{
#if DEBUG_PLUGINS
				Console.WriteLine ("Trying to load assembly {0}", assemblie);
#endif
				try {
					Assembly asm = Assembly.LoadFrom (assemblie);
					object[] attrs = asm.GetCustomAttributes (typeof (PluginInfoAttribute), false);
					if (attrs.Length == 0)
					{
#if DEBUG_PLUGINS
						Console.WriteLine ("WARNING: Plugin info not found in {0}",
											assemblie);
						Console.WriteLine ("Disabling plugin.");
#endif
						// Assembly does not have PluginInfo, skip...
						continue;
					}
					PluginInfoAttribute pinfo = attrs[0] as PluginInfoAttribute;
#if DEBUG_PLUGINS
					Console.WriteLine ("Info loaded");
#endif
					IPlugin plugin = asm.CreateInstance (pinfo.Class) as IPlugin;
					switch (pinfo.Type)
					{
						case PluginType.Active:
							AppContext.ABus.AddMember (plugin);
							break;
						case PluginType.Passive:
							AppContext.EBus.AddMember (plugin);
							break;
						case PluginType.Standard:
							AppContext.EBus.AddMember (plugin);
							AppContext.ABus.AddMember (plugin);
							break;
						default:
							break;
					}
					pluginList.Add (plugin);
#if DEBUG_PLUGINS
					Console.WriteLine("INFO: Plugin loaded succesfully.\nClass: {0}\nName: {1}",
										pinfo.Class,
										pinfo.Name);
#endif
				} catch (Exception e) {
#if DEBUG_PLUGINS
					Console.WriteLine ("ERROR: Could not load assembly");
					Console.WriteLine (e.Message);
#endif
				}
			}
		}
	}
	
	/*
	 * Checks if the plugins dir exist, if not, we create it.
	 */
	private void InitPluginsDir ()
	{
		try {
			if (!Directory.Exists (pluginsDir))
				Directory.CreateDirectory (pluginsDir);
		} catch {
			enabled = false;
#if DEBUG_PLUGINS
			Console.WriteLine ("ERROR: Cannot use plugins dir. Plugins disabled.");
#endif
		}
	}
	
}
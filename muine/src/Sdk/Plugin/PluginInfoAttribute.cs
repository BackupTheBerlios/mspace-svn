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

/*
 * This attribute registers a plugin within the suitable
 * bus manager.
 * PluginType.Active: the plugin is registered within the ActionBus.
 * PluginType.Pasive: the plugin is registered within the EventBus.
 * PluginType.Standard: the plugin is registered in both buses, ActionBus
 * and EventBus.
 */						
[AttributeUsage (AttributeTargets.Assembly)]
public class PluginInfoAttribute : Attribute
{

	public PluginInfoAttribute (string pluginClass,
								string pluginName,
								string pluginDescription,
								PluginType type)
	{
		this.type = type;
		this.name = pluginName;
		this.pclass = pluginClass;
		this.description = pluginDescription;
	}
	
	private PluginType type;
	public PluginType Type {
		get {
			return type;
		}
	}
	
	private string name;
	public string Name {
		get {
			return name;
		}
	}
	
	private string pclass;
	public string Class {
		get {
			return pclass;
		}
	}
		
	private string description;
	public string Description {
		get {
			return description;
		}
	}
	
}
public enum PluginType {
						Active,
						Passive,
						Standard
						}

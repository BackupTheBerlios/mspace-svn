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
using Gnome;
using GConf;

/* 
 * some of this stuff should be hiddend. Bus should help on this task
 */
public class AppContext
{
    private PluginManager pluginManager = new PluginManager ();
	
    public static readonly EventBus EBus = new EventBus ();
    
    public static readonly ActionBus ABus = new ActionBus ();
    
    public static GConf.Client GConfClient;

    public static SongDatabase DB;

    public static CoverDatabase CoverDB;

    public static ActionThread ActionThread;

    public static GettextCatalog Catalog;
    
    public AppContext ()
    {
	StockIcons.Initialize ();
    	pluginManager.LoadPlugins ();
    	Catalog = new GettextCatalog ("muine");
    	/* Init GConf */
		GConfClient = new GConf.Client ();
		/* Start the action thread */
		ActionThread = new ActionThread ();
		/* Load cover database */
		try {
			CoverDB = new CoverDatabase (2);
		} catch (Exception e) {
			new ErrorDialog (String.Format (AppContext.Catalog.GetString ("Failed to load the cover database: {0}\n\nExiting..."), e.Message));

			Environment.Exit (0);
		}
		
		/* Load song database */
		try {
			DB = new SongDatabase (3);
		} catch (Exception e) {
			new ErrorDialog (String.Format (AppContext.Catalog.GetString ("Failed to load the song database: {0}\n\nExiting..."), e.Message));

			Environment.Exit (0);	
		}


    }
}


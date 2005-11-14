/*
Babuine Component Model & Babuine Framework
Copyright (C) 2005  Néstor Salceda Alonso

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Collections;
using ComponentModel.Interfaces;
using ComponentModel.Container.Dao;
using ComponentModel.Exceptions;
using ComponentModel.DTO;
using NLog;


namespace ComponentModel.Container {
    public class DefaultContainer : IContainer {
        private static DefaultContainer instance = null; 
        private Hashtable componentHashtable;
        //Logging
        Logger logger = LogManager.GetLogger ("ComponentModel.Container.DefaultContainer");
        
        private DefaultContainer () {
            Console.WriteLine ("--- Babuine Component Model & Babuine Framework Inicializated ----");
            logger.Debug ("Init DefaultContainer");
            logger.Debug ("Getting Relative Search Path: " + AppDomain.CurrentDomain.RelativeSearchPath);
            logger.Debug ("Getting Dynamic Directory: " + AppDomain.CurrentDomain.DynamicDirectory);
            logger.Debug ("Getting Private Binary Path: " + AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);
            logger.Debug ("Getting Private Binary Path 2: " + AppDomain.CurrentDomain.SetupInformation.PrivateBinPathProbe);
            logger.Debug ("Get MONO_PATH : " + Environment.GetEnvironmentVariable ("MONO_PATH"));
            //Antes de nada, cargar los ensamblados que se encuentren en el
            //MONO_PATH
            this.LoadAssembliesInPath ();
            
            //Getting data from assembly resolv.
            componentHashtable = new Hashtable ();
            //lock (componentHashtable) {
            //En cada ensamblado, cargará el / los componente y lo registrará con el
            //nombre que se le ha dado al atributo.
            GetAllComponents ();
            //}
            logger.Debug ("Exiting default ctor.");
        }
        
        private void LoadAssembliesInPath () {
            //Get el path
            string monoPathEnvironment = Environment.GetEnvironmentVariable ("MONO_PATH");
            string[] monoPaths = monoPathEnvironment.Split (':');
            //GetAssembliesInPath
            for (int i = 0; i< monoPaths.Length; i++) {
                this.GetAssembliesInPath (monoPaths[i]);
            }
        }
        
        private void GetAssembliesInPath (string monoPath) {
            logger.Debug ("Getting all assemblies in: " + monoPath);
            if (Directory.Exists (monoPath)) {
                String[] assemblies = Directory.GetFiles (monoPath, "*.dll"); 
                foreach (String assembly in assemblies) {
                    logger.Debug ("Assembly finded: " + assembly);
                    Assembly.LoadFrom (assembly);
                }
            }
        }
        
        private void GetAllComponents () {
            //Append private path assemblies to current domain :)
            foreach (Assembly ass  in AppDomain.CurrentDomain.GetAssemblies ()) {
                ICollection collection = DefaultContainerDao.Instance.ProcessAssembly (ass);
                if (collection.Count != 0)
                    RegisterComponent (collection);
            }
            //TODO: Get Private Components
        
        }
        
        private void RegisterComponent (ICollection collection) {
            logger.Debug ("Entering RegisterComponent.");
            logger.Info ("Finded " + collection.Count + " components");
            IEnumerator enumerator = collection.GetEnumerator ();
            while (enumerator.MoveNext ()) {
                //¿Intefaces ?!!
                DefaultComponentModel defaultComponentModel = (DefaultComponentModel)enumerator.Current;
                this.Add (defaultComponentModel); 
            }
            logger.Debug ("Exiting RegisterComponent.");
            logger.Info ("Container has : " + componentHashtable.Count + " Components Registered.");
        }
        
        public static DefaultContainer Instance {
            get {
                if (instance == null)
                    instance = new DefaultContainer ();
                return instance;
            }
        }

        public IComponentModel GetComponentByName (string componentName) {
            //lock (componentHashtable) {
            IComponentModel componentModel = (IComponentModel) componentHashtable[componentName];
            if (componentModel == null) {
                throw new ComponentNotFoundException ("Component " + componentName + " not found in container.");
            }
            return componentModel;
            //}
        }

        public IComponentModel this [string componentName] {
            get {
                return GetComponentByName (componentName);
            }
        }

        public void Add (IComponentModel component) {
            componentHashtable.Add (component.ComponentModelDTO.ComponentName, component);
            logger.Info ("Registering component: " + component + " as Name: " + component.ComponentModelDTO.ComponentName);
        }

        public void Remove (IComponentModel component) {
            componentHashtable.Remove (component.ComponentModelDTO.ComponentName);
        }

        /*Servicio ejecutor*/
        public ResponseMethodDTO Execute (string componentName, string methodName, object[] parameters, bool redirect, IViewHandler viewHandler, bool block) {
            IComponentModel componentModel = this.GetComponentByName (componentName);
            return componentModel.Execute (methodName, parameters, redirect, viewHandler, block);
        }

        public ResponseMethodDTO Execute (string componentName, string methodName, object[] parameters, IViewHandler viewHandler) {
            return this.Execute (componentName, methodName, parameters, true, viewHandler, true);
        }       

        public ResponseMethodDTO Execute (string componentName, string methodName, object[] parameters, bool redirect) {
            return this.Execute (componentName, methodName, parameters, redirect, null, true);
        }

        public ResponseMethodDTO Execute (string componentName, string methodName, object[] parameters) {
            return this.Execute (componentName, methodName, parameters, true, null, true);
        }
    }
}

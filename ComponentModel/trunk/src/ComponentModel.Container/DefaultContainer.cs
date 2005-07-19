using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Collections;
using ComponentModel.Interfaces;
using ComponentModel.Container.Dao;
using ComponentModel.Exceptions;
using ComponentModel.VO;
using NLog;


namespace ComponentModel.Container {
    public class DefaultContainer : IContainer {
        private static DefaultContainer instance = null; 
        //private IList componentList;
        private Hashtable componentHashtable;
        //Logging
        Logger logger = LogManager.GetLogger ("ComponentModel.Container.DefaultContainer");
        
        private DefaultContainer () {
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
            //componentList = new ArrayList ();
            componentHashtable = new Hashtable ();
            //En cada ensamblado, cargará el / los componente y lo registrará con el
            //nombre que se le ha dado al atributo.
            GetAllComponents (); 
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
            //for (int i = 0; i < componentList.Count; i++) {
            //    if ((componentList[i] as DefaultComponentModel).VO.ComponentName.Equals (componentName)) {
            //        return (IComponentModel)componentList[i];
            //    }
            //}
            try {
                return (IComponentModel)componentHashtable[componentName];
            }
            catch (Exception e) {
                throw new ComponentNotFoundException ("Component " + componentName + " not found in container.");
            }
            //return null;
        }

        public void Add (IComponentModel component) {
            /**
             * Dado que un componente es un peso pesado para la carga de un
             * sistema no se permite más de una instancia, así que con pasarle
             * la referencia será suficiente.
             */
            //if (componentList.Contains (component))
            //    return;
            /*Chequeamos que no existan dos componentes con el mismo nombre.*/
            //for (int i = 0; i< componentList.Count; i++) {
            //    if (((IComponentModel)componentList[i]).VO.ComponentName.Equals (component.VO.ComponentName)) {
                    /*No lanzo una excepción porque no se debe parar la
                     * ejecución del programa por este error.*/
            //        return;
            //    }
            //}
            componentHashtable.Add (component.VO.ComponentName, component);
            logger.Info ("Registering component: " + component + " as Name: " + component.VO.ComponentName);
        }

        public void Remove (IComponentModel component) {
            //if (componentList.Contains (component)) {
            //    componentList.Remove (component);
            //}
            componentHashtable.Remove (component.VO.ComponentName);
        }

        /*Servicio ejecutor*/
        public ResponseMethodVO Execute (string componentName, string methodName, object[] parameters, bool redirect, IViewHandler viewHandler, bool block) {
            IComponentModel componentModel = this.GetComponentByName (componentName);
            return componentModel.Execute (methodName, parameters, redirect, viewHandler, block);
        }

        public ResponseMethodVO Execute (string componentName, string methodName, object[] parameters, IViewHandler viewHandler) {
            return this.Execute (componentName, methodName, parameters, true, viewHandler, true);
        }       

        public ResponseMethodVO Execute (string componentName, string methodName, object[] parameters, bool redirect) {
            return this.Execute (componentName, methodName, parameters, redirect, null, true);
        }

        public ResponseMethodVO Execute (string componentName, string methodName, object[] parameters) {
            return this.Execute (componentName, methodName, parameters, true, null, true);
        }
    }
}

using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections;
using ComponentModel.Interfaces;
using ComponentModel.Container.Dao;
using NLog;

using System.Threading;

namespace ComponentModel.Container {
    public class DefaultContainer : IContainer {
        private static DefaultContainer instance = null; 
        private static Assembly[] assemblies; 
        private IList componentList;
        //Logging
        Logger logger = LogManager.GetLogger ("ComponentModel.Container.DefaultContainer");
        
        internal Assembly[] Assemblies {
            get {
                if (assemblies == null) {
                    AppDomain appDomain = AppDomain.CurrentDomain;
                    assemblies = appDomain.GetAssemblies ();
                }
                logger.Debug ("Finded: " + assemblies.Length +" assemblies.");
                return assemblies;
            }
        }
        
        private DefaultContainer () {
            logger.Debug ("Init DefaultContainer");
            componentList = new ArrayList ();
            //En cada ensamblado, cargará el / los componente y lo registrará con el
            //nombre que se le ha dado al atributo.
            GetAllComponents (); 
        }

        public void GetAllComponents () {
            foreach (Assembly ass  in Assemblies) {
                ICollection collection = DefaultContainerDao.Instance.ProcessAssembly (ass);
                if (collection.Count != 0)
                    RegisterComponent (collection);
            }
        }
        
        private void RegisterComponent (ICollection collection) {
            logger.Debug ("Entering RegisterComponent.");
            logger.Debug ("Finded " + collection.Count + " components");
            IEnumerator enumerator = collection.GetEnumerator ();
            while (enumerator.MoveNext ()) {
                DefaultComponentModel defaultComponentModel = (DefaultComponentModel)enumerator.Current;
                //Registrar aqui el exceptionClass
                this.Add (defaultComponentModel); 
            }
            logger.Debug ("Exiting RegisterComponent.");
        }
        
        public static DefaultContainer Instance {
            get {
                Console.WriteLine ("Getting default Instance of Container.");
                if (instance == null)
                    instance = new DefaultContainer ();
                Console.WriteLine ("Exiting getting default instance of Container.");
                return instance;
            }
        }

        public IComponentModel GetComponentByName (string componentName) {
            for (int i = 0; i < componentList.Count; i++) {
                if ((componentList[i] as DefaultComponentModel).VO.Name.Equals (componentName)) {
                    return (IComponentModel)componentList[i];
                }
            }
            return null;
            //Exception
        }

        public void Add (IComponentModel component) {
            if (componentList.Contains (component))
                return;
            componentList.Add (component);
            logger.Debug ("Registering component: " + component + " as Name: " + component.VO.Name);
        }

        public void Remove (IComponentModel component) {
            if (componentList.Contains (component)) {
                componentList.Remove (component);
            }
        }
    }
}

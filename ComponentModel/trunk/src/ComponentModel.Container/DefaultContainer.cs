using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections;
using ComponentModel.Interfaces;
using ComponentModel.Container.Dao;
using NLog;

namespace ComponentModel.Container {
    public class DefaultContainer : IContainer {
        private static DefaultContainer instance = null; 
        private Assembly[] assemblies; 
        private IList componentList;
        
        //Logging
        Logger logger = LogManager.GetLogger ("ComponentModel.Container.DefaultContainer");
        
        internal Assembly[] Assemblies {
            get {
                return assemblies;
            }
        }
        
        private DefaultContainer () {
            logger.Debug ("Init DefaultContainer");
            componentList = new ArrayList ();
            //Toma el único dominio de la aplicacion
            AppDomain appDomain = AppDomain.CurrentDomain;
            assemblies = appDomain.GetAssemblies ();
            foreach (Assembly ass  in assemblies) {
                ICollection collection = DefaultContainerDao.Instance.ProcessAssembly (ass);
                //DEbugg
                //foreach (Type type in ass.GetTypes ()) {
                //    logger.Debug ("Type finded: " + type.ToString ());
                //}
                //
                RegisterComponent (collection);
            }
            //En cada ensamblado, cargará el / los componente y lo registrará con el
            //nombre que se le ha dado al atributo.

        }

        private void RegisterComponent (ICollection collection) {
            IEnumerator enumerator = collection.GetEnumerator ();
            while (enumerator.MoveNext ()) {
                DefaultComponentModel defaultComponentModel = (DefaultComponentModel)enumerator.Current;
                this.Add (defaultComponentModel); 
            }
        }
        
        public static DefaultContainer Instance {
            get {
                if (instance == null)
                    instance = new DefaultContainer ();
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

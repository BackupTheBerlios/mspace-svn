using System;
using System.Collections;
using ComponentModel.Interfaces;

namespace ComponentModel.Container {
    public class DefaultContainer : IContainer {
        private static DefaultContainer instance; 
       
        private IList componentList;
        
        private DefaultContainer () {
            componentList = new ArrayList ();
            //Leerá el xml y cargara en la lista los componentes
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
                if ((componentList[i] as IComponentModel).VO.Name.Equals (componentName)) {
                    
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
        }

        public void Remove (IComponentModel component) {
            if (componentList.Contains (component)) {
                componentList.Remove (component);
            }
        }
    }
}

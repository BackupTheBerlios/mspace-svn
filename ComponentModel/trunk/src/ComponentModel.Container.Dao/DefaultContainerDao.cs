using System;
using System.Reflection;
using ComponentModel.Interfaces;
using ComponentModel.Container;
using ComponentModel.VO;

namespace ComponentModel.Container.Dao {
    public class DefaultContainerDao {
        private static DefaultContainerDao instance = null;
        
        private DefaultContainerDao () {
        }
        
        public static DefaultContainerDao Instance {
            get {
                if (instance == null)
                    instance = new DefaultContainerDao ();
                return instance;
            }
        }

        public void ProcessAssembly (Assembly assembly) {
            Type[] types = assembly.GetTypes ();
            //TODO filter with memberfilter :)
            for (int i = 0; i < types.Length; i++) {
                //Debe ser una subclase de DefaultComponentModel (implemente
                //IComponentModel)
                if (types[i].IsSubclassOf (typeof (DefaultComponentModel))) {
                    Attribute[] attributes = (Attribute[])types[i].GetCustomAttributes (typeof (ComponentAttribute), true); 
                    if (attributes.Length.Equals (1)) { 
                        //Deberiamos registrar el tipo en el Container. Y
                        //parsear la información para rellenar su VO.
                        ComponentModelVO componentModelVO = this.fillVO (types[i]);

                        Console.WriteLine (types[i]);
                    }
                }
            }
        }

        private ComponentModelVO fillVO (Type type) {
            ComponentModelVO componentModelVO = new ComponentModelVO ();
            ComponentAttribute componentAttribute = (ComponentAttribute)(type.GetCustomAttributes (typeof (ComponentAttribute), true)[0]);
            
            componentModelVO.ClassName = type.FullName;
            componentModelVO.Name = componentAttribute.ComponentName;
            componentModelVO.ExceptionClassName = componentAttribute.ExceptionManager;
            
            return componentModelVO;
        }
    }
}

using System;
using System.Collections;
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

        public ICollection ProcessAssembly (Assembly assembly) {
            ArrayList list = new ArrayList ();
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
                        ConstructorInfo constructorInfo = types[i].GetConstructor (null);
                        DefaultComponentModel defaultComponentModel = (DefaultComponentModel)constructorInfo.Invoke (null);
                        /**
                        FieldInfo voFieldInfo = types[i].GetField ("vO", BindingFlags.NonPublic);
                        voFieldInfo.SetValue (defaultComponentModel, componentModelVO);
                        */
                        //TODO Reflect it !!
                        defaultComponentModel.SetVO (componentModelVO);
                        list.Add (defaultComponentModel);
                    }
                }
            }
            return (ICollection)list;
        }

        /**
         * Rellenamos el VO con los valores que tenga asociado ese tipo.
         */
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
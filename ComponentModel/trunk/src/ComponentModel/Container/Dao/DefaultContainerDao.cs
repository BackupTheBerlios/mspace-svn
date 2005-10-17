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
using System.Collections;
using System.Reflection;
using ComponentModel.Interfaces;
using ComponentModel.Factory;
using ComponentModel.Container;
using ComponentModel.DTO;
using ComponentModel.Exceptions;
using NLog;

namespace ComponentModel.Container.Dao {
    public class DefaultContainerDao {
        private static DefaultContainerDao instance = null;
        private Logger logger;
        
        private DefaultContainerDao () {
            logger = LogManager.GetLogger (this.GetType ().ToString ());
        }
        
        public static DefaultContainerDao Instance {
            get {
                if (instance == null)
                    instance = new DefaultContainerDao ();
                return instance;
            }
        }

        public Type GetType (string type) {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies (); 
            for (int i = 0; i < assemblies.Length; i++) {
                Type[] types = assemblies[i].GetTypes ();
                for (int j = 0; j < types.Length; j++) {
                    if (types[j].FullName.Equals (type))
                        return types[j];
                }
            }
            throw new TypeNotFoundException ("Type can't be resolved in current app domain.");
        }
        
        public ICollection ProcessAssembly (Assembly assembly) {
            logger.Debug ("Assembly to Process: " + assembly.FullName);
            ArrayList list = new ArrayList ();
            //VAmos a comprobar que no se procesen ensamblados que sean del
            //core.
            
            if (assembly.FullName.StartsWith ("mscorlib") ||
                assembly.FullName.StartsWith ("System")
                    ) {
                logger.Debug ("Core Assembly detected: " + assembly.FullName);
                logger.Info ("* SKIPPING * Core Assembly: " + assembly.FullName);
                return list;
            }
            
            Type[] types = assembly.GetTypes ();
            //TODO filter with memberfilter :)
            for (int i = 0; i < types.Length; i++) {
                //Debe ser una subclase de DefaultComponentModel (implemente
                //IComponentModel)
                if (types[i].IsSubclassOf (typeof (DefaultComponentModel))) {
                    Attribute[] attributes = (Attribute[])types[i].GetCustomAttributes (typeof (ComponentAttribute), true); 
                    if (attributes.Length.Equals (1)) { 
                        //Deberiamos registrar el tipo en el Container. Y
                        //parsear la información para rellenar su DTO.
                        ComponentModelDTO componentModelDTO = this.fillDTO (types[i]);
                        ConstructorInfo constructorInfo = types[i].GetConstructor (null);
                        DefaultComponentModel defaultComponentModel = (DefaultComponentModel)constructorInfo.Invoke (null);
                        //Seteamos el vo con reflection y mantener oculto el
                        //resto.
                        FieldInfo voFieldInfo = types[i].GetField ("vO", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField); // | BindingFlags.DeclaredOnly);
                        //if (voFieldInfo == null)
                        //    Console.WriteLine ("FieldInfo == null.");
                        voFieldInfo.SetValue (defaultComponentModel, componentModelDTO);
                        list.Add (defaultComponentModel);
                    }
                }
            }
            return (ICollection)list;
        }

        /**
         * Rellenamos el DTO con los valores que tenga asociado ese tipo.
         */
        private ComponentModelDTO fillDTO (Type type) {
            ComponentModelDTO componentModelDTO =  (ComponentModelDTO) FactoryDTO.Instance.Create (CreateDTO.ComponentModel);
            ComponentAttribute componentAttribute = (ComponentAttribute)(type.GetCustomAttributes (typeof (ComponentAttribute), true)[0]);
            
            componentModelDTO.ComponentClassName = type.FullName;
            componentModelDTO.ComponentName = componentAttribute.ComponentName;
            componentModelDTO.ExceptionManagerClassName = componentAttribute.ExceptionManager;
            
            return componentModelDTO;
        }
    }
}

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

namespace ComponentModel {
    
    [AttributeUsage (AttributeTargets.Class,  AllowMultiple = false, Inherited = false)]
    public class ComponentAttribute : Attribute {
        //Campos
        private string componentName;
        private string exceptionManager;
        
        //Propiedades públicos.
        public string ExceptionManager {
            get {return exceptionManager;}
        }
        
        public string ComponentName {
            get {return componentName;}
        }
        
        //Métodos privados.
        private void SetExceptionManager (string exceptionManager) {
            if (exceptionManager == null)
                throw new Exception ("Null value not allowed.");
            if (exceptionManager.Equals (String.Empty))
                throw new Exception ("String empty not allowed.");
            this.exceptionManager = exceptionManager;
        }
        
        private void SetComponentName (string componentName) {
            if (componentName == null) 
                throw new Exception ("Null value not allowed.");
            if (componentName.Equals (String.Empty))
                throw new Exception ("String empty not allowed.");
            this.componentName = componentName;
        }
        
        /**
         * Generalmente los parámetros requeridos se deben pasar en el
         * constructor, para forzar así en tiempo de compilación el checkeo de
         * que el número es correcto.
         */
        public ComponentAttribute (string componentName, string exceptionManager) : base () {
            this.SetComponentName (componentName);
            this.SetExceptionManager (exceptionManager);
        }
    }
}

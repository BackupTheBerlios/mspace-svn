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

    [AttributeUsage (AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ComponentMethodAttribute : Attribute {
        //Campos
        private string responseName;
        private string viewName;

        //Propiedades públicas.
        public string ViewName {
            get {return viewName;}
        }
        
        public string ResponseName {
            get {return responseName;}
        }

        //Métodos privados.
        private void SetViewName (string viewName) {
            if (viewName == null)
                throw new Exception ("Null value not allowed.");
            if (viewName.Equals (String.Empty))
                throw new Exception ("String empty not allowed.");
            this.viewName = viewName;
        }
        
        private void SetResponseName (string responseName) {
            if (responseName == null)
                throw new Exception ("Null value not allowed.");
            if (responseName.Equals (String.Empty))
                throw new Exception ("String empty not allowed.");
            this.responseName = responseName;
        }

        /**
         * Generalmente los parámetros requeridos se le pasan en orden al
         * constructor.  Los opcionales se le pasarán como propiedades.
         */
        public ComponentMethodAttribute (string viewName, string responseName) : base () {
            this.SetViewName (viewName);
            this.SetResponseName (responseName);
        }
        
    }
}

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

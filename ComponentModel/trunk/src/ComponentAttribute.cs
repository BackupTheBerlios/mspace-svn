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

using System;

namespace ComponentModel.DefaultComponentModel {
    
    [AttributeUsage (AttributeTargets.Class,  AllowMultiple = false, Inherited = false)]
    public class ComponentAttribute : Attribute {
        private string componentName;
        private string exceptionManager;

        public string ExceptionManager {
            get {return exceptionManager;}
            set {exceptionManager = value;}
        }
        
        public string ComponentName {
            get {return componentName;}
            set {componentName = value;}
        }

        public ComponentAttribute () : base () {
        }
        
        public ComponentAttribute (string componentName, string exceptionManager) : base () {
            this.ComponentName = componentName;    
            this.ExceptionManager = exceptionManager;
        }
    }
}

using System;
using System.Collections;

namespace ComponentModel.VO {
    public class ComponentModelVO {
        private string componentName;
        private string componentClassName;
        private string exceptionManagerClassName;

        public string ExceptionManagerClassName {
            get {return exceptionManagerClassName;}
            set {exceptionManagerClassName = value;}
        }
        
        public string ComponentClassName {
            get {return componentClassName;}
            set {componentClassName = value;}
        }
        
        public string ComponentName {
            get {return componentName;}
            set {componentName = value;}
        }
    }
}

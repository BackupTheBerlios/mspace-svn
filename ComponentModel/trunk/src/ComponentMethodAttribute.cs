using System;

namespace ComponentModel {

    [AttributeUsage (AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ComponentMethodAttribute : Attribute {
        private string responseName;
        private string viewName;

        public string ViewName {
            get {return viewName;}
            set {viewName = value;}
        }
        
        public string ResponseName {
            get {return responseName;}
            set {responseName = value;}
        }

        public ComponentMethodAttribute () : base () {
        }
        
        public ComponentMethodAttribute (string viewName, string responseName) : base () {
            this.ViewName = viewName;
            this.ResponseName = responseName;
        }
        
    }
}

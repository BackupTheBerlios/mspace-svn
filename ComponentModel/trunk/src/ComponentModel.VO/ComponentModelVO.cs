using System;
using System.Collections;

namespace ComponentModel.VO {
    public class ComponentModelVO {
        private string name;
        private string interfaceFile;
        private Hashtable methods;

        private string className;
        private string exceptionClassName;


        public ComponentModelVO () {
            methods = new Hashtable ();
        }
        
        public string ExceptionClassName {
            get {return exceptionClassName;}
            set {exceptionClassName = value;}
        }
        
        public string ClassName {
            get {return className;}
            set {className = value;}
        }
        
        public Hashtable Methods {
            get {return methods;}
            set {methods = value;}
        }
        
        public string InterfaceFile {
            get {return interfaceFile;}
            set {interfaceFile = value;}
        }

        public string Name {
            get {return name;}
            set {name = value;}
        }

    }
}

using System;

namespace ComponentModel.Vo {
    public class ComponentModelVO {
        private string name;
        private string interfaceFile;

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

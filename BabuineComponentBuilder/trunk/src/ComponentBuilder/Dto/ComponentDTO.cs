using System;
using System.Collections;
using System.Collections.Specialized;
using ComponentModel.Interfaces;

namespace ComponentBuilder.DTO {
    [Serializable]
    public sealed class ComponentDTO : IDataTransferObject {
        //Nombre del componente
        private string componentName;
        //Gestor de exceptions
        private string classExceptionManager;
        //Collection de vistas
        private StringCollection viewCollection; 
        //Collection de casos de uso
        private IList methodCollection;

        public ComponentDTO () {
            viewCollection = new StringCollection ();
            methodCollection = new ArrayList ();
        }
        
        public IList MethodCollection {
            get {return methodCollection;}
            set {methodCollection = value;}
        }

        public StringCollection ViewCollection {
            get {return viewCollection;}
            set {viewCollection = value;}
        }

        public string ClassExceptionManager {
            get {return classExceptionManager;}
            set {classExceptionManager = value;}
        }   
        
        public string ComponentName {
            get {return componentName;}
            set {componentName = value;}
        }
        
    }
}

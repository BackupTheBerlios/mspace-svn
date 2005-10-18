using System;
using System.Collections;
using System.Collections.Specialized;
using ComponentModel.Interfaces;

namespace ComponentBuilder.DTO {
    [Serializable]
    public sealed class ComponentSettingsDTO : IDataTransferObject {
        //Nombre del componente
        private string componentName;
        //Gestor de exceptions
        private string classExceptionManager;
        //Collection de vistas
        private StringCollection viewCollections; 
        //Collection de casos de uso
        private IList methodCollection;

        public IList MethodCollection {
            get {return methodCollection;}
            set {methodCollection = value;}
        }

        public StringCollection ViewCollections {
            get {return viewCollections;}
            set {viewCollections = value;}
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

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
        private StringCollection viewsCollection; 
        //Collection de casos de uso
        private IList methodsCollection;

        public IList MethodsCollection {
            get {return methodsCollection;}
            set {methodsCollection = value;}
        }

        public StringCollection ViewsCollection {
            get {return viewsCollection;}
            set {viewsCollection = value;}
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

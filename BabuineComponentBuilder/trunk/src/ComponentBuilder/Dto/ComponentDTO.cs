using System;
using System.Xml.Serialization;
using System.Collections;
using ComponentModel.Interfaces;

namespace ComponentBuilder.DTO {
    [Serializable]
    [XmlInclude (typeof (MethodDTO))]
    [XmlInclude (typeof (ViewDTO))]
    public sealed class ComponentDTO : IDataTransferObject {
        //Nombre del componente
        private string componentName;
        //Gestor de exceptions
        private string classExceptionManager;
        //Collection de vistas
        private IList viewCollection; 
        //Collection de casos de uso
        private IList methodCollection;

        public ComponentDTO () {
            componentName = String.Empty;
            classExceptionManager = String.Empty;
            viewCollection = new ArrayList ();
            methodCollection = new ArrayList ();
        }
        
        public IList MethodCollection {
            get {return methodCollection;}
            set {methodCollection = value;}
        }

        public IList ViewCollection {
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

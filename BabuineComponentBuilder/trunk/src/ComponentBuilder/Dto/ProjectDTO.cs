using System;
using System.Xml.Serialization;
using System.Collections;
using ComponentModel.Interfaces;

namespace ComponentBuilder.DTO {
    [Serializable]
    public enum DeployMode {
        Application,
        ComponentLibrary,
        SingleComponent
    }
    
    [Serializable]
    [XmlInclude (typeof (ComponentDTO))]
    public sealed class ProjectDTO : IDataTransferObject {
        string projectName;
        IList componentCollection;
        DeployMode deployMode;    

        public ProjectDTO () {
            componentCollection = new ArrayList ();
        }
        
        public DeployMode DeployMode {
            get {return deployMode;}
            set {deployMode = value;}
        }
        
        public IList ComponentCollection {
            get {return componentCollection;}
            set {componentCollection = value;}
        }
        
        public string ProjectName {
            get {return projectName;}
            set {projectName = value;}
        }
    }
}

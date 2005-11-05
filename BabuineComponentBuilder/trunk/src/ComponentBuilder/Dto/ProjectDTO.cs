using System;
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
    public sealed class ProjectDTO : IDataTransferObject {
        string projectName;
        IList componentCollection;
        DeployMode deployMode;    

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

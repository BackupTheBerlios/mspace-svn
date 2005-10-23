using System;
using ComponentModel.Interfaces;

namespace ComponentBuilder.DTO {
    [Serializable]
    public sealed class ConfigurationDTO : IDataTransferObject {
        string baseOutPath;
        bool generateNAnt;
        
        public bool GenerateNAnt {
            get {return generateNAnt;}
            set {generateNAnt = value;}
        }
        
        public string BaseOutPath {
            get {return baseOutPath;}
            set {baseOutPath = value;}
        }
    }
}

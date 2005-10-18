using System;
using ComponentModel.Interfaces;

namespace ComponentBuilder.DTO {
    [Serializable]
    public sealed class ParameterDTO : IDataTransferObject {
        private string typeName;
        private string varName;

        public string VarName {
            get {return varName;}
            set {varName = value;}
        }
        
        public string TypeName {
            get {return typeName;}
            set {typeName = value;}
        }
    }
}

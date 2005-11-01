using System;
using ComponentModel.Interfaces;

namespace ComponentBuilder.DTO {
    [Serializable]
    public sealed class PreferencesDTO : IDataTransferObject {
        private string outputPath;
        private string prefixNamespace;
        private bool generateBuildfile;

        public PreferencesDTO () {
            OutputPath = String.Empty;
            PrefixNamespace = String.Empty;
            //GenerateBuildfile = true;
            GenerateBuildfile = false;
        }
        
        public bool GenerateBuildfile {
            get {
                //return generateBuildfile;
                return false;
            }
            set {generateBuildfile = value;}
        }
        
        public string PrefixNamespace {
            get {return prefixNamespace;}
            set {prefixNamespace = value;}
        }
        
        public string OutputPath {
            get {return outputPath;}
            set {outputPath = value;}
        }
    }
}

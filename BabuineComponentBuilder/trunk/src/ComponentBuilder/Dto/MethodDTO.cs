using System;
using System.Collections;
using ComponentModel.Interfaces;

namespace ComponentBuilder.DTO {
    [Serializable]
    public sealed class MethodDTO : IDataTransferObject {
        private string returnType;
        private string methodName;
        private string viewToResponse;
        private string responseMethod;
        private IList parameterCollection;

        public IList ParameterCollection {
            get {return parameterCollection;}
            set {parameterCollection = value;}
        }
        
        public string ResponseMethod {
            get {return responseMethod;}
            set {responseMethod = value;}
        }
        
        public string ViewToResponse {
            get {return viewToResponse;}
            set {viewToResponse = value;}
        }
        
        public string MethodName {
            get {return methodName;}
            set {methodName = value;}
        }
        
        public string ReturnType {
            get {return returnType;}
            set {returnType = value;}
        }
        
    }
}

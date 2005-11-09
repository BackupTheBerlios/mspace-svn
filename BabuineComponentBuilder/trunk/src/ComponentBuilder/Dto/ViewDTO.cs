using System;
using ComponentModel.Interfaces;

namespace ComponentBuilder.DTO {
    [Serializable]
    public sealed class ViewDTO : IDataTransferObject {
        private string viewName;

        public string ViewName {
            get {return viewName;}
            set {viewName = value;}
        }
    }
}

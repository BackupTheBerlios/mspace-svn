using System;

namespace ComponentModel.Interfaces {
    public interface IContainer {
        IComponentModel GetComponentByName (string componentName);
        
        void Add (IComponentModel componentModel);
        void Remove (IComponentModel componentModel);
    }
}

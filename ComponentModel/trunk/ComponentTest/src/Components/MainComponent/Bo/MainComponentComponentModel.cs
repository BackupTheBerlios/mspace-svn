using System;
using ComponentModel;

namespace ComponentModel.ComponentTest.Components.MainComponent.Bo {
    [Component (ComponentName="MainComponent", ExceptionManager="ComponentModel.ComponentTest.Components.MainComponent.Exception.MainComponentExceptionManager")]
    public class MainComponentComponentModel : DefaultComponentModel {
    }
}

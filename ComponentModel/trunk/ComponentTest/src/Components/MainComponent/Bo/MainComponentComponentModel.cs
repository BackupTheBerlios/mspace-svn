using System;
using ComponentModel;

namespace ComponentModel.ComponentTest.MainComponent.Bo {
    [Component (ComponentName="MainComponent", ExceptionManager="ComponentModel.ComponentTest.MainComponent.Exception.MainComponentExceptionManager")]
    public class MainComponentComponentModel : DefaultComponentModel {
    }
}

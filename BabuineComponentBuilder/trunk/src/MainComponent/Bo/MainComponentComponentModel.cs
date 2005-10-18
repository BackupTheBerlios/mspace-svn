using System;
using ComponentModel;
using ComponentModel.Container;

namespace MainComponent.Bo {
    [Component ("MainComponent", "MainComponent.Exceptions.MainComponentExceptionManager")]
    public sealed class MainComponentComponentModel : DefaultComponentModel {
        [ComponentMethod ("MainComponent.Forms.MainComponentForm", "ResponseInitApp")]
        public void InitApp () {
        }
    }
}

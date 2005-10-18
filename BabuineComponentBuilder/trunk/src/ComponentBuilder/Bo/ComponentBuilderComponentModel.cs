using System;
using ComponentModel;

namespace ComponentBuilder.Bo {
    [Component ("ComponentBuilder", "ComponentBuilder.Exceptions.ComponenBuilderExceptionManager")]
    public sealed class ComponentBuilderComponentModel : DefaultComponentModel {
       
        [ComponentMethod ("ComponentBuilder.Forms.MainComponentBuilderForm", "ResponseShowForm")]
        public void ShowForm () {
        }
    }
}

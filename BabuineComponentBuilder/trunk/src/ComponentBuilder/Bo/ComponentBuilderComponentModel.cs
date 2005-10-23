using System;
using ComponentModel;
using ComponentBuilder.DTO;

namespace ComponentBuilder.Bo {
    [Component ("ComponentBuilder", "ComponentBuilder.Exceptions.ComponentBuilderExceptionManager")]
    public sealed class ComponentBuilderComponentModel : DefaultComponentModel {
       
        [ComponentMethod ("ComponentBuilder.Forms.MainComponentBuilderForm", "ResponseShowForm")]
        public void ShowForm () {
        }

        [ComponentMethod ("ComponentBuilder.Forms.MainComponentBuilderForm", "ResponseGenerateComponent")]
        public void GenerateComponent (ComponentSettingsDTO componentSettingsDTO) {
        }
    }
}

using System;
using ComponentModel;

namespace ComponentModel.ComponentTest.Saludator.Bo {
   [Component (ComponentName="Saludator", ExceptionManager="ComponentModel.ComponentText.Saludator.Exception.SaludatorExceptionManager")] 
   public sealed class SaludatorComponentModel : DefaultComponentModel {
       public SaludatorComponentModel () : base () {
       }

       [ComponentMethod (ViewName = "ComponentModel.ComponentTest.Saludator.Form.SaludatorForm", ResponseName = "ResponseSaluda")]
       public void Saluda () {
       }
   }
}

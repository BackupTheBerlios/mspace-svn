using System;
using ComponentModel;

namespace ComponentModel.ComponentTest.Components.Saludator.Bo {
   [Component (ComponentName="Saludator", ExceptionManager="ComponentModel.ComponentTest.Components.Saludator.Exception.SaludatorExceptionManager")] 
   public sealed class SaludatorComponentModel : DefaultComponentModel {
       public SaludatorComponentModel () : base () {
       }

       //ComponentModel.ComponentTest.Components.Saludator.Form.SaludatorForm
       [ComponentMethod (ViewName = "ComponentModel.ComponentTest.Components.Saludator.Form.SaludatorForm", ResponseName = "ResponseSaluda")]
       public int Saluda () {
           Console.WriteLine ("Que pasa co !!");
           return 1;
       }
   }
}

using System;
using ComponentModel;
using ComponentModel.Container;

namespace ComponentModel.ComponentTest.Components.Saludator.Bo {
   [Component (ComponentName="Saludator", ExceptionManager="ComponentModel.ComponentTest.Components.Saludator.Exception.SaludatorExceptionManager")] 
   public sealed class SaludatorComponentModel : DefaultComponentModel {
       public SaludatorComponentModel () : base () {
       }

       [ComponentMethod (ViewName = "ComponentModel.ComponentTest.Components.Saludator.Form.SaludatorForm", ResponseName = "ResponseSaluda")]
       public int Saluda () {
           Console.WriteLine ("Que pasa co !!");
           DefaultComponentModel dcm = (DefaultComponentModel)DefaultContainer.Instance.GetComponentByName ("Saludatorr");
           throw new System.Exception ("Yeahhh");
           return 1;
       }

       [ComponentMethod (ViewName = "ComponentModel.ComponentTest.Components.Saludator.Form.SaludatorForm", ResponseName = "ResponseSaludaATodos")]
       public void SaludaATodos () {
            Console.WriteLine ("Saludaré a todos si me los pides, bellaco !!"); 
       }
   }
}

using System;
using ComponentModel;

namespace ComponentModel.ComponentTest.Components.Saludator.Bo {
   [Component (ComponentName="Saludator", ExceptionManager="ComponentModel.ComponentTest.Components.Saludator.Exceptions.SaludatorExceptionManager")] 
   public sealed class SaludatorComponentModel : DefaultComponentModel {
       public SaludatorComponentModel () : base () {
       }

        [ComponentMethod (ViewName = "ComponentModel.ComponentTest.Components.Saludator.Form.SaludatorForm", ResponseName = "ResponseInitApp")]
        public void InitApp () {
        }
       
        [ComponentMethod (ViewName = "ComponentModel.ComponentTest.Components.Saludator.Form.SaludatorForm", ResponseName = "ResponseSaluda")]
        public int Saluda () {
            Console.WriteLine ("Que pasa co !!");
            throw new Exception ("Yeahhhh");
            return 1;
        }

        [ComponentMethod (ViewName = "ComponentModel.ComponentTest.Components.Saludator.Form.SaludatorForm", ResponseName = "ResponseSaludaATodos")]
        public void SaludaATodos () {
            Console.WriteLine ("Saludaré a todos si me los pides, bellaco !!"); 
        }
   }
}

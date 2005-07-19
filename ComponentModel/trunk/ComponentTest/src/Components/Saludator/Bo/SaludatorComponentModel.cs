using System;
using ComponentModel;

namespace ComponentModel.ComponentTest.Components.Saludator.Bo {
   [Component ("Saludator","ComponentModel.ComponentTest.Components.Saludator.Exceptions.SaludatorExceptionManager")] 
   public sealed class SaludatorComponentModel : DefaultComponentModel {
       public SaludatorComponentModel () : base () {
       }

        [ComponentMethod ("ComponentModel.ComponentTest.Components.Saludator.Form.SaludatorForm", "ResponseInitApp")]
        public void InitApp () {
            Console.WriteLine ("//-----------------//");
            Console.WriteLine ("LLamada en el BO para iniciar la applicattion.");
            Console.WriteLine ("//----------------//");
        }
       
        [ComponentMethod ("ComponentModel.ComponentTest.Components.Saludator.Form.SaludatorForm", "ResponseSaluda")]
        public int Saluda () {
            Console.WriteLine ("//-----------------//");
            Console.WriteLine ("Que pasa co !!");
            //throw new Exception ("Yeahhhh");
            Console.WriteLine ("//-----------------//");
            return 1;
        }

        [ComponentMethod ("ComponentModel.ComponentTest.Components.Saludator.Form.SaludatorForm", "ResponseSaludaATodos")]
        public void SaludaATodos () {
            Console.WriteLine ("Saludaré a todos si me los pides, bellaco !!"); 
        }
   }
}

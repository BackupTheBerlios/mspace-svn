using System;
using ComponentModel;
using ComponentModel.VO;

namespace ComponentModel.ComponentTest.Components.Saludator.Form {
    public class SaludatorForm {
        public SaludatorForm () {
        }
        
        public void ResponseSaluda (ResponseMethodVO response) {
            Console.WriteLine ("Response hola !!");
            Console.WriteLine ((int)response.ResponseValue);
        }
    }
}

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

        public void ResponseSaludaATodos (ResponseMethodVO response) {
            Console.WriteLine ("Aquí ejecutaré el response del caso de uso.\nBellaco lo será tu calavera en almibar.");
        }
    }
}

using System;

using AppExample.Components.Saludator.Bo;

namespace AppExample.Components.MainComponent.Bo {
    public class MainComponentComponentModel {
        public MainComponentComponentModel () {
        }

        public static void Co () {
            Console.WriteLine ("Hola co !!");
        }
        
        //Use case initApp
        public void InitApp () {
            new SaludatorComponentModel ().Saluda ();
        }
    }
}

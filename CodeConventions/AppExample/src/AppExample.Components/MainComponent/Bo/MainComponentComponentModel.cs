using System;

using AppExample.Components.Saludator.Bo;

namespace AppExample.Components.MainComponent.Bo {
    public class MainComponentComponentModel {
        public MainComponentComponentModel () {

        }

        //Use case initApp
        public void InitApp () {
            new SaludatorComponentModel ().Saluda ();
        }
    }
}

using System;
//Esto tendrá uno menos de entrada o incluso no debe ser necesario para el
//desacople entre componentes.
//
//Falta también la instancia única del componente.
using AppExample.Components.MainComponent.Bo;

namespace AppExample {
            //OR AppExample
    
    public class Launcher {
        public static void Main () {
            new MainComponentComponentHandler ().InitApp ();
        }
    }
}

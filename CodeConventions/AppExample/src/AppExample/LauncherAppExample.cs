using System;
//Esto tendrá uno menos de entrada o incluso no debe ser necesario para el
//desacople entre componentes.
//
//Falta también la instancia única del componente.
using AppExample.Components.MainComponent.Bo;

using Test;

namespace AppExample {
            //OR AppExample
    
    public class Launcher {
        public static void Main () {
            MainComponentComponentModel.Co ();

            MainComponentComponentModel mc = new MainComponentComponentModel ();
            mc.InitApp ();
            Console.WriteLine ("Hello world"); 
            Out.Write ("Hello!!");           
        }
    }
}

using System;
using System.Diagnostics;
using System.Reflection;
using ComponentModel.Container;

namespace ComponentModel.ComponentTest {
    public class Launcher {
        public static void Main () {
            DefaultContainer defaultContainer = DefaultContainer.Instance; 
        //    Console.WriteLine (Assembly.GetExecutingAssembly ().FullName);
            DefaultComponentModel dcm = (DefaultComponentModel)defaultContainer.GetComponentByName ("Saludator");
            Debug.Assert (dcm != null);
            if (dcm != null)
                Console.WriteLine ("Dcm no es null");
            else
                Console.WriteLine ("Dcm es null");
            dcm.Execute ("Saluda", null);
        }
    }
}

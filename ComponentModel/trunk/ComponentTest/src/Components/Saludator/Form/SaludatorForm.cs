using System;
using ComponentModel;
using ComponentModel.Container;
using ComponentModel.VO;
using ComponentModel.Interfaces;
using Gtk;

namespace ComponentModel.ComponentTest.Components.Saludator.Form {
    public class SaludatorForm : IViewHandler {
        private Window window;
        private Button button;
        
        public SaludatorForm ()  {
        /**
            Application.Init ();
            window = new Window ("Saludator Form from Component Test");
            button = new Button ("Pulsame");
            button.Clicked += new EventHandler (OnButtonClicked);
            window.Add (button);
       */
        }
       
        public void ResponseInitApp (ResponseMethodVO response) {
         //   window.ShowAll ();
         //   Application.Run ();
         //   Application.RunIteration ();
            Console.WriteLine ("Response Initapp executed.");
        }
        
        public void ResponseSaluda (ResponseMethodVO response) {
            
            Console.WriteLine ("Response hola !!");
            Console.WriteLine ((int)response.ResponseValue);
        }

        public void ResponseSaludaATodos (ResponseMethodVO response) {
            Console.WriteLine ("Aquí ejecutaré el response del caso de uso.\nBellaco lo será tu calavera en almibar.");
        }

        private void OnButtonClicked (object o, EventArgs args) {
            DefaultComponentModel dcm = (DefaultComponentModel) DefaultContainer.Instance.GetComponentByName ("Saludator"); 
            dcm.Execute ("Saluda", null);
        }
    }
}

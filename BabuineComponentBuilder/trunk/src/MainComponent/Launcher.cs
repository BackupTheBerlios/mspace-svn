using System;
using ComponentModel.Interfaces;
using ComponentModel.Container;
using Gtk;

namespace MainComponent {
    public sealed class Launcher {
        public static void Main () {
            Gdk.Threads.Init ();
            Gtk.Application.Init ();
            IComponentModel componentModel = DefaultContainer.Instance.GetComponentByName ("MainComponent");
            Gdk.Threads.Enter ();
            componentModel.Execute ("InitApp", null);
            //DefaultContainer.Instance.Execute ("MainComponent", "InitApp", null);
            Gtk.Application.Run ();
            Gdk.Threads.Leave ();
        }
    }
}

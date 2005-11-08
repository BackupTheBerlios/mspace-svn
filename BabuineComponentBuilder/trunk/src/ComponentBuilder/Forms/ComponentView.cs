using System;
using ComponentModel.Interfaces;
using ComponentBuilder.Interfaces;
using Gtk;
using Glade;

namespace ComponentBuilder.Forms {
    internal class ComponentView : IViewHandler, IGtkView {
        Glade.XML componentView;
        
        internal ComponentView () {
            componentView = new Glade.XML (null, "MainComponentBuilderForm.glade", "table5", null);
            //componentView.Autoconnect (this);
        }
        
        /* Interface Implementation */
        public void LoadDataForm (IDataTransferObject dto) {
        }

        public void ClearForm () {
        }

        public IDataTransferObject GetDataForm () {
            return null;
        }

        public Widget GetWidget () {
            return componentView["table5"];
        }
    }
}

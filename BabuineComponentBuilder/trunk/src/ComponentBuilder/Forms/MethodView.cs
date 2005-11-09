using System;
using ComponentModel.Interfaces;
using ComponentBuilder.Interfaces;
using ComponentBuilder.DTO;
using Gtk;
using Glade;

namespace ComponentBuilder.Forms {
    internal class MethodView : IViewHandler, IGtkView {
        Glade.XML newMethodForm;
        
        internal MethodView () {
            newMethodForm = new Glade.XML (null, "MainComponentBuilderForm.glade", "newMethodDialog", null);
            newMethodForm.Autoconnect (this);
        }

        private void OnNewParameterClicked (object sender, EventArgs args) {
        }

        /* Interface Implementation */

        public void ClearForm () {
        }

        public IDataTransferObject GetDataForm () {
            Dialog newMethodDialog = (Dialog) GetWidget ();
            Console.WriteLine (newMethodDialog.Run ());
            newMethodDialog.Destroy ();
            newMethodDialog = null;
            return null;
        }

        public void LoadDataForm (IDataTransferObject dto) {
            if (dto is MethodDTO) {
            }
        }

        public Widget GetWidget () {
            return newMethodForm["newMethodDialog"];
        }

    }
}

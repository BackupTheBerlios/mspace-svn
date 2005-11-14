using System;
using ComponentModel.Interfaces;
using ComponentBuilder.Interfaces;
using ComponentBuilder.DTO;
using Gtk;
using Glade;

namespace ComponentBuilder.Forms {
    internal class FormView : IViewHandler, IGtkView {
        Glade.XML newViewForm;
        [Widget] Entry viewNameEntry;   
        
        internal FormView () {
            newViewForm = new Glade.XML (null, "MainComponentBuilderForm.glade", "newViewDialog", null);
            newViewForm.Autoconnect (this);
        }

        /* Interface Implementation */

        public void ClearForm () {
            viewNameEntry.Text = String.Empty;
        }

        public IDataTransferObject GetDataForm () {
            Dialog newViewDialog = (Dialog) GetWidget ();
            ViewDTO viewDTO = null;
            switch (newViewDialog.Run ()) {
                case (int) ResponseType.Ok:
                    if (viewNameEntry.Text.Length != 0) { 
                        viewDTO = new ViewDTO ();
                        viewDTO.ViewName = viewNameEntry.Text;
                    }
                    break;
                case (int) ResponseType.Cancel:
                    break;
                default:
                    break;
            }
            newViewDialog.Destroy ();
            newViewDialog = null;
            return viewDTO;
        }

        public void LoadDataForm (IDataTransferObject dto) {
            if (dto is ViewDTO) {
                ViewDTO viewDTO = (ViewDTO) dto;
                viewNameEntry.Text = viewDTO.ViewName;
            }
        }

        public Widget GetWidget () {
            return newViewForm ["newViewDialog"];
        }

        private void OnViewNameEntryActivate (object sender, EventArgs args) {
            Dialog dialog = (Dialog) GetWidget ();
            dialog.Respond (ResponseType.Ok);
            dialog = null;
        }
    }
}

using System;
using ComponentModel.Interfaces;
using ComponentBuilder.Interfaces;
using ComponentBuilder.DTO;
using Gtk;
using Glade;

namespace ComponentBuilder.Forms {
    internal class PreferencesView : IViewHandler, IGtkView {
        private static PreferencesView instance;
        Glade.XML preferencesForm;

        [Widget] Entry defaultOutputPathEntry, prefixNamespaceEntry; 
        
        internal PreferencesView () {
            preferencesForm = new Glade.XML (null, "MainComponentBuilderForm.glade", "preferencesDialog", null);
            preferencesForm.Autoconnect (this);
        }

        private void OnBrowseButtonClicked (object sender, EventArgs args) {
            FileChooserDialog chooser = new FileChooserDialog ("Select a path:", (Window)preferencesForm["preferencesDialog"], FileChooserAction.SelectFolder, Stock.Ok);
            if (defaultOutputPathEntry.Text.Length != 0) {
                chooser.SetCurrentFolder (defaultOutputPathEntry.Text);
            }
            chooser.AddButton (Stock.Ok, ResponseType.Accept);
            ResponseType responseType = (ResponseType) chooser.Run ();
            switch (responseType) {
                case ResponseType.Accept:
                    defaultOutputPathEntry.Text = chooser.CurrentFolder;
                    break;
                default:
                    break;
            }
            chooser.Destroy ();
        }

        /* Interface Implementation */

        public void ClearForm () {
            defaultOutputPathEntry.Text = String.Empty;
            prefixNamespaceEntry.Text = String.Empty;
        }

        public IDataTransferObject GetDataForm () {
            Dialog preferencesDialog = (Dialog) GetWidget ();
            PreferencesDTO preferencesDTO = null;
            switch (preferencesDialog.Run ()) {
                case (int) ResponseType.Ok:
                    if (defaultOutputPathEntry.Text.Length != 0 && prefixNamespaceEntry.Text.Length != 0) {
                        preferencesDTO = new PreferencesDTO ();
                        preferencesDTO.OutputPath = defaultOutputPathEntry.Text;
                        preferencesDTO.PrefixNamespace = prefixNamespaceEntry.Text;
                    }
                    break;
                case (int) ResponseType.Cancel:
                    break;
                default:
                    break;
            }
            preferencesDialog.Destroy ();
            preferencesDialog = null;
            return preferencesDTO;
        }

        public void LoadDataForm (IDataTransferObject dto) {
            if (dto is PreferencesDTO) {
                PreferencesDTO preferencesDTO = (PreferencesDTO) dto;
                defaultOutputPathEntry.Text = preferencesDTO.OutputPath;
                prefixNamespaceEntry.Text = preferencesDTO.PrefixNamespace;
            }
        }

        public Widget GetWidget () {
            return preferencesForm ["preferencesDialog"];
        }
    }
}

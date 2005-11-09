using System;
using ComponentModel.Interfaces;
using ComponentBuilder.Interfaces;
using ComponentBuilder.DTO;
using Gtk;
using Glade;

namespace ComponentBuilder.Forms {
    internal class ParameterView : IViewHandler, IGtkView {
        Glade.XML newParameterForm;
        
        [Widget] Entry typeEntry, varNameEntry;
        
        internal ParameterView () {
            newParameterForm = new Glade.XML (null, "MainComponentBuilderForm.glade", "newParameterDialog", null);
            newParameterForm.Autoconnect (this);
        }

        /* Interface Implementation */

        public void ClearForm () {
            typeEntry.Text = String.Empty;
            varNameEntry.Text = String.Empty;
        }

        public IDataTransferObject GetDataForm () {
            Dialog newParameterDialog = (Dialog) newParameterForm ["newParameterDialog"];
            ParameterDTO parameterDTO = null;
            switch (newParameterDialog.Run ()) {
                case (int) ResponseType.Ok:
                    parameterDTO = new ParameterDTO ();
                    parameterDTO.TypeName = typeEntry.Text;
                    parameterDTO.VarName = varNameEntry.Text;
                    break;
                case (int) ResponseType.Cancel:
                    break;
                default:
                    break;
            }
            newParameterDialog.Destroy ();
            newParameterDialog = null;
            return parameterDTO;
        }

        public void LoadDataForm (IDataTransferObject dto) {
            if (dto is ParameterDTO) {
                ParameterDTO parameterDTO = (ParameterDTO) dto;
                typeEntry.Text = parameterDTO.TypeName;
                varNameEntry.Text = parameterDTO.VarName;
            }
        }

        public Widget GetWidget () {
            return newParameterForm ["newParameterDialog"];
        }
    }
}

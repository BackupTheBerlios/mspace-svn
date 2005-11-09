using System;
using ComponentModel.Interfaces;
using ComponentBuilder.Interfaces;
using ComponentBuilder.Forms.TableModel;
using ComponentBuilder.DTO;
using Gtk;
using Glade;

namespace ComponentBuilder.Forms {
    internal class MethodView : IViewHandler, IGtkView {
        Glade.XML newMethodForm;

        [Widget] TreeView parametersTreeView;
        [Widget] Entry nameEntry, returnTypeEntry, responseMethodEntry;
        [Widget] ComboBox viewToResponseCombo;
        
        ParameterTableModel parameterTableModel;
        ParameterView parameterView;

        internal MethodView () {
            newMethodForm = new Glade.XML (null, "MainComponentBuilderForm.glade", "newMethodDialog", null);
            newMethodForm.Autoconnect (this);
        
            parametersTreeView.AppendColumn ("Type", new CellRendererText (), "text", 0);
            parametersTreeView.AppendColumn ("Variable Name", new CellRendererText (), "text", 1);
            parameterTableModel = new ParameterTableModel ();
            parametersTreeView.Model = parameterTableModel.ListStore;
        }

        private void OnNewParameterClicked (object sender, EventArgs args) {
            parameterView = new ParameterView ();
            ParameterDTO parameterDTO = (ParameterDTO) parameterView.GetDataForm ();
            if (parameterDTO != null) {
                parameterTableModel.Add (parameterDTO);
            }
            parameterView = null;
        }

        /* Interface Implementation */

        public void ClearForm () {
            nameEntry.Text = String.Empty;
            returnTypeEntry.Text = String.Empty;
            responseMethodEntry.Text = String.Empty;
            parameterTableModel = new ParameterTableModel ();
            TreeStore treeStore = new TreeStore (typeof (string));
            viewToResponseCombo.Model = treeStore;
        }

        public IDataTransferObject GetDataForm () {
            Dialog newMethodDialog = (Dialog) GetWidget ();
            MethodDTO methodDTO = null;
            switch (newMethodDialog.Run ()) {
                case (int) ResponseType.Ok :
                    methodDTO = new MethodDTO ();
                    methodDTO.MethodName = nameEntry.Text;
                    methodDTO.ReturnType = returnTypeEntry.Text;
                    TreeIter iter;
                    viewToResponseCombo.GetActiveIter (out iter);
                    methodDTO.ViewToResponse = (string) viewToResponseCombo.Model.GetValue (iter, 0);
                    methodDTO.ResponseMethod = responseMethodEntry.Text;
                    methodDTO.ParametersCollection = parameterTableModel.ListModel;
                    break;
                case (int) ResponseType.Cancel:
                    break;
                default:
                    break;
            }
            newMethodDialog.Destroy ();
            newMethodDialog = null;
            return methodDTO;
        }

        public void LoadDataForm (IDataTransferObject dto) {
            if (dto is MethodDTO) {
            }
            else if (dto is ComponentDTO) {
                ComponentDTO componentDTO = (ComponentDTO) dto;
                TreeStore treeStore = new TreeStore (typeof (string));
                viewToResponseCombo.Model = treeStore;
                foreach (ViewDTO viewDTO in componentDTO.ViewCollection) {
                    treeStore.AppendValues (viewDTO.ViewName);
                }
            }
        }

        public Widget GetWidget () {
            return newMethodForm["newMethodDialog"];
        }

    }
}

using System;
using ComponentModel.Interfaces;
using ComponentModel.DTO;
using ComponentBuilder.DTO;
using ComponentBuilder.Forms.TableModel;
using Gtk;
using Glade;

namespace ComponentBuilder.Forms {
    public sealed class MainComponentBuilderForm : IViewHandler {
        Glade.XML gxml, newViewDialog, newMethodDialog, newParameterDialog = null;
        [Widget] Statusbar statusbar1;
        [Widget] TreeView viewsTreeView, methodsTreeView;

        ViewTableModel viewTableModel;
        MethodTableModel methodTableModel;
        ParameterTableModel parameterTableModel;
        
        public MainComponentBuilderForm () {
            //La gracia sería sacar el VBox y pasarlo como si fuera un
            //container.
            gxml = new Glade.XML (null, "MainComponentBuilderForm.glade", "window1", null);
            gxml.Autoconnect (this);

            //Vamos a setar los models para los treeViews
            viewsTreeView.AppendColumn ("View Type Name", new CellRendererText (), "text", 0);
            
            methodsTreeView.AppendColumn ("Method Name", new CellRendererText (), "text", 0);
            methodsTreeView.AppendColumn ("Return Type", new CellRendererText (), "text", 1);
            methodsTreeView.AppendColumn ("View To Reponse", new CellRendererText (), "text", 2);
            methodsTreeView.AppendColumn ("Response Method", new CellRendererText (), "text", 3);
            methodsTreeView.AppendColumn ("Parameters", new CellRendererText (), "text", 4);
        }
        
        public IDataTransferObject GetDataForm () {
            return null;
        }

        public void LoadDataForm (IDataTransferObject dto) {
            if (dto is ComponentSettingsDTO) {
                ComponentSettingsDTO componentSettingsDTO = (ComponentSettingsDTO) dto;
            }
        }

        public void ClearForm () {
        }

        public void ResponseShowForm (ResponseMethodDTO response) {
            if (response.ExecutionSuccess) {
                response.MethodResult = gxml["vbox1"];
                statusbar1.Push (0, String.Format ("Welcome to Babuine Component Builder: {0}@{1}", Environment.UserName, Environment.MachineName));
                //Vamos a instanciar también los modelos de las vistas.
                viewTableModel = new ViewTableModel ();
                viewsTreeView.Model = viewTableModel.ListStore;
                methodTableModel = new MethodTableModel ();
                methodsTreeView.Model = methodTableModel.ListStore;
            }
        }
        
        private void OnWindow1DeleteEvent (object sender, DeleteEventArgs args) {
            Application.Quit ();
        }

        private void OnNewViewClicked (object sender, EventArgs args) {
            newViewDialog = new Glade.XML (null, "MainComponentBuilderForm.glade", "newViewDialog", null);
            Dialog dialog = (Dialog) newViewDialog ["newViewDialog"];
            switch (dialog.Run ()) {
                case (int) ResponseType.Ok:
                    Entry viewNameEntry = (Entry) newViewDialog ["viewNameEntry"];
                    if (viewNameEntry.Text.Length != 0) {
                        viewTableModel.Add (viewNameEntry.Text);
                    }
                    break;
                case (int) ResponseType.Cancel:
                    break;
                default:
                    break;
            }
            dialog.Destroy ();
            newViewDialog = null;
        }

        private void OnNewParameterClicked (object sender,EventArgs args) {
            newParameterDialog = new Glade.XML (null, "MainComponentBuilderForm.glade", "newParameterDialog", null); 
            Dialog dialog = (Dialog) newParameterDialog ["newParameterDialog"];
            switch (dialog.Run ()) {
                case (int) ResponseType.Ok:
                    Entry typeEntry = (Entry) newParameterDialog ["typeEntry"];
                    Entry varNameEntry = (Entry) newParameterDialog ["varNameEntry"];
                    if (typeEntry.Text.Length != 0 && varNameEntry.Text.Length != 0) {
                        //Suponemos que newMethodDialog estará instanciado y
                        //podremos acceder a él. 
                        ParameterDTO parameterDTO = new ParameterDTO ();
                        parameterDTO.TypeName = typeEntry.Text;
                        parameterDTO.VarName = varNameEntry.Text;
                        parameterTableModel.Add (parameterDTO);
                    }
                    break;
                case (int) ResponseType.Cancel:
                    break;
                default:
                    break;
            }
            dialog.Destroy ();
            newParameterDialog = null;
        }

        private void OnNewMethodClicked (object sender, EventArgs args) {
            newMethodDialog = new Glade.XML (null, "MainComponentBuilderForm.glade", "newMethodDialog", null);
            newMethodDialog.Autoconnect (this);
            Dialog dialog = (Dialog) newMethodDialog ["newMethodDialog"];
            //Creamos el TreeView de parameters
            TreeView parametersTreeView = (TreeView) newMethodDialog ["parametersTreeView"];
            parametersTreeView.AppendColumn ("Type", new CellRendererText (), "text", 0);
            parametersTreeView.AppendColumn ("Variable Name", new CellRendererText (), "text", 1);
            parameterTableModel = new ParameterTableModel ();
            parametersTreeView.Model = parameterTableModel.ListStore;
            //Barajar la opción de si merece la pena poner otro controlador para
            //esta vista.
            switch (dialog.Run ()) {
                case  (int) ResponseType.Ok:
                    Entry nameEntry = (Entry) newMethodDialog ["nameEntry"];
                    Entry returnTypeEntry = (Entry) newMethodDialog ["returnTypeEntry"];
                    Entry viewToResponseEntry = (Entry) newMethodDialog ["viewToResponseEntry"];
                    Entry responseMethodEntry = (Entry) newMethodDialog ["responseMethodEntry"];
                    if (nameEntry.Text.Length != 0 && returnTypeEntry.Text.Length != 0 &&
                        viewToResponseEntry.Text.Length != 0 && responseMethodEntry.Text.Length != 0    
                        ) {
                        MethodDTO methodDTO = new MethodDTO ();
                        methodDTO.MethodName = nameEntry.Text;
                        methodDTO.ReturnType = returnTypeEntry.Text;
                        methodDTO.ViewToResponse = viewToResponseEntry.Text;
                        methodDTO.ResponseMethod = responseMethodEntry.Text;
                        methodTableModel.Add (methodDTO);
                    }
                    break;
                case (int) ResponseType.Cancel:
                    break;
                default:
                    break;
            }
            dialog.Destroy ();
            newMethodDialog = null;
        }
    }
}

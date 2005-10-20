using System;
using ComponentModel.Interfaces;
using ComponentModel.DTO;
using ComponentBuilder.DTO;
using ComponentBuilder.Forms.TableModel;
using Gtk;
using Glade;

namespace ComponentBuilder.Forms {
    public sealed class MainComponentBuilderForm : IViewHandler {
        Glade.XML gxml = null;
        [Widget] Statusbar statusbar1;
        [Widget] TreeView viewsTreeView, methodsTreeView;
        
        public MainComponentBuilderForm () {
            gxml = new Glade.XML (null, "MainComponentBuilderForm.glade", "window1", null);
            gxml.Autoconnect (this);

            //Vamos a setar los models para los treeViews
            viewsTreeView.AppendColumn ("View Type Name", new CellRendererText (), "text", 0);
            
            methodsTreeView.AppendColumn ("Method Name", new CellRendererText (), "text", 0);
            methodsTreeView.AppendColumn ("Return Type", new CellRendererText (), "text", 1);
            methodsTreeView.AppendColumn ("Parameters", new CellRendererText (), "text", 2);
            methodsTreeView.AppendColumn ("View To Reponse", new CellRendererText (), "text", 3);
            methodsTreeView.AppendColumn ("Response Method", new CellRendererText (), "text", 4);
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
                viewsTreeView.Model = new ViewTableModel ().ListStore;
                methodsTreeView.Model = new MethodTableModel ().ListStore;
            }
        }
        
        private void OnWindow1DeleteEvent (object sender, DeleteEventArgs args) {
            Application.Quit ();
        }
    }
}

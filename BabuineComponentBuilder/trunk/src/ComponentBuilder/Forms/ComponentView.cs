using System;
using ComponentModel.Interfaces;
using ComponentBuilder.Interfaces;
using ComponentBuilder.DTO;
using ComponentBuilder.Forms.TableModel;
using Gtk;
using Glade;

namespace ComponentBuilder.Forms {
    internal class ComponentView : IViewHandler, IGtkView {
        Glade.XML componentView;
        [Widget] TreeView viewsTreeView, methodsTreeView;
        [Widget] Entry componentNameEntry, exceptionManagerEntry;
            
        ViewTableModel viewTableModel;
        MethodTableModel methodTableModel;
        
        internal ComponentView () {
            componentView = new Glade.XML (null, "MainComponentBuilderForm.glade", "table5", null);
            componentView.Autoconnect (this);

            //

            viewsTreeView.AppendColumn ("View Type Name", new CellRendererText (), "text", 0);
            
            methodsTreeView.AppendColumn ("Method Name", new CellRendererText (), "text", 0);
            methodsTreeView.AppendColumn ("Return Type", new CellRendererText (), "text", 1);
            methodsTreeView.AppendColumn ("View To Reponse", new CellRendererText (), "text", 2);
            methodsTreeView.AppendColumn ("Response Method", new CellRendererText (), "text", 3);
            methodsTreeView.AppendColumn ("Parameters", new CellRendererText (), "text", 4);

            //Vamos a instanciar también los modelos de las vistas.
        
            viewTableModel = new ViewTableModel ();
            viewsTreeView.Model = viewTableModel.ListStore;
            methodTableModel = new MethodTableModel ();
            methodsTreeView.Model = methodTableModel.ListStore;
        }
       
        /* GUI Events */
        private void OnNewMethodClicked (object sender, EventArgs args) {
        }

        private void OnNewViewClicked (object sender, EventArgs args) {
        }
        
        /* Interface Implementation */
        public void LoadDataForm (IDataTransferObject dto) {
            if (dto is ComponentDTO) {
                ComponentDTO componentDTO = (ComponentDTO) dto; 
                componentNameEntry.Text = componentDTO.ComponentName;
                exceptionManagerEntry.Text = componentDTO.ClassExceptionManager;
                viewTableModel = new ViewTableModel (componentDTO.ViewCollection);
                viewsTreeView.Model = viewTableModel.ListStore;
                methodTableModel = new MethodTableModel (componentDTO.MethodCollection);
                methodsTreeView.Model = viewTableModel.ListStore;
            }
        }

        public void ClearForm () {
            componentNameEntry.Text = String.Empty;
            exceptionManagerEntry.Text = String.Empty;
            viewTableModel = new ViewTableModel ();
            viewsTreeView.Model = viewTableModel.ListStore;
            methodTableModel = new MethodTableModel ();
            methodsTreeView.Model = methodTableModel.ListStore;
        }

        public IDataTransferObject GetDataForm () {
            return null;
        }

        public Widget GetWidget () {
            return componentView["table5"];
        }
    }
}

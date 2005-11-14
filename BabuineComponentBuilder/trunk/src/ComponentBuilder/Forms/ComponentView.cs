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
        
        FormView formView;
        MethodView methodView;
       
        ComponentDTO componentDTO;
        
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
            methodView = new MethodView ();
            methodView.LoadDataForm ((ComponentDTO) GetDataForm ());
            MethodDTO methodDTO = (MethodDTO) methodView.GetDataForm ();
            if (methodDTO != null) {
                methodTableModel.Add (methodDTO);
            }
            methodView = null;
        }

        private void OnDeleteMethodClicked (object sender, EventArgs args) {
            TreeIter iter;
            if (methodsTreeView.Selection.GetSelected (out iter)) {
                string methodName = methodTableModel.ListStore.GetValue (iter, 0).ToString ();
                MethodDTO auxMethodDTO = null;
                foreach (MethodDTO methodDTO in methodTableModel.ListModel) {
                    if (methodDTO.MethodName.Equals (methodName)) {
                        auxMethodDTO = methodDTO;
                        break;
                    }
                }
                if (auxMethodDTO != null) {
                    methodTableModel.ListModel.Remove (auxMethodDTO);
                    methodTableModel = new MethodTableModel (methodTableModel.ListModel);
                    methodsTreeView.Model = methodTableModel.ListStore;
                }
            }
        }

        private void OnClearMethodsClicked (object sender, EventArgs args) {
            methodTableModel.Clear ();
        }

        private void OnNewViewClicked (object sender, EventArgs args) {
            formView = new FormView ();
            ViewDTO viewDTO = (ViewDTO) formView.GetDataForm ();
            if (viewDTO != null) 
                viewTableModel.Add (viewDTO);
            formView = null;
        }

        private void OnDeleteViewClicked (object sender, EventArgs args) {
            TreeIter iter; 
            if (viewsTreeView.Selection.GetSelected (out iter)) {
                string viewName = viewTableModel.ListStore.GetValue (iter, 0).ToString ();
                ViewDTO auxViewDTO = null;
                foreach (ViewDTO viewDTO in viewTableModel.ListModel) {
                    if (viewDTO.ViewName.Equals (viewName)) {
                        auxViewDTO = viewDTO;
                        break;
                    }
                }
                //Siempre será != null
                if (auxViewDTO != null) {
                    viewTableModel.ListModel.Remove (auxViewDTO);
                    viewTableModel = new ViewTableModel (viewTableModel.ListModel);
                    viewsTreeView.Model = viewTableModel.ListStore;
                }
            }
        }

        private void OnClearViewsClicked (object sender, EventArgs args) {
            viewTableModel.Clear ();
        }
        
        /* Interface Implementation */
        public void LoadDataForm (IDataTransferObject dto) {
            if (dto is ComponentDTO) {
                ComponentDTO componentDTO = (ComponentDTO) dto; 
                this.componentDTO = componentDTO;
                componentNameEntry.Text = componentDTO.ComponentName;
                exceptionManagerEntry.Text = componentDTO.ClassExceptionManager;
                viewTableModel = new ViewTableModel (componentDTO.ViewCollection);
                viewsTreeView.Model = viewTableModel.ListStore;
                methodTableModel = new MethodTableModel (componentDTO.MethodCollection);
                methodsTreeView.Model = methodTableModel.ListStore;
            }
            else {
                //Lo ponemos en modo de creación de uno nuevo.
                this.componentDTO = null;
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
            if (ValidateForm ()) {
                //Hay una referencia asociada para este componente.
                if (this.componentDTO != null) {
                    //Ejecución del modo edición.
                    this.componentDTO.ComponentName = componentNameEntry.Text;
                    this.componentDTO.ClassExceptionManager = exceptionManagerEntry.Text;
                    this.componentDTO.ViewCollection = viewTableModel.ListModel;
                    this.componentDTO.MethodCollection = methodTableModel.ListModel;
                    //Recogemos y devolvemos los nuevos datos editados.
                    return componentDTO;
                }
                //No existe una referencia asociada al componentDTO.
                else {
                    //Creamos uno nuevo.
                    ComponentDTO componentDTO = new ComponentDTO ();
                    componentDTO.ComponentName = componentNameEntry.Text;
                    componentDTO.ClassExceptionManager = exceptionManagerEntry.Text;
                    componentDTO.ViewCollection = viewTableModel.ListModel;
                    componentDTO.MethodCollection = methodTableModel.ListModel;
                    return componentDTO;
                }
            }
            this.componentDTO = null;
            return null;
        }

        private void OnTestClicked (object sender, EventArgs args) {
            componentDTO = (ComponentDTO) GetDataForm ();
        }
        
        private bool ValidateForm () {
            //Diremos que un componente debe solo tener completo el nombre.
            if (componentNameEntry.Text.Length != 0) {
                return true;
            }
            return false;
        }

        public Widget GetWidget () {
            return componentView["table5"];
        }
    }
}

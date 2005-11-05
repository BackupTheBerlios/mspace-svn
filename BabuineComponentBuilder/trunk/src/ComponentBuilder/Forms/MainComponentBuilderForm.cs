using System;
using ComponentModel.Interfaces;
using ComponentModel.DTO;
using ComponentModel.Container;
using ComponentBuilder.DTO;
using ComponentBuilder.Forms.TableModel;
using Gtk;
using Glade;

namespace ComponentBuilder.Forms {
    public sealed class MainComponentBuilderForm : IViewHandler {
        Glade.XML gxml, newViewDialog, newMethodDialog, newParameterDialog, preferencesDialog, aboutDialog = null;
        [Widget] Statusbar statusbar1;
        [Widget] TreeView viewsTreeView, methodsTreeView;
        [Widget] Entry componentNameEntry, exceptionManagerClassNameEntry;
        
        ViewTableModel viewTableModel;
        MethodTableModel methodTableModel;
        ParameterTableModel parameterTableModel;
        
        /* Ctor */
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
        
        /*Helpers implementation*/


        public IDataTransferObject GetDataForm () {
            if (ValidateForm ()) {
                ComponentDTO componentDTO = new ComponentDTO ();                
                componentDTO.ComponentName = componentNameEntry.Text;
                componentDTO.ClassExceptionManager = exceptionManagerClassNameEntry.Text;
                componentDTO.ViewCollection = viewTableModel.ListModel;
                componentDTO.MethodCollection = methodTableModel.ListModel;
                return componentDTO;
            }
            return null;
        }
        
        private bool ValidateForm () {
            if (componentNameEntry.Text.Length == 0) {
                return false;
            }
            if (exceptionManagerClassNameEntry.Text.Length == 0) {
                return false;
            }
            return true;
        }

        public void LoadDataForm (IDataTransferObject dto) {
            if (dto is ComponentDTO) {
                ComponentDTO componentDTO = (ComponentDTO) dto;
            }
        }

        public void ClearForm () {
            componentNameEntry.Text = String.Empty;
            exceptionManagerClassNameEntry.Text = String.Empty;
            viewTableModel = new ViewTableModel ();
            viewsTreeView.Model = viewTableModel.ListStore;
            methodTableModel = new MethodTableModel ();
            methodsTreeView.Model = methodTableModel.ListStore;
        }

        /*Responses */
        
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

        public void ResponseGenerateComponent (ResponseMethodDTO response) {
            if (response.ExecutionSuccess) {
                Console.WriteLine ("Todo ha ido bien :)");
            }
        }

        public void ResponseSerializePreferences (ResponseMethodDTO response) {
            if (response.ExecutionSuccess) {
                Console.WriteLine ("Configuration saved.");
            }
        }

        public void ResponseDeserializePreferences (ResponseMethodDTO response) {
            if (response.ExecutionSuccess) {
                Console.WriteLine ("Configuracion guardada.");
            }
        }

        /*Gui Events*/
        
        private void OnWindow1DeleteEvent (object sender, DeleteEventArgs args) {
            Application.Quit ();
        }

        /*Menu Bar*/
        private void OnMenuNewActivate (object sender, EventArgs args) {
        }

        private void OnMenuOpenActivate (object sender, EventArgs args) {
        }

        private void OnMenuSaveActivate (object sender, EventArgs args) {
        }

        private void OnMenuSaveAsActivate (object sender, EventArgs args) {
        }

        private void OnMenuExitActivate (object sender, EventArgs args) {
            Application.Quit ();
        }
        
        private void OnMenuAboutActivate (object sender, EventArgs args) {
            aboutDialog = new Glade.XML (null, "MainComponentBuilderForm.glade", "aboutDialog", null);
            aboutDialog = null;
        }

        /*Toolbar*/

        private void OnNewComponentClicked (object sender, EventArgs args) {
            ClearForm ();
        }

        private void OnGenerateComponentClicked (object sender, EventArgs args) {
            ComponentDTO componentDTO = (ComponentDTO) GetDataForm (); 
            if (componentDTO != null) {
                DefaultContainer.Instance.Execute ("ComponentBuilder", "GenerateComponent", new object[]{componentDTO}, this);
            }
        }
        /*Views & Methods buttons*/
        
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
            //
            //Grrrr !! Puta mierda de TreeModels !!!!!
            ComboBox viewToResponseCombo = (ComboBox) newMethodDialog ["viewToResponseCombo"];
            TreeStore treeStore = new TreeStore (typeof (string));
            viewToResponseCombo.Model = treeStore;
            foreach (string viewName in viewTableModel.ListModel) {
                treeStore.AppendValues (viewName);
            }
            
            switch (dialog.Run ()) {
                case  (int) ResponseType.Ok:
                    Entry nameEntry = (Entry) newMethodDialog ["nameEntry"];
                    Entry returnTypeEntry = (Entry) newMethodDialog ["returnTypeEntry"];
                    //Entry viewToResponseEntry = (Entry) newMethodDialog ["viewToResponseEntry"];
                    Entry responseMethodEntry = (Entry) newMethodDialog ["responseMethodEntry"];
                    if (nameEntry.Text.Length != 0 && returnTypeEntry.Text.Length != 0 &&
                        responseMethodEntry.Text.Length != 0    
                        ) {
                        MethodDTO methodDTO = new MethodDTO ();
                        methodDTO.MethodName = nameEntry.Text;
                        methodDTO.ReturnType = returnTypeEntry.Text;
                        //methodDTO.ViewToResponse = viewToResponseCombo.ActiveText;
                        TreeIter iter;
                        viewToResponseCombo.GetActiveIter (out iter);
                        methodDTO.ViewToResponse = (string) viewToResponseCombo.Model.GetValue (iter, 0);
                        //
                        methodDTO.ResponseMethod = responseMethodEntry.Text;
                        methodDTO.ParametersCollection = parameterTableModel.ListModel;
                        methodTableModel.Add (methodDTO);
                    }
                    break;
                case (int) ResponseType.Cancel:
                    break;
                default:
                    break;
            }
            parameterTableModel = null;
            dialog.Destroy ();
            newMethodDialog = null;
        }

        private void OnBrowseButtonClicked (object sender, EventArgs args) {
            Entry defaultOutputPathEntry = (Entry) preferencesDialog ["defaultOutputPathEntry"]; 
            FileChooserDialog chooser = new FileChooserDialog ("Select a path:", (Window)preferencesDialog["preferencesDialog"], FileChooserAction.SelectFolder, Stock.Ok);
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

        private void OnPreferencesClicked (object sender, EventArgs args) {
            preferencesDialog = new Glade.XML (null, "MainComponentBuilderForm.glade", "preferencesDialog", null);
            preferencesDialog.Autoconnect (this);
            Dialog dialog = (Dialog) preferencesDialog  ["preferencesDialog"];
            Entry defaultOutputPathEntry = (Entry) preferencesDialog ["defaultOutputPathEntry"];
            Entry prefixNamespaceEntry = (Entry) preferencesDialog ["prefixNamespaceEntry"];
            CheckButton generateCheck = (CheckButton) preferencesDialog ["generateCheck"];
            IComponentModel componentModel = DefaultContainer.Instance.GetComponentByName ("ComponentBuilder");
            PreferencesDTO preferencesDTO = (PreferencesDTO) componentModel.GetProperty ("PreferencesDTO"); 
            // Vamos a cargar los datos.
            defaultOutputPathEntry.Text = preferencesDTO.OutputPath;
            prefixNamespaceEntry.Text = preferencesDTO.PrefixNamespace;
            generateCheck.Active = preferencesDTO.GenerateBuildfile;
            //
            switch (dialog.Run ()) {
                case (int) ResponseType.Ok:
                    preferencesDTO = new PreferencesDTO ();
                    preferencesDTO.OutputPath = defaultOutputPathEntry.Text;
                    preferencesDTO.PrefixNamespace = prefixNamespaceEntry.Text;
                    preferencesDTO.GenerateBuildfile = generateCheck.Active;
                    //Primero asignar la propiedad y luego salvar.
                    componentModel.SetProperty ("PreferencesDTO", preferencesDTO);
                    componentModel.Execute ("SerializePreferences", null, this);
                    break;
                case (int) ResponseType.Cancel:
                    break;
                default:
                    break;
            }
            dialog.Destroy ();
            preferencesDialog = null;
        }
    }
}

using System;
using ComponentModel.Container;
using ComponentModel.Interfaces;
using ComponentModel.DTO;
using ComponentBuilder.DTO;
using Gtk;
using Glade;

namespace ComponentBuilder.Forms {
    public sealed class MainComponentBuilderForm : IViewHandler {
        Glade.XML mainComponentBuilderForm;
        [Widget] HPaned hpaned1;
        [Widget] Statusbar statusbar1;
        
        ProjectView projectView;
        ComponentView componentView;
        
        public MainComponentBuilderForm () {
            mainComponentBuilderForm = new Glade.XML (null, "MainComponentBuilderForm.glade", "mainView", null);
            mainComponentBuilderForm.Autoconnect (this);

            //Terminamos de construir el gui.
            projectView = new ProjectView ();
            hpaned1.Pack1 (projectView.GetWidget (), true, true);
            componentView = new ComponentView ();
            hpaned1.Pack2 (componentView.GetWidget (), true, true);
            hpaned1.ShowAll ();
        }

        /* GUI Events */

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
            //Serialization.
            FileChooserDialog chooser = new FileChooserDialog ("Selecciona un fichero para guardar", (Window)mainComponentBuilderForm["mainView"], FileChooserAction.Save, Stock.Save);
            chooser.AddButton (Stock.Save, ResponseType.Accept);
            chooser.AddButton (Stock.Cancel, ResponseType.Cancel);
            chooser.SelectMultiple = false;
            ResponseType response = (ResponseType)chooser.Run ();
            if (response.Equals (ResponseType.Accept)) {
                if (chooser.Filename.Length != 0) {
                    DefaultContainer.Instance.Execute ("ComponentBuilder","SerializeProject", new object[]{(ProjectDTO)this.GetDataForm (), chooser.Filename}, this);
                }
            }
            chooser.Destroy ();
        }

        private void OnMenuExitActivate (object sender, EventArgs args) {
            Application.Quit ();
        }
        
        private void OnMenuAboutActivate (object sender, EventArgs args) {
        }

        /*Toolbar*/

        private void OnNewProjectClicked (object sender, EventArgs args) {
            ClearForm ();
            ProjectDTO projectDTO = new ProjectDTO ();
            projectDTO.ProjectName = "New Project";
            LoadDataForm (projectDTO);
        }
        
        private void OnAddComponentClicked (object sender, EventArgs args) {
            ProjectDTO projectDTO = (ProjectDTO) projectView.GetDataForm ();
            ComponentDTO componentDTO = new ComponentDTO ();
            componentDTO.ComponentName = "New Component";
            projectDTO.ComponentCollection.Add (componentDTO);
            //Hago el refresco aquí, explícito.
            projectView.LoadDataForm (componentDTO);
            LoadDataForm (componentDTO);
        }

        private void OnGenerateComponentClicked (object sender, EventArgs args) {
        }

        private void OnPreferencesClicked (object sender, EventArgs args) {
        }        
        
        /* Response Notifications */

        public void ResponseShowForm (ResponseMethodDTO response) {
            if (response.ExecutionSuccess) {
                response.MethodResult = mainComponentBuilderForm ["vbox1"];
                statusbar1.Push (0, String.Format ("Welcome to Babuine Component Builder: {0}@{1}", Environment.UserName, Environment.MachineName));
            }
        }

        public void ResponseSerializeProject (ResponseMethodDTO response) {
            if (response.ExecutionSuccess) {
                Console.WriteLine ("Project serialized");
            }
        }
        
        /* Interface Implementation */
        public void LoadDataForm (IDataTransferObject dto) {
            if (dto is ProjectDTO) {
                projectView.LoadDataForm (dto);
            }
            else if (dto is ComponentDTO) {
                componentView.LoadDataForm (dto);
            }
        }

        public void ClearForm () {
            projectView.ClearForm ();
            componentView.ClearForm ();
        }

        public IDataTransferObject GetDataForm () {
            return projectView.GetDataForm ();
        }
    }
}

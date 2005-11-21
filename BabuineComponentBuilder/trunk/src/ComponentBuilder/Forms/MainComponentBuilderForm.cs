using System;
using ComponentModel.Container;
using ComponentModel.Interfaces;
using ComponentModel.DTO;
using ComponentBuilder.DTO;
using ComponentBuilder.Interfaces;
using Gtk;
using Glade;

namespace ComponentBuilder.Forms {
    public sealed class MainComponentBuilderForm : IViewHandler {
        Glade.XML mainComponentBuilderForm;
        [Widget] HPaned hpaned1;
        [Widget] Statusbar statusbar1;
        
        ProjectView projectView;
        ComponentView componentView;
        PreferencesView preferencesView;
        
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
            FileChooserDialog chooser = new FileChooserDialog ("Selecciona un fichero para abrir", (Window)mainComponentBuilderForm["mainView"], FileChooserAction.Open, Stock.Open);
            chooser.AddButton (Stock.Open, ResponseType.Accept);
            chooser.AddButton (Stock.Cancel, ResponseType.Cancel);
            chooser.SelectMultiple = false;
            ResponseType response = (ResponseType) chooser.Run ();
            switch (response) {
                case ResponseType.Accept:
                    if (chooser.Filename.Length != 0) {
                        DefaultContainer.Instance.Execute ("ComponentBuilder", "DeserializeProject", new object[]{chooser.Filename}, this);
                    }
                    break;
                case ResponseType.Cancel:
                    break;
                default:
                    break;
            }
            chooser.Destroy ();
            chooser = null;
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
            chooser = null;
        }

        private void OnMenuExitActivate (object sender, EventArgs args) {
            Application.Quit ();
        }
        
        private void OnMenuAboutActivate (object sender, EventArgs args) {
            Glade.XML aboutForm = new Glade.XML (null, "MainComponentBuilderForm.glade", "aboutDialog", null);
            //AboutDialog no existe todavia en GTK#.
            //Gtk.AboutDialog aboutDialog = (Gtk.AboutDialog) aboutForm ["aboutDialog"];
            //aboutDialog.Logo = Gdk.PixBuf.LoadFromResource ("mono-powered.png");
            //aboutDialog.Run ();
            //aboutDialog.Destroy ();
            //aboutDialog = null;
            aboutForm = null;
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
            if (projectDTO != null) {
                //Con esta linea decimos que vamos a añadir uno nuevo.
                //OJO AQUI!!
                componentView.ActionState = ActionState.Create;
                ComponentDTO componentDTO = (ComponentDTO) componentView.GetDataForm ();
                if (componentDTO != null) {
                    projectDTO.ComponentCollection.Add (componentDTO);
                    LoadDataForm (projectDTO);
                    componentView.ClearForm ();
                }
            }
        }

        private void OnDeleteComponentClicked (object sender, EventArgs args) {
            ComponentDTO componentDTO = (ComponentDTO) componentView.GetDataForm ();
            if (componentDTO != null) {
                ProjectDTO projectDTO = (ProjectDTO) projectView.GetDataForm ();
                if (projectDTO != null) {
                    foreach (ComponentDTO auxComponentDTO in projectDTO.ComponentCollection) {
                        if (componentDTO.ComponentName.Equals (auxComponentDTO.ComponentName)) {
                            //Igual se deberian añadir más condiciones
                            componentDTO = auxComponentDTO;
                            break;
                        }
                    }
                }
                projectDTO.ComponentCollection.Remove (componentDTO);
                LoadDataForm (projectDTO);
                componentView.ClearForm ();
            }
        }

        private void OnGenerateComponentClicked (object sender, EventArgs args) {
        }

        private void OnPreferencesClicked (object sender, EventArgs args) {
            preferencesView = new PreferencesView ();
            IComponentModel componentModel = DefaultContainer.Instance["ComponentBuilder"];
            preferencesView.LoadDataForm ((IDataTransferObject) componentModel.GetProperty ("PreferencesDTO"));
            PreferencesDTO preferencesDTO = (PreferencesDTO) preferencesView.GetDataForm ();
            if (preferencesDTO != null) {
                componentModel.SetProperty ("PreferencesDTO", preferencesDTO);
                componentModel.Execute ("SerializePreferences", null, this);
            }
            preferencesView = null;
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
        
        public void ResponseDeserializeProject (ResponseMethodDTO response) {
            if (response.ExecutionSuccess) {
                ProjectDTO projectDTO = (ProjectDTO) response.MethodResult;
                LoadDataForm (projectDTO);
            }
        }

        public void ResponseSerializePreferences (ResponseMethodDTO response) {
            if (response.ExecutionSuccess) {
                Console.WriteLine ("Serialized settings.");
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

using System;
using ComponentModel.Interfaces;
using ComponentModel.DTO;
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
        }

        private void OnMenuExitActivate (object sender, EventArgs args) {
            Application.Quit ();
        }
        
        private void OnMenuAboutActivate (object sender, EventArgs args) {
        }

        /*Toolbar*/

        private void OnNewComponentClicked (object sender, EventArgs args) {
            ClearForm ();
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
        
        /* Interface Implementation */
        public void LoadDataForm (IDataTransferObject dto) {
        }

        public void ClearForm () {
        }

        public IDataTransferObject GetDataForm () {
            return null;
        }
    }
}

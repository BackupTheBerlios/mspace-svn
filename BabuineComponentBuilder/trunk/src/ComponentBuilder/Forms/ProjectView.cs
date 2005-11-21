using System;
using ComponentModel.Container;
using ComponentModel.Interfaces;
using ComponentBuilder.Interfaces;
using ComponentBuilder.Forms.NodeModel;
using ComponentBuilder.DTO;
using Gtk;

namespace ComponentBuilder.Forms {
    internal class ProjectView : IViewHandler, IGtkView {
        
        /*Contenenedor*/
        ScrolledWindow componentScrolledWindow;
        NodeView componentNodeView;
        NodeStore componentNodeStore;
        /*Model*/ 
        ProjectDTO projectDTO;

        internal ProjectView () {
            componentNodeStore = new NodeStore (typeof (GenericNode));
            componentNodeView = new NodeView (componentNodeStore);
            componentNodeView.AppendColumn ("Project Tree", new CellRendererText (),"text", 0);
            componentScrolledWindow = new ScrolledWindow ();
            componentScrolledWindow.Add (componentNodeView);
            componentNodeView.NodeSelection.Mode = SelectionMode.Single;
            componentNodeView.NodeSelection.Changed += new EventHandler (OnSelectionChanged);
        }

        /* Interface Implementation */

        /*
         * Cargará el proyecto.
         * Coge el ProjectDTO que le pasamos y asignamos la referencia al modelo
         * del projectDTO.  Además con ese nuevo DTO crea un nuevo Nodo y
         * rellenará también sus componentes.
         */
        public void LoadDataForm (IDataTransferObject dto) {
            if (dto is ProjectDTO) {
                //Refresca el proyecto.
                ProjectDTO projectDTO = (ProjectDTO) dto;
                this.projectDTO = projectDTO;
                ClearForm ();
                componentNodeStore.AddNode (new ProjectNode (this.projectDTO));
            }
            componentNodeView.ExpandAll ();
        }

        /*
         * Limpia el formulario.
         */
        public void ClearForm () {
            componentNodeStore.Clear ();
        }

        /*
         * Retornará el ProjectDTO del modelo de datos.
         */
        public IDataTransferObject GetDataForm () {
            if (componentNodeStore.GetNode (TreePath.NewFirst ()) != null) 
                return ((GenericNode) componentNodeStore.GetNode (TreePath.NewFirst ())).DataTransferObject;
            else 
                return null;
        }

        public Widget GetWidget () {
            return componentScrolledWindow;
        }

        private void OnSelectionChanged (object sender, EventArgs args) {
            if (sender is NodeSelection) {
                NodeSelection nodeSelection = (NodeSelection) sender;
                if (nodeSelection.SelectedNode is ComponentNode) {
                    ComponentNode componentNode = (ComponentNode) nodeSelection.SelectedNode;
                    ComponentDTO componentDTO = (ComponentDTO) componentNode.DataTransferObject;
                    IComponentModel componentModel = DefaultContainer.Instance["ComponentBuilder"];
                    componentModel.ViewHandlerCollection[0].LoadDataForm (componentDTO);
                }
            }
        }
    }
}

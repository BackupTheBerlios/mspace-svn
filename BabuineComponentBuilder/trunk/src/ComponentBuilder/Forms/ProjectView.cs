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
        
        internal ProjectView () {
            componentNodeStore = new NodeStore (typeof (GenericNode));
            componentNodeView = new NodeView (componentNodeStore);
            componentNodeView.AppendColumn ("Project Tree", new CellRendererText (),"text", 0);
            componentScrolledWindow = new ScrolledWindow ();
            componentScrolledWindow.Add (componentNodeView);

            //Event Handling
            componentNodeView.NodeSelection.Mode = SelectionMode.Single;
            componentNodeView.NodeSelection.Changed += new EventHandler (OnSelectionChanged);
            
        }
        
        /* Interface Implementation */
        public void LoadDataForm (IDataTransferObject dto) {
            if (dto is ProjectDTO) {
                ProjectDTO projectDTO = (ProjectDTO) dto;
                ClearForm ();
                componentNodeStore.AddNode (new ProjectNode (projectDTO));
            }
            else if (dto is ComponentDTO) {
                ComponentDTO componentDTO = (ComponentDTO) dto;
                ProjectNode projectNode = (ProjectNode) componentNodeStore.GetNode (TreePath.NewFirst ());
                projectNode.AddChild (new ComponentNode (componentDTO)); 
            }
            componentNodeView.ExpandAll ();
        }

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
                    //Ahora truco de gestion de vista.
                    IComponentModel componentModel = DefaultContainer.Instance.GetComponentByName ("ComponentBuilder");
                    //Responderá a la vista y ejecutará el loadDataForm de
                    //MainComponentForm
                    componentModel.ViewHandlerCollection[0].LoadDataForm (componentDTO);
                }
            }
        }
    }
}

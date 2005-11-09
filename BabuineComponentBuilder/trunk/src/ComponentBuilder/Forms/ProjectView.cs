using System;
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
        }

        public void ClearForm () {
            componentNodeStore.Clear ();
        }

        public IDataTransferObject GetDataForm () {
            return ((GenericNode)componentNodeStore.GetNode (TreePath.NewFirst ())).DataTransferObject;
        }

        public Widget GetWidget () {
            return componentScrolledWindow;
        }
    }
}

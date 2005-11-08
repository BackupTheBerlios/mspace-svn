using System;
using ComponentModel.Interfaces;
using ComponentBuilder.Interfaces;
using ComponentBuilder.Forms.NodeModel;
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
        }

        public void ClearForm () {
        }

        public IDataTransferObject GetDataForm () {
            return null;
        }

        public Widget GetWidget () {
            return componentScrolledWindow;
        }
    }
}

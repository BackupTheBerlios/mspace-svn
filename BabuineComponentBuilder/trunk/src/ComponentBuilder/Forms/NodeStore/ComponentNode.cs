using System;
using System.Collections;
using System.Text;
using ComponentModel.Interfaces;
using ComponentBuilder.DTO;
using Gtk;

namespace ComponentBuilder.Forms.NodeModel {
    /*
     *
     * Vamos a usar NodeStores para ver que tal van.
     *
     */
    //[TreeNode (ColumnCount = 1)]
    public sealed class ComponentNode : GenericNode {

        public ComponentNode () : base () {}

        public ComponentNode (ComponentDTO componentDTO) : base((IDataTransferObject) componentDTO) {
        }

        //[TreeNodeValue (Column = 0)]
        public override string Value {
            get {return ((ComponentDTO)DataTransferObject).ComponentName;}
            set {((ComponentDTO)DataTransferObject).ComponentName = value;}
        }
        
        
    }
}

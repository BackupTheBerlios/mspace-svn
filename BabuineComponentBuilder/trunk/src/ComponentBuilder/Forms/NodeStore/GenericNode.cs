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
    [TreeNode (ColumnCount = 1)]
    public abstract class GenericNode : TreeNode {
        IDataTransferObject dto;
            
        public GenericNode () {}

        public GenericNode (IDataTransferObject dto) {
            this.dto = dto;
        }
        
        public IDataTransferObject DataTransferObject {
            get {return dto;}
            set {dto = value;}
        }

        [TreeNodeValue (Column = 0)]
        public abstract string Value {get; set;}
        
    }
}

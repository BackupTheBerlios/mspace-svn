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
    public sealed class ProjectNode : GenericNode {

        public ProjectNode () : base () {}

        public ProjectNode (ProjectDTO projectDTO) : base ((IDataTransferObject) projectDTO){
            foreach (ComponentDTO componentDTO in projectDTO.ComponentCollection) {
                this.AddChild (new ComponentNode (componentDTO));
            }
        }
        

        //[TreeNodeValue (Column = 0)]
        public override string Value {
            get {return ((ProjectDTO)DataTransferObject).ProjectName;}
            set {((ProjectDTO)DataTransferObject).ProjectName = value;}
        }
        
        
    }
}

/*using System.Collections;
using org.vim.Components.VimControl.Model;

namespace org.vim.Components.BufferedVimControl.Model {
    //
    //<summary
    //  This interface represents the main component in the Application.
    //  It's a top level component. It manage the list of pairs of editor &
    //  command line.  And with it, we can manage the buffers, and access to a
    //  content of each buffer.
    //
    //  The implementation reference is written with GTK#, but the main idea is
    //  use other data structures in order to decouple the application and, for
    //  example; write easily the component with QT#, only implementing this
    //  interface.
    //  
    //  This is thinked by Extensibility.
    //</summary>
    //
    public interface IBufferedVimControl {
        
        //
        //<summary>
        // This is a List of Buffers.
        //</summary>
        //
        IList ControlList {get; set;}
        
        //
        //<summary>
        // Accessor for each IVimControl component.
        //</summary>
        //
        IVimControl this [int index] {get;}

        //
        //<summary>
        //Returns number of Buffers that this component contains.
        //</summary>
        //
        int Count {get;}
    
        //
        //<summary>
        //Returns active control.
        //</summary>
        //
        int Active {get;}
            
        //
        //<summary>
        //Goes to next VimControl.
        //</summary>
        //
        IVimControl NextControl ();

        //
        //<summary>
        //Goes to previous VimControl
        //</summary>
        //
        IVimControl PreviousControl ();
       
        //
        //<summary>
        //Appends a Control.
        //</summary>
        //
        void Append (IVimControl vimControl);

        //
        //<summary>
        //Delete a Control.
        //</summary>
        //
        void Delete (IVimControl vimControl);
    }
}
*/

/**using System;
using org.vim.Components.VimBuffer.Model;
using org.vim.Components.VimEntry.Model;

namespace org.vim.Components.VimControl.Model {

    //
    //<summary>
    //  Handler for ChangeMode event.
    //</summary>
    //
    public delegate void ChangeModeHandler (IVimControl sender);
    
    //
    //<summary>
    //  This enum, represent states which Control could be.  Command and
    //  edition, probabily we add another state more.
    //</summary>
    //
    public enum Mode {
        Command,
        Edition
    }
    
    //
    //<summary>
    //  This interface represents a VimControl; it's constructed by a union of
    //  the TextView component, and the Entry component.  The entry, will be act
    //  as command line, and textView manage the model to edit.
    //</summary>
    //
    public interface IVimControl {
        //
        //<summary>
        //      This event represents when the mode is Changed.
        //</summary>
        //
        event ChangeModeHandler ChangeMode;
       
        //
        //<summary>
        //      Control also get information for the file which will be edited.
        //</summary>
        //
        string FileUri {get; set;}

        //
        //<summary>
        //      State of this control.
        //</summary
        //
        Mode Mode {get; set;}

        //
        //<summary>
        //      Accessor to the IVimTextView.
        //</summary>
        //
        IVimBuffer VimBuffer {get; set;}


        //
        //<summary>
        //      Accessor to little command line.
        //</summary>
        //
        IVimEntry VimEntry {get; set;}
        
        //
        //<summary>
        //      Method to Change to command mode.
        //</summary>
        //
        void ChangeToCommand ();
        
        //
        //<summary>
        //      Method to Change to edition mode.
        //</summary>
        //
        void ChangeToEdition ();
    }
}
*/

namespace Vim.Components.VimEntry.Model {
//    public delegate void CommandActivatedHandler (IVimEntryHandler sender);
    public interface IVimEntryHandler {
//        event CommandActivatedHandler CommandActivate; 
        string Command {get; set;}
       
    }
}

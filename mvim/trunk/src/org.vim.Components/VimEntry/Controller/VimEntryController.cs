using org.vim.Components.VimEntry.Model;
using org.vim.Components.VimEntry.View;

namespace org.vim.Components.VimEntry.Controller {
    public class VimEntryController : IVimEntryController {
        
        private IVimEntryHandler vimEntryHandler;
        private IVimEntryView vimEntryView;
            
        public VimEntryController (IVimEntryHandler vimEntryHandler, IVimEntryView vimEntryView) {
            this.vimEntryHandler = vimEntryHandler;
            this.vimEntryView = vimEntryView;
        }

        public void Execute (string text) {
            vimEntryHandler.Command = text; 
        }
    }
}

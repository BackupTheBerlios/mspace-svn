using Vim.Components.VimEntry.Model;
using Vim.Components.VimEntry.View;

namespace Vim.Components.VimEntry.Controller {
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

using Vim.Components.VimEntry.Model;
using Gtk;


namespace Vim.Components.VimEntry.View {
    public class VimEntryView : Gtk.Entry, IVimEntryView {
        IVimEntryHandler vimEntryHandler;
        //IVimEntryController vimEntryController;

        public VimEntryView () {
            vimEntryHandler = new VimEntryHandler ();
        }

        protected override void OnActivated () {
               base.OnActivated ();
        }
    }
}

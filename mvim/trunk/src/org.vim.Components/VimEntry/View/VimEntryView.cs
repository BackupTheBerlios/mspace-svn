using org.vim.Components.VimEntry.Model;
using Gtk;


namespace org.vim.Components.VimEntry.View {
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

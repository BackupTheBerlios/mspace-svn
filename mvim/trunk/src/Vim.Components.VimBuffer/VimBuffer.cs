namespace Vim.Components.VimBuffer {

    public class VimBuffer : IVimBuffer {
        private string text;

        public string Text {
            get {return text;}
            set {text = value;}
        }
    }
}

namespace Vim.Components.VimBuffer.Model {

    public class VimBuffer : IVimBuffer {
        private string text;

        public string Text {
            get {return text;}
            set {text = value;}
        }
    }
}

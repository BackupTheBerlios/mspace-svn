using System;

namespace Vim.Components.VimBuffer.Model.VimBufferStrategy {
    public class StringVimBufferStrategy : IVimBufferStrategy {
        private string text;
   
        public StringVimBufferStrategy () {
        }
        
        public int Length {
            get {return text.Length;}
        }

        public char this [int offset] {
            get {return text[offset];}
        }

        public void Insert (int offset, string text) {
            this.text = this.text.Insert (offset, text);
        }

        public void Remove (int offset, int length) {
            text = text.Remove (offset, length);
        }

        public void Replace (int offset, int length, string text) {
            Remove (offset, length);
            Insert (offset, text);
        }

        public string GetText (int offset, int length) {
            return text.Substring (offset, Math.Min (length, text.Length - offset));
        }

        public void SetContent (string text) {
            this.text = text;
        }
    }
}

//
// Vim.Components.VimBuffer.Model.VimBufferStrategy.StringVimBufferStrategy.cs :
//      A implementation of IVimBufferStrategy with a string.
//
// Author:
//      Néstor Salceda Alonso (wizito@gentelibre.org)
// (C) 2005
// 
using System;

namespace Vim.Components.VimBuffer {
    public class StringVimBufferStrategy : IVimBufferStrategy {
        private string text;
   
        public StringVimBufferStrategy () {
        }
        
        public int Length {
            get {return text.Length;}
        }

        public char this [int offset] {
            get {
                if (offset == text.Length)
                    return '\0'; 
                return text[offset];
            }
        }

        public void Insert (int offset, string text) {
            if (text != null) 
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
            if (length == 0)
                return String.Empty;
            return text.Substring (offset, Math.Min (length, text.Length - offset));
        }

        public void SetContent (string text) {
            this.text = text;
        }
    }
}

//
// Vim.Components.VimBuffer.Model.VimBufferStrategy.IVimBufferStrategy.cs :  
//
// Author:
//      Néstor Salceda Alonso (wizito@gentelibre.org)
// (C) 2005

namespace Vim.Components.VimBuffer {
    /**
     *  Basic strategy interface to operate with buffer implementations.
     */
    public interface IVimBufferStrategy {
        /**
         * Returns length of the buffer.
         */
        int Length {get;}

        /**
         * Returns char specified at offset.
         */
        char this [int offset] {get;}
       
        /**
         * Insert text in offset position.
         */
        void Insert (int offset, string text);

        /**
         * Remove length characters, to offset position.
         */
        void Remove (int offset, int length);

        /**
         * Replace position offset and length to text.
         */
        void Replace (int offset,int length, string text);

        /**
         * Get buffer or a buffer subset from offset & length.
         */
        string GetText (int offset, int length);

        /**
         * Sets buffer content.
         */
        void SetContent (string text);
    }
}


namespace Vim.Components.VimBuffer.Model.VimBufferStrategy {
    public interface IVimBufferStrategy {
        int Length {get;}
        char this [int offset] {get;}
        
        void Insert (int offset, string text);
        void Remove (int offset, int length);
        void Replace (int offset,int length, string text);
        string GetText (int offset, int length);
        void SetContent (string text);
    }
}

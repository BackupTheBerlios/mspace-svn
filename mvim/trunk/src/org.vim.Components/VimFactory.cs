using log4net;
using log4net.Config;

namespace org.vim.Components {
    public class VimFactory {
        private static VimFactory vimFactory;
        
        private VimFactory () {
        
        }

        public static VimFactory Instance {
            get {
                if (vimFactory == null)
                    vimFactory = new VimFactory ();
                return vimFactory;
            }
        }
    }
}

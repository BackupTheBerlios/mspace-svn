/**using org.vim.Components.VimControl.Model;
using System.Collections;
using log4net;
using log4net.Config;

namespace Vim.Components.BufferedVimControl.Model {
    public class BufferedVimControl : IBufferedVimControl {
        IList controlList;
        ILog log;
        
        public BufferedVimControl () {
            log = LogManager.GetLogger (this.GetType ());
            DOMConfigurator.Configure ();
            
            controlList = new ArrayList ();
            
            log.Debug ("Init new BufferedVimControl");
        }
        
        //INherited methods.
        public IList ControlList {
            get {return controlList;}
            set {controlList = value;}
        }
        
        public IVimControl this [int index] {
            get {
                return (IVimControl)controlList[index];
            }
        }
        
        public int Count {
            get {
                return controlList.Count;
            }
        }

        public int Active {
            get {
                return 0;
            }
        }

        public IVimControl NextControl () {
            return null;
        }

        public IVimControl PreviousControl () {
            return null;
        }

        public void Append(IVimControl vimControl) {
            log.Debug ("Appending a new VimControl.");
        }
        
        public void Delete (IVimControl vimControl) {
            log.Debug ("Deleting an existent VimControl");
        }
    }
}
*/

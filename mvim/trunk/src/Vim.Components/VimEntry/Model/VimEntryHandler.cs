using System;
using log4net;
using log4net.Config;

namespace Vim.Components.VimEntry.Model {
    public class VimEntryHandler : IVimEntryHandler {
        
        private string command;
//        public event CommandActivatedHandler CommandActivate; 
        
        //Logging Utility
        private ILog log;
        
        public VimEntryHandler () {
            DOMConfigurator.Configure ();
            log = LogManager.GetLogger (this.GetType ());
            command = String.Empty;
            log.Debug ("Init a new VimEntry.");
        }
        
        public string Command {
            get {return command;}
            set {
                command = value;
        //        NotifyCommandActivate ();
            }
        }
    
/**     private void NotifyCommandActivate () {
           if (CommandActivate != null)
            this.CommandActivate (this);
            log.Debug ("Notifing that command has been Activated.");
        }
*/
    }
}

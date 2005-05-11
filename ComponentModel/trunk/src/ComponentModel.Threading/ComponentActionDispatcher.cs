using System;
using System.Threading;

namespace ComponentModel.Threading {
    internal class ComponentActionDispatcher  {
        /*Cojo los campos para la información de ponerlo en segundo plano*/
        private Thread thread;
        private ThreadStart threadStart;
        /*Recolecto datos para la ejecucución.*/
        
        internal ComponentActionDispatcher () {
            
            threadStart += new ThreadStart (Run);            
            thread = new Thread (threadStart);
            thread.Start ();
        }

        internal void Run () {
        }
    }
}

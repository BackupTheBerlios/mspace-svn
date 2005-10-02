using System;
using System.Collections;
using ComponentModel.Interfaces;

namespace ComponentModel.Collections {
    public class IViewHandlerEnumerator : IEnumerator {
        private IEnumerator enumerator;

        internal IViewHandlerEnumerator (IViewHandlerCollection collection) {
            this.enumerator = ((IEnumerable)collection).GetEnumerator ();
        }

        public IViewHandler Current {
            get {return (IViewHandler) enumerator.Current;}
        }

        public bool MoveNext () {
            return enumerator.MoveNext ();
        }

        public void Reset () {
            enumerator.Reset ();
        }

        object IEnumerator.Current {
            get {return Current;}
        }

        bool IEnumerator.MoveNext () {
            return MoveNext ();
        }

        void IEnumerator.Reset () {
            Reset ();
        }

    }
}

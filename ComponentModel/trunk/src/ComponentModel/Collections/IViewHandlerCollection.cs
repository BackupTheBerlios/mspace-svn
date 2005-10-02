using System;
using System.Collections;
using ComponentModel.Interfaces;

namespace ComponentModel.Collections {
    //Without generics support; this is the way to implement a collection which
    //only support IViewHandler interfaces.
    public sealed class IViewHandlerCollection : IList, IEnumerable, ICollection {
        private ArrayList arrayList;

        public IViewHandlerCollection () {
            arrayList = new ArrayList ();
        }
        
        public IViewHandler this [int index] {
            get {return (IViewHandler) arrayList [index];}
            set {arrayList [index] = value;}
        }
        
        public int Count {
            get {return arrayList.Count;}
        }

        bool IList.IsReadOnly {
            get {return arrayList.IsReadOnly;}
        }

        bool IList.IsFixedSize {
            get {return arrayList.IsFixedSize;}
        }
        
        public int Add (IViewHandler value) {
            return arrayList.Add (value);
        }

        public void AddRange (IViewHandler[] value) {
            if (value == null) {
                throw new ArgumentNullException ("Value null");
            }
            arrayList.AddRange (value);
        }

        public void Clear () {
            arrayList.Clear ();
        }

        public bool Contains (IViewHandler value) {
            return arrayList.Contains (value);
        }

        public void CopyTo (IViewHandler[] array, int index) {
            arrayList.CopyTo (array, index);
        }


        //IViewHandler enumerator please.
        public IViewHandlerEnumerator GetEnumerator () {
            return new IViewHandlerEnumerator (this);
        }

        public int IndexOf (IViewHandler value) {
            return arrayList.IndexOf (value);
        }

        public void Insert (int index, IViewHandler value) {
            arrayList.Insert (index, value);
        }

        public bool IsReadOnly {
            get {return arrayList.IsReadOnly;}
        }

        public bool IsSynchronized {
            get {return arrayList.IsSynchronized;}
        }

        public void Remove (IViewHandler value) {
            arrayList.Remove (value);
        }

        public void RemoveAt (int index) {
            arrayList.RemoveAt (index);
        }

        public object SyncRoot {
            get {return this;}
        }

        object IList.this [int index] {
            get {return this [index];}
            set {this[index] = (IViewHandler) value;}
        }

        int IList.Add (object value) {
            return Add ((IViewHandler) value);
        }

        bool IList.Contains (object value) {
            return Contains ((IViewHandler) value);
        }

        int IList.IndexOf (object value) {
            return IndexOf ((IViewHandler) value);
        }

        void IList.Insert (int index, object value) {
            Insert (index, (IViewHandler) value);
        }

        void IList.Remove (object value) {
            Remove ((IViewHandler) value);
        }

        void ICollection.CopyTo (Array array, int index) {
            arrayList.CopyTo (array, index);
        }

        IEnumerator IEnumerable.GetEnumerator () {
            return arrayList.GetEnumerator ();
        }
    }
}

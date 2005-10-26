using System;
using System.Collections;
using System.Collections.Specialized;
using ComponentModel.Interfaces;
using ComponentBuilder.DTO;
using ComponentBuilder.Interfaces;
using Gtk;

namespace ComponentBuilder.Forms.TableModel {
    public sealed class ViewTableModel : ITableModel {
        private ListStore listStore; 
        private Type[] tipos;
        private IList list;

        public ViewTableModel () {
            list = new ArrayList ();
            tipos = new Type[1];
            for (int i = 0; i < tipos.Length; i++) {
                tipos[i] = typeof (string);
            }
            listStore = new ListStore (tipos);
        }

        public ViewTableModel (IList list) {
            foreach (string view in list) {
                this.Add (view);
            }
        }

        public void Add (string view) {
            listStore.AppendValues (view);
            list.Add (view);
        }
        
        public void Add (IDataTransferObject dto) {
            //Ojo aqui !
        }

        public void Clear () {
            listStore.Clear ();
            list.Clear ();
        }
        
        public ListStore ListStore {
            get {return listStore;}
        }

        public StringCollection ListModel {
            get {
                StringCollection stringCollection = new StringCollection ();
                foreach (string view in list) {
                    stringCollection.Add (view);
                }
                return stringCollection;
            }
        }
        
        IList ITableModel.ListModel {
            get {return list;}
        }

    }
}
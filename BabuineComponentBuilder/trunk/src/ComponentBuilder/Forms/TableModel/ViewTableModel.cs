using System;
using System.Collections;
using ComponentModel.Interfaces;
using ComponentBuilder.DTO;
using ComponentBuilder.Interfaces;
using Gtk;

namespace ComponentBuilder.Forms.TableModel {
    public sealed class ViewTableModel : ITableModel {
        private ListStore listStore; 
        private Type[] tipos;

        public ViewTableModel () {
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
        }
        
        public void Add (IDataTransferObject dto) {
            //Ojo aqui !
        }

        public void Clear () {
            listStore.Clear ();
        }
        
        public ListStore ListStore {
            get {return listStore;}
        }

    }
}

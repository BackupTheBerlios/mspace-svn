using System;
using System.Collections;
using ComponentModel.Interfaces;
using ComponentBuilder.DTO;
using ComponentBuilder.Interfaces;
using Gtk;

namespace ComponentBuilder.Forms.TableModel {
    public sealed class MethodTableModel : ITableModel {
        private ListStore listStore; 
        private Type[] tipos;

        public MethodTableModel () {
            tipos = new Type[5];
            for (int i = 0; i < tipos.Length; i++) {
                tipos[i] = typeof (string);
            }
            listStore = new ListStore (tipos);
        }

        public MethodTableModel (IList list) {
            foreach (MethodDTO methodDTO in list) {
                this.Add (methodDTO);
            }
        }

        public void Add (IDataTransferObject dto) {
            if (dto is MethodDTO) {
                MethodDTO methodDTO = (MethodDTO) dto;
                listStore.AppendValues (
                        methodDTO.MethodName,
                        methodDTO.ReturnType,
                        methodDTO.ViewToResponse,
                        methodDTO.ResponseMethod,
                        methodDTO.ParametersCollection.ToString ()
                        );
            }
        }

        public void Clear () {
            listStore.Clear ();
        }
        
        public ListStore ListStore {
            get {return listStore;}
        }

    }
}

using System;
using System.Collections;
using ComponentModel.Interfaces;
using ComponentBuilder.DTO;
using ComponentBuilder.Interfaces;
using Gtk;

namespace ComponentBuilder.Forms.TableModel {
    public sealed class ParameterTableModel : ITableModel {
        private ListStore listStore; 
        private Type[] tipos;
        private IList list;

        public ParameterTableModel () {
            tipos = new Type[2];
            for (int i = 0; i < tipos.Length; i++) {
                tipos[i] = typeof (string);
            }
            listStore = new ListStore (tipos);
            list = new ArrayList ();
        }

        public ParameterTableModel (IList list) {
            foreach (ParameterDTO parameterDTO in list) {
                this.Add (parameterDTO);
            }
        }

        public void Add (IDataTransferObject dto) {
            if (dto is ParameterDTO) {
                ParameterDTO parameterDTO = (ParameterDTO) dto;
                listStore.AppendValues (
                        parameterDTO.TypeName,
                        parameterDTO.VarName
                        );
                list.Add (parameterDTO);
            }
        }

        public void Clear () {
            listStore.Clear ();
            list.Clear ();
        }
        
        public ListStore ListStore {
            get {return listStore;}
        }

        public IList ListModel {
            get {return list;}
        }

    }
}

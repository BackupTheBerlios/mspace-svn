using System;
using System.Collections;
using System.Text;
using ComponentModel.Interfaces;
using ComponentBuilder.DTO;
using ComponentBuilder.Interfaces;
using Gtk;

namespace ComponentBuilder.Forms.TableModel {
    public sealed class MethodTableModel : ITableModel {
        private ListStore listStore; 
        private Type[] tipos;
        private IList list;

        public MethodTableModel () {
            tipos = new Type[5];
            for (int i = 0; i < tipos.Length; i++) {
                tipos[i] = typeof (string);
            }
            listStore = new ListStore (tipos);
            list = new ArrayList ();
        }

        public MethodTableModel (IList list) : this () {
            foreach (MethodDTO methodDTO in list) {
                this.Add (methodDTO);
            }
        }

        public void Add (IDataTransferObject dto) {
            if (dto is MethodDTO) {
                MethodDTO methodDTO = (MethodDTO) dto;
                StringBuilder stringBuilder = new StringBuilder ();
                foreach (ParameterDTO parameterDTO in methodDTO.ParametersCollection) {
                    stringBuilder = stringBuilder.Append (parameterDTO.TypeName);
                    stringBuilder = stringBuilder.Append (" ");
                    stringBuilder = stringBuilder.Append (parameterDTO.VarName);
                    stringBuilder = stringBuilder.Append (", ");
                }
                if (stringBuilder.Length != 0) {
                    stringBuilder = stringBuilder.Remove (stringBuilder.Length -2, 2);
                }
                
                listStore.AppendValues (
                        methodDTO.MethodName,
                        methodDTO.ReturnType,
                        methodDTO.ViewToResponse,
                        methodDTO.ResponseMethod,
                        stringBuilder.ToString ()
                        );
                list.Add (methodDTO);
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

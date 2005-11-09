using System;
using ComponentModel.Interfaces;
using ComponentBuilder.Interfaces;
using Gtk;

namespace ComponentBuilder.Forms {
    internal class ParameterView : IViewHandler, IGtkView {
        internal ParameterView () {
        }

        /* Interface Implementation */

        public void ClearForm () {
        }

        public IDataTransferObject GetDataForm () {
            return null;
        }

        public void LoadDataForm (IDataTransferObject dto) {
        }

        public Widget GetWidget () {
            return null;
        }
    }
}

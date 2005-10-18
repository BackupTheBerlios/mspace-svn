using System;
using ComponentModel.Container;
using ComponentModel.Interfaces;
using ComponentModel.DTO;
using Gtk;

namespace MainComponent.Forms {
    public sealed class MainComponentForm : IViewHandler {
        
        public MainComponentForm () {
        }
        
        public IDataTransferObject GetDataForm () {
            return null;
        }

        public void LoadDataForm (IDataTransferObject dto) {
        }

        public void ClearForm () {
        }

        public void ResponseInitApp (ResponseMethodDTO response) {
            if (response.ExecutionSuccess) {
                response = DefaultContainer.Instance.Execute ("ComponentBuilder", "ShowForm", null);
            }
        }
    }
}

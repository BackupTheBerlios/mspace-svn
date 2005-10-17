using System;
using ComponentModel.Interfaces;
using ComponentModel.DTO;
using System.Windows.Forms;

namespace UnitTest.Components.TestComponent.Form {
    public sealed class MainTestComponentForm : IViewHandler {
        public IDataTransferObject GetDataForm () {
            return null;
        }

        public void LoadDataForm (IDataTransferObject dto) {
        }

        public void ClearForm () {
        }

        public void ResponseReturnValue (ResponseMethodDTO responseMethodDTO) {
            if (responseMethodDTO.ExecutionSuccess) {
                MessageBox.Show ("Yeah !!, el valor de retorno es: " + responseMethodDTO.MethodResult); 
            }
        }
    }
}

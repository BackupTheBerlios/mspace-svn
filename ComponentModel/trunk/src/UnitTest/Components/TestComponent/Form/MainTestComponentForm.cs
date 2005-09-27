using System;
using ComponentModel.Interfaces;

namespace UnitTest.Components.TestComponent.Form {
    public sealed class MainTestComponentForm : IViewHandler {
        public IDataTransferObject GetDataForm () {
            return null;
        }

        public void LoadDataForm (IDataTransferObject dto) {
        }

        public void ClearForm () {
        }
    }
}

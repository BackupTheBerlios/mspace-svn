using System;
using ComponentModel.DefaultExceptionManager;

namespace ComponentModel.ComponentTest.Saludator.Exception {
    public class SaludatorExceptionManager : DefaultExceptionManager {
        public override void ProcessException (Exception exception) {
            base.ProcessException (exception); 
        }
    }
}

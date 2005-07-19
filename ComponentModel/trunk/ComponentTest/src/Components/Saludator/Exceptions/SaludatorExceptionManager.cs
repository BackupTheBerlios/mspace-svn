using System;
using ComponentModel.ExceptionManager;

namespace ComponentModel.ComponentTest.Components.Saludator.Exceptions {

    
    public class SaludatorExceptionManager : DefaultExceptionManager {

        public SaludatorExceptionManager () {
        }
        public override void ProcessException (Exception exception) {
            base.ProcessException (exception);
            Console.WriteLine ("Ha ocurrido una exception." + exception.Message);
            Console.WriteLine (exception.ToString ());
        }
    }
}

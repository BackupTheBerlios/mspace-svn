using System;
using ComponentModel.ExceptionManager;

namespace ComponentModel.ComponentTest.Components.Saludator.Exceptions {

    
    public class SaludatorExceptionManager : DefaultExceptionManager {
        public SaludatorExceptionManager () {
        }
        public override void ProcessException (Exception exception) {
            base.ProcessException (exception);
            if (exception is Exception) {
                Console.WriteLine ("Exception has been ocurred: " + exception.Message);
            }
            Console.WriteLine ("Ha ocurrido una exception.");
        }
    }
}

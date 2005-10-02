using System;
using ComponentModel.Interfaces;

namespace UnitTest.Components.TestComponent.Exceptions {
    public sealed class TestComponentExceptionManager : IExceptionManager {
        public void ProcessException (Exception exception) {
            Console.WriteLine ("BEGIN CAUGHT MANAGED EXCEPTION");
            Console.WriteLine ("Exception caught !! by " + this.GetType ());
            Console.WriteLine (exception.Message);
            Console.WriteLine ("END CAUGHT MANAGED EXCEPTION");
        }
    }
}

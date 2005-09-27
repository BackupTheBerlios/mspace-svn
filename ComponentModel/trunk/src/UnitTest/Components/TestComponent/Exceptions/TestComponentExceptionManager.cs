using System;
using ComponentModel.Interfaces;

namespace UnitTest.Components.TestComponent {
    public sealed class TestComponentExceptionManager : IExceptionManager {
        public void ProcessException (Exception exception) {
            Console.WriteLine ("BEGIN EXCEPTION");
            Console.WriteLine ("Exception caught !! by " + this.GetType ());
            Console.WriteLine (exception.ToString ());
            Console.WriteLine ("END EXCEPTION");
        }
    }
}

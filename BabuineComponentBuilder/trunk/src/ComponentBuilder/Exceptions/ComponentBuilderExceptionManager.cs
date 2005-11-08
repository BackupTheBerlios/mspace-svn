using System;
using ComponentModel.Interfaces;

namespace ComponentBuilder.Exceptions {
    public sealed class ComponentBuilderExceptionManager : IExceptionManager {
        public void ProcessException (Exception exception) {
            Console.WriteLine ("--Exception Has Been Caugth --");
            Console.WriteLine (exception.Message);
            Console.WriteLine ("--Exception Detail: ");
            Console.WriteLine (exception.ToString ());
            Console.WriteLine ("--End Exception.");
        }
    }
}

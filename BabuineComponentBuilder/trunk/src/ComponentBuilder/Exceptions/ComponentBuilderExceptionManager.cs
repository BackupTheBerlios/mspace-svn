using System;
using ComponentModel.Interfaces;

namespace ComponentBuilder.Exceptions {
    public sealed class ComponentBuilderExceptionManager : IExceptionManager {
        public void ProcessException (Exception exception) {
            Console.WriteLine (exception.ToString ());
        }
    }
}

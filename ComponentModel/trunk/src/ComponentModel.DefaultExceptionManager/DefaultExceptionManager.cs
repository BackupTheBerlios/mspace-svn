using System;
using ComponentModel.Interfaces;

namespace ComponentModel.DefaultExceptionManager {
    public class DefaultExceptionManager : IExceptionManager {
        public virtual void ProcessException (Exception exception) {
            Console.WriteLine (exception.ToString ());
        }
    }
}

using System;
using ComponentModel.Interfaces;

namespace ComponentModel.ExceptionManager {
    public class DefaultExceptionManager : IExceptionManager {
        public virtual void ProcessException (Exception exception) {
        }
    }
}

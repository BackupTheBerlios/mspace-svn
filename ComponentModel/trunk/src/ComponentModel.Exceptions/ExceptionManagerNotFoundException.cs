using System;

namespace ComponentModel.Exceptions {
    public class ExceptionManagerNotFoundException : ComponentModelException {
        public ExceptionManagerNotFoundException (string message) : base (message) {}
    }
}

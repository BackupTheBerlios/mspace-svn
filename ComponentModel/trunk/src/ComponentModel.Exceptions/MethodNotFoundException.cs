using System;

namespace ComponentModel.Exceptions {
    public class MethodNotFoundException : ComponentModelException {
        public MethodNotFoundException (string message) : base (message) {}
    }
}

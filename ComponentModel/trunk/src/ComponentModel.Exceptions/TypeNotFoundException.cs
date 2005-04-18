using System;

namespace ComponentModel.Exceptions {
    public class TypeNotFoundException : ComponentModelException {
        public TypeNotFoundException (string message) : base (message) {}
    }
}

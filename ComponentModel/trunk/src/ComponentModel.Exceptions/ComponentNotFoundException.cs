using System;

namespace ComponentModel.Exceptions {
    public class ComponentNotFoundException : ComponentModelException {
        public ComponentNotFoundException (string message) : base (message) {}
    }
}

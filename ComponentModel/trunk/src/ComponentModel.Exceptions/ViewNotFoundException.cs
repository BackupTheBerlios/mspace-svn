using System;

namespace ComponentModel.Exceptions {
    public class ViewNotFoundException : ComponentModelException {
        public ViewNotFoundException (string message) : base (message) {}
    }
}

using System;

namespace ComponentModel.Exceptions {
    public class ResponseNotFoundException : ComponentModelException {
        public ResponseNotFoundException (string message) : base (message) {}
    }
}

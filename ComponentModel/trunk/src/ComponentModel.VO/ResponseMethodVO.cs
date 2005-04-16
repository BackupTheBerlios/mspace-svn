using System;

namespace ComponentModel.VO {
    public class ResponseMethodVO {
        private bool executionSuccess;
        private object methodResult;

        public ResponseMethodVO () {
            this.SetExecutionSuccess (false);
        }
        
        public object MethodResult {
            get {return methodResult;}
            set {methodResult = value;}
        }
        
        public bool ExecutionSuccess {
            get {return executionSuccess;}
        }

        internal void SetExecutionSuccess (bool executionSuccess) {
            this.executionSuccess = executionSuccess;
        }
        
    }
}

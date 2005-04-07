using System;

namespace ComponentModel.VO {
    public class ResponseMethodVO {
        private object responseValue; 
        private bool executionSuccess;

        public ResponseMethodVO () {
            this.ExecutionSuccess = false;
        }
        
        public bool IsExecutionSuccess {
            get {return executionSuccess;}
        }

        internal bool ExecutionSuccess {
            set {executionSuccess = value;}
        }
        
        public object ResponseValue {
            get {return responseValue;}
            set {responseValue = value;}
        }

    }
}

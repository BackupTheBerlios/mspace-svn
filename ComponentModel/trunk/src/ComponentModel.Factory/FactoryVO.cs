using System;
using ComponentModel.VO;

namespace ComponentModel.Factory {
    public sealed class FactoryVO {
        private static FactoryVO instance;

        private FactoryVO () {
        }

        public static FactoryVO Instance {
            get {
                if (instance == null)
                    instance = new FactoryVO ();
                return instance;
            }
        }

        public ComponentModelVO CreateComponentModelVO () {
            return new ComponentModelVO ();
        }

        public ResponseMethodVO CreateResponseMethodVO () {
            ResponseMethodVO responseMethodVO = new ResponseMethodVO ();
            responseMethodVO.SetExecutionSuccess (false);
            return responseMethodVO;
        }
    }
}

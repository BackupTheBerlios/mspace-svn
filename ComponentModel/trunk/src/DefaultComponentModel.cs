using System;
using ComponentModel.Interfaces;
using ComponentModel.VO;

namespace ComponentModel {
    public class DefaultComponentModel : IComponentModel {
        ComponentModelVO vO;

        public ComponentModelVO VO {
            get {return vO;}
        }
        
        public DefaultComponentModel () {
        }
         
        internal void SetVO (ComponentModelVO vo) {
            this.vO = vo;
        }
    
    }
}

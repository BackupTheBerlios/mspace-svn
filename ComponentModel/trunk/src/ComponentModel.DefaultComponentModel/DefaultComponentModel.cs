using System;
using ComponentModel.Interfaces;
using ComponentModel.Vo;

namespace ComponentModel.DefaultComponentModel {
    public class DefaultComponentModel : IComponentModel {
        ComponentModelVO vO;

        public ComponentModelVO VO {
            get {return vO;}
        }

        
        internal DefaultComponentModel () {
            //Deberá rellenar del xml sus datos
        }
          
    
    }
}

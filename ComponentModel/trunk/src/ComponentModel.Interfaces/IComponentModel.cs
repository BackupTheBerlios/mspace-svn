using System;
using ComponentModel.VO;
namespace ComponentModel.Interfaces {
    public interface IComponentModel {
        ComponentModelVO VO {get;}

        ResponseMethodVO Execute (string methodName, params object[] parameters);
    }
}

using System;
using ComponentModel.VO;
namespace ComponentModel.Interfaces {
    public interface IComponentModel {
        ComponentModelVO VO {get;}

        ResponseMethodVO Execute (string methodName, object[] parameters);//Redirige & bloquea a la vista por defecto.
        ResponseMethodVO Execute (string methodName, bool redirect, bool block, object[] parameters);// Igual sobra !!
        ResponseMethodVO Execute (string methodName, bool redirect, bool block, object[] parameters, IViewHandler view);
    }
}

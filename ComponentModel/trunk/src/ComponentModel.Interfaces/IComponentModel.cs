using System;
using ComponentModel.VO;

namespace ComponentModel.Interfaces {
    public delegate void VirtualMethod (ResponseMethodVO responseMethodVO);
    
    public interface IComponentModel {
        ComponentModelVO VO {get;}
        VirtualMethod VirtualMethod {get; set;}
        
        ResponseMethodVO Execute (string methodName, object[] parameters);//Redirige & bloquea a la vista por defecto.
        ResponseMethodVO Execute (string methodName, object[] parameters, IViewHandler viewHandler);//Redirige y elige vista.
        ResponseMethodVO Execute (string methodName, object[] parameters, bool redirect);//Redirige o no redirige, si redirige vista nueva.
        ResponseMethodVO Execute (string methodName, object[] parameters, bool redirect, IViewHandler viewHandler, bool block);
    }
}

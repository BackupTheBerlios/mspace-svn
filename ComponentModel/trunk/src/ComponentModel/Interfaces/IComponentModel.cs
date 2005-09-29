/*
Babuine Component Model & Babuine Framework
Copyright (C) 2005  Néstor Salceda Alonso

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

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

        object GetProperty (string propertyName);
        void SetProperty (string propertyName, object valor);
    }
}

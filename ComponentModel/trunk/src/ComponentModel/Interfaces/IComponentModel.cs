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
using ComponentModel.DTO;
using ComponentModel.Collections;

namespace ComponentModel.Interfaces {
    public delegate void VirtualMethod (ResponseMethodDTO responseMethodDTO);
    
    public interface IComponentModel {

        ComponentModelDTO ComponentModelDTO {get;}
        VirtualMethod VirtualMethod {get; set;}
        IViewHandlerCollection ViewHandlerCollection {get; set;}
        
        ResponseMethodDTO Execute (string methodName, object[] parameters);//Redirige & bloquea a la vista por defecto.
        ResponseMethodDTO Execute (string methodName, object[] parameters, IViewHandler viewHandler);//Redirige y elige vista.
        ResponseMethodDTO Execute (string methodName, object[] parameters, bool redirect);//Redirige o no redirige, si redirige vista nueva.
        ResponseMethodDTO Execute (string methodName, object[] parameters, bool redirect, IViewHandler viewHandler, bool block);

        object GetProperty (string propertyName);
        void SetProperty (string propertyName, object valor);
    }
}

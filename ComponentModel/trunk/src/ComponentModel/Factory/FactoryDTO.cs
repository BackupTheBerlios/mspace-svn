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
using ComponentModel.Interfaces;

namespace ComponentModel.Factory {
    public enum CreateDTO {
        ComponentModel,
        ResponseMethod
    }
    
    public sealed class FactoryDTO {
        private static FactoryDTO instance;

        private FactoryDTO () {
        }

        public static FactoryDTO Instance {
            get {
                if (instance == null)
                    instance = new FactoryDTO ();
                return instance;
            }
        }

        public IComponentModelDTO Create (CreateDTO value) {
            switch (value) {
                case CreateDTO.ComponentModel:
                    return new ComponentModelDTO ();
                case CreateDTO.ResponseMethod:
                    ResponseMethodDTO responseMethodDTO = new ResponseMethodDTO ();
                    responseMethodDTO.SetExecutionSuccess (false); //Por defecto inicializará a false.
                    return responseMethodDTO;
                default:
                    return null;
            }
        }

    }
}

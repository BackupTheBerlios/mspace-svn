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

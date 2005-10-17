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
using System.Collections;

namespace ComponentModel.VO {
    [Obsolete ("ComponentModelVO is being removed in future Babuine Component Model version.  Please, use ComponentModelDto instead")]    
    public sealed class ComponentModelVO {
        private string componentName;
        private string componentClassName;
        private string exceptionManagerClassName;

        internal ComponentModelVO () {
        }
        
        public string ExceptionManagerClassName {
            get {return exceptionManagerClassName;}
            set {exceptionManagerClassName = value;}
        }
        
        public string ComponentClassName {
            get {return componentClassName;}
            set {componentClassName = value;}
        }
        
        public string ComponentName {
            get {return componentName;}
            set {componentName = value;}
        }
    }
}

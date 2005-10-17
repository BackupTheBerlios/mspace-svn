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
using System.Threading;
using System.Reflection;
using ComponentModel;
using ComponentModel.Factory;
using ComponentModel.Interfaces;
using ComponentModel.DTO;

namespace ComponentModel.Threading {
    internal class ComponentActionDispatcher  {
        /*Cojo los campos para la información de ponerlo en segundo plano*/
        private Thread thread;
        private ThreadStart threadStart;

        /*Recolecto datos para la ejecucución.*/
        IComponentModel componentModel;
        MethodInfo methodToExecute;
        object[] parameters;
        Type viewType;
        IViewHandler viewHandler;
        MethodInfo methodToResponse;
        ResponseMethodDTO responseMethodDTO;

        internal ResponseMethodDTO ResponseMethodDTO {
            get {
                lock (responseMethodDTO) {
                    return responseMethodDTO;
                }
            }
        }
        
        internal ComponentActionDispatcher () {}
        
        /*Ctor new View*/
        internal ComponentActionDispatcher (IComponentModel componentModel, MethodInfo methodToExecute, object[] parameters, Type viewType, MethodInfo methodToResponse) {
            this.componentModel = componentModel;
            this.methodToExecute = methodToExecute;
            this.parameters = parameters;
            this.viewType = viewType;
            this.methodToResponse = methodToResponse;

            //Set up Thread;
            threadStart = new ThreadStart (CallBackExecuteRedirectNewView);
            thread = new Thread (threadStart);
        }

        internal ComponentActionDispatcher (IComponentModel componentModel, MethodInfo methodToExecute, object[] parameters, IViewHandler viewHandler, MethodInfo methodToResponse) {
            this.componentModel = componentModel;
            this.methodToExecute = methodToExecute;
            this.parameters = parameters;
            this.viewHandler = viewHandler;
            this.methodToResponse = methodToResponse;

            threadStart = new ThreadStart (CallBackExecuteRedirectView);
            thread = new Thread (threadStart);
        }

        internal ComponentActionDispatcher (IComponentModel componentModel, MethodInfo methodToExecute, object[] parameters) {
            this.componentModel = componentModel;
            this.methodToExecute = methodToExecute;
            this.parameters = parameters;

            threadStart = new ThreadStart (CallBackExecuteNoRedirect);
            thread = new Thread (threadStart);
        }
        
        private void CallBackExecuteRedirectNewView () {
            lock (responseMethodDTO) {
                responseMethodDTO = (ResponseMethodDTO) FactoryDTO.Instance.Create (CreateDTO.ResponseMethod);
                object ret = methodToExecute.Invoke (componentModel, parameters);
                responseMethodDTO.MethodResult = ret;
                responseMethodDTO.SetExecutionSuccess (true);
                if (componentModel.VirtualMethod != null) {
                    componentModel.VirtualMethod (responseMethodDTO);
                    componentModel.VirtualMethod = null;
                }
                object obj = viewType.GetConstructor (null).Invoke (null);
                methodToResponse.Invoke (obj, new object[] {responseMethodDTO});
            }
        }

        private void CallBackExecuteRedirectView () {
            lock (responseMethodDTO) {
                responseMethodDTO = (ResponseMethodDTO)FactoryDTO.Instance.Create (CreateDTO.ResponseMethod);
                object ret = methodToExecute.Invoke (componentModel, parameters);
                responseMethodDTO.MethodResult = ret;
                responseMethodDTO.SetExecutionSuccess (true);
                if (componentModel.VirtualMethod != null) {
                    componentModel.VirtualMethod (responseMethodDTO);
                    componentModel.VirtualMethod = null;
                }
                methodToResponse.Invoke (viewHandler, new object[] {responseMethodDTO});
            }
        }

        private void CallBackExecuteNoRedirect () {
            lock (responseMethodDTO) {
                responseMethodDTO = (ResponseMethodDTO)FactoryDTO.Instance.Create (CreateDTO.ResponseMethod);
                object ret = methodToExecute.Invoke (componentModel, parameters);
                responseMethodDTO.MethodResult = ret;
                responseMethodDTO.SetExecutionSuccess (true);
                if (componentModel.VirtualMethod != null) {
                    componentModel.VirtualMethod (responseMethodDTO);
                    componentModel.VirtualMethod = null;
                }
            }
        }

        internal void Do () {
            if (thread != null) {
                thread.Start ();
            }
        }
    }
}

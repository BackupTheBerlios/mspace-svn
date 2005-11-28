/*
Babuine Component Model & Babuine Framework
Copyright (C) 2005  N�stor Salceda Alonso

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
using System.Reflection;
using ComponentModel.Interfaces;
using ComponentModel.DTO;
using ComponentModel.Container;
using ComponentModel.Container.Dao;
using ComponentModel.Factory;
using ComponentModel.ExceptionManager;
using ComponentModel.Threading;
using ComponentModel.Exceptions;
using ComponentModel.Collections;
using NLog;

namespace ComponentModel {
    public abstract class DefaultComponentModel : IComponentModel {
        //Logging
        private Logger logger = null;//= LogManager.GetLogger (this.GetType ().ToString ());
        //Value object with information associated to component
        private ComponentModelDTO componentModelDTO;
        
        //Exception manager to process exceptions.
        private IExceptionManager defaultExceptionManager;

        private VirtualMethod virtualMethod;
        private IViewHandlerCollection viewHandlerCollection;

        public IViewHandlerCollection ViewHandlerCollection {
            get {return viewHandlerCollection;}
            set {viewHandlerCollection = value;}
        }
        
        public VirtualMethod VirtualMethod {
            get {return virtualMethod;}
            set {virtualMethod = value;}
        }
        
        public IExceptionManager DefaultExceptionManager {
            get {return defaultExceptionManager;}
            set {defaultExceptionManager = value;}
        }

        public ComponentModelDTO ComponentModelDTO {
            get {return componentModelDTO;}
        }
        
        //Ctor
        protected DefaultComponentModel () {
            logger = LogManager.GetLogger (this.GetType ().ToString ());
            logger.Debug ("Executing ctor for: " + this.GetType ().FullName);
            viewHandlerCollection = new IViewHandlerCollection ();
        }
      
        private Type GetTypeExceptionManager (string exceptionManagerClassName) {
            //Precondition: exceptionManagerClassName != null &&
            //exceptionManagerClassName != String.Empty
            if ((exceptionManagerClassName == null) || (exceptionManagerClassName.Equals (String.Empty))) {
                throw new ExceptionManagerNotFoundException ("Null exception managerClassName.");
            }
            try {
                Type type = DefaultContainerDao.Instance.GetType (exceptionManagerClassName);
                if (type.IsSubclassOf (typeof (DefaultExceptionManager)) || (type.GetInterface ("IExceptionManager") != null)) 
                    return type;
            }
            catch (TypeNotFoundException ex) {
            //PostCondition: return != null
            //Si llega aquí, no ha encontrado el tipo del exceptionManager.
                throw new ExceptionManagerNotFoundException ("Exception Manager Can't be found in Component.");
            }
            return null;
        }

        // Implementación de referencia para obtener la sobrecarga.  No es
        // llamada, dado que aún no ha sido testeado su funcionamiento.
        private MethodInfo GetMethodToExecute (string methodName, object[] parameters, Type componentType) {
            //Precondition: methodName != null && methodName != String.Empty &&
            //componentType != null
            logger.Debug ("Entering GetMethodToExecute.  Searching " + methodName + " in: " + componentType.ToString ());
            Type[] typeParam = Type.EmptyTypes;
            if (parameters != null) {
                typeParam = new Type[parameters.Length];
                for (int i = 0; i < typeParam.Length; i++) {
                    typeParam[i] = parameters[i].GetType ();
                }
            }
            logger.Debug ("Types of method invocation: ");
            for (int i = 0;i < typeParam.Length;i++) {
                logger.Debug ("Parameter: " + i + " " + typeParam[i]);
            }
            //Busca solamente los publicos.  Cambiar el Binder puede ser un lio
            //grande, ojo con esto.
            MethodInfo methodInfo = componentType.GetMethod (methodName, typeParam);
            //¿encontrado?
            if (methodInfo == null) {
                throw new MethodNotFoundException ("Method to execute: " + methodName + " not found.");
            }
            else {
                logger.Debug ("Finded method to execute: " + methodInfo.ToString ());
            }
            //Post: methodInfo != null
            return methodInfo;
        }
        
        private MethodInfo GetMethodToExecute (string methodName, object[] parameters) {
            return this.GetMethodToExecute (methodName, parameters, this.GetType ());
        }
        
        private MethodInfo GetMethodToResponse (Type viewType, ComponentMethodAttribute componentMethodAttribute) {
            if (componentMethodAttribute == null) {
                throw new ResponseNotFoundException ("Please set up correctly Response Attribute.");    
            }
            MethodInfo responseMethod = viewType.GetMethod (componentMethodAttribute.ResponseName, BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public) ;
            if (responseMethod == null) 
                throw new ResponseNotFoundException ("Response: " + componentMethodAttribute.ResponseName + " not found in: " + viewType.ToString ());
            else 
                logger.Debug ("Finded response to execute: " + responseMethod.ToString () + " in: " + viewType.ToString ());
            return responseMethod;
        } 
        
        private void MapException (Exception exception) {
            if (exception.InnerException is ComponentModelException) 
                throw exception.InnerException;
            else {
                if (this.defaultExceptionManager == null) {
                    //Type typeManager = this.GetTypeExceptionManager (this.VO.ExceptionManagerClassName);
                    Type typeManager = this.GetTypeExceptionManager (this.ComponentModelDTO.ExceptionManagerClassName);
                    this.defaultExceptionManager = (IExceptionManager) typeManager.GetConstructor (null).Invoke (null);
                }
                this.defaultExceptionManager.ProcessException (exception.InnerException);
            }
        }
        
        private Type GetViewType (string viewType) {
            if ((viewType == null) || (viewType.Equals (String.Empty)))
                throw new ViewNotFoundException ("Please, set up attributes correctly.");
            try {
                Type type = DefaultContainerDao.Instance.GetType (viewType);
                if (type.GetInterface ("IViewHandler") != null)
                    return type;
            }
            catch (TypeNotFoundException exception) {
                throw new ViewNotFoundException ("View " + viewType + " not found.");
            }
            return null;
        }
        
        private Type GetViewType (ComponentMethodAttribute componentMethodAttribute) {
            if (componentMethodAttribute == null)
                throw new ViewNotFoundException ("Please set up attributes correctly.");
            return this.GetViewType (componentMethodAttribute.ViewName);
        }
    
        private ComponentMethodAttribute GetComponentAttributes (MethodInfo methodInfo) {
            //Pre: MethodInfo != null
            if (methodInfo == null)
                //TODO: ¿Es buena la excepción?
                throw new MethodNotFoundException ("Please set up attributes correctly.");
            Attribute[] atts = (Attribute[]) methodInfo.GetCustomAttributes (typeof (ComponentMethodAttribute), true);
            //Post: atts != null
            if (atts[0] == null)
                throw new MethodNotFoundException ("Can't find ComponentMethodAttribute.");
            return ((ComponentMethodAttribute)atts[0]);
        }


        private ResponseMethodDTO ExecuteRedirectNewView (MethodInfo methodToExecute, object[] parameters, Type viewType, MethodInfo methodToResponse) {
            ResponseMethodDTO responseMethodDTO = (ResponseMethodDTO) FactoryDTO.Instance.Create (CreateDTO.ResponseMethod); 
            try {
                object ret = methodToExecute.Invoke (this, parameters);
                responseMethodDTO.MethodResult = ret;
                object obj = viewType.GetConstructor (null).Invoke (null);
                responseMethodDTO.SetExecutionSuccess (true);
                if (VirtualMethod != null) {
                    VirtualMethod (responseMethodDTO);
                    VirtualMethod = null;
                }
                methodToResponse.Invoke (obj, new object[] {responseMethodDTO});
                //Beta Implementation
                //
                //Siempre se va a a�adir esta vista puesto que es una vista
                //nueva.
                ViewHandlerCollection.Add ((IViewHandler)obj);
                logger.Debug ("A new view has been added to View Cache.");
                //
                //End
                return responseMethodDTO;
            }
            catch (TargetInvocationException exception) {
                this.MapException (exception);
            }
            return responseMethodDTO;
        }

        private ResponseMethodDTO ExecuteRedirectView (MethodInfo methodToExecute, object[] parameters, IViewHandler viewHandler, MethodInfo methodToResponse) {
            ResponseMethodDTO responseMethodDTO = (ResponseMethodDTO) FactoryDTO.Instance.Create (CreateDTO.ResponseMethod);
            try {
                object ret = methodToExecute.Invoke (this, parameters);
                responseMethodDTO.MethodResult = ret;
                responseMethodDTO.SetExecutionSuccess (true);
                if (VirtualMethod != null) {
                    VirtualMethod (responseMethodDTO);
                    VirtualMethod = null;
                }
                //Beta Implementation
                //
                if (!ViewHandlerCollection.Contains (viewHandler)) {
                    ViewHandlerCollection.Add (viewHandler);
                    logger.Debug ("A new view has been added to View Cache");
                }
                else {
                    logger.Debug ("This view has already been registered at View Cache.");
                }
                //
                //End
                
                methodToResponse.Invoke (viewHandler, new object[] {responseMethodDTO});
            }
            catch (TargetInvocationException exception) {
                this.MapException (exception);
            }
            return responseMethodDTO;
        }
        
        private ResponseMethodDTO ExecuteNoRedirect (MethodInfo methodToExecute, object[] parameters) {
            ResponseMethodDTO responseMethodDTO = (ResponseMethodDTO) FactoryDTO.Instance.Create (CreateDTO.ResponseMethod);
            try {
                object ret = methodToExecute.Invoke (this, parameters);
                responseMethodDTO.MethodResult = ret;
                responseMethodDTO.SetExecutionSuccess (true);
                if (VirtualMethod != null) {
                    VirtualMethod (responseMethodDTO);
                    VirtualMethod = null;
                }
            }
            catch (TargetInvocationException exception) {
                this.MapException (exception);
            }
            return responseMethodDTO;
        }
        
        /*Executor overloads*/
        public ResponseMethodDTO Execute (string methodName, object[] parameters) {
            return this.Execute (methodName, parameters, true, null, true);
        }
        
        public ResponseMethodDTO Execute (string methodName, object[] parameters, bool redirect) {
            return this.Execute (methodName, parameters, redirect, null, true);
        }
        
        public ResponseMethodDTO Execute (string methodName, object[] parameters, IViewHandler viewHandler) {
            return this.Execute (methodName, parameters, true, viewHandler, true);
        }       
        
        /*Executor commander !*/
        public ResponseMethodDTO Execute (string methodName, object[] parameters, bool redirect, IViewHandler viewHandler, bool block) {
            /*Existen cosas que siempre deben de buscarse*/
            MethodInfo methodToExecute = this.GetMethodToExecute (methodName, parameters); 
            ComponentMethodAttribute componentMethodAttribute = this.GetComponentAttributes (methodToExecute);
            /*Primero discernimos si es bloqueante o no lo es*/
            if (block) {
                //Operaciones que son bloqueantes
                if (redirect) {
                    MethodInfo methodToResponse ;
                    Type viewType;
                    if (viewHandler == null) {
                        //FIX: Aún se podria mejorar esta invocación.
                        viewType = this.GetViewType (componentMethodAttribute);
                        methodToResponse = this.GetMethodToResponse (viewType, componentMethodAttribute);
                        return ExecuteRedirectNewView (methodToExecute, parameters, viewType, methodToResponse);
                    }
                    else {
                        //Redirigimos a la que nos pide
                        viewType = viewHandler.GetType ();
                        methodToResponse = this.GetMethodToResponse (viewType, componentMethodAttribute);
                        return ExecuteRedirectView (methodToExecute, parameters, viewHandler, methodToResponse);
                    }
                }
                else {
                    //No necesitamos información sobre la vista, ni el response.
                    //No se va a ejecutar.
                    return ExecuteNoRedirect (methodToExecute, parameters);
                }
            }
            else {
                ComponentActionDispatcher componentActionDispatcher;
                //Operaciones bloqueantes, envolver en un hilo
                if (redirect) {
                    MethodInfo methodToResponse;
                    Type viewType;
                    if (viewHandler == null) {
                        //Exception handling ¿¿
                        viewType = this.GetViewType (componentMethodAttribute);
                        methodToResponse = this.GetMethodToResponse (viewType, componentMethodAttribute);
                        componentActionDispatcher = new ComponentActionDispatcher (this, methodToExecute, parameters, viewType, methodToResponse);
                        try {
                            componentActionDispatcher.Do ();
                        }
                        catch (TargetInvocationException exception) {
                            this.MapException (exception);
                        }
                        return componentActionDispatcher.ResponseMethodDTO;
                    }
                    else {
                        viewType = viewHandler.GetType ();
                        methodToResponse = this.GetMethodToResponse (viewType, componentMethodAttribute);
                        componentActionDispatcher = new ComponentActionDispatcher (this, methodToExecute, parameters, viewHandler, methodToResponse);
                        try {
                            componentActionDispatcher.Do ();
                        }
                        catch (TargetInvocationException exception) {
                            this.MapException (exception);
                        }
                        return componentActionDispatcher.ResponseMethodDTO;
                    }
                }
                else {
                    componentActionDispatcher = new ComponentActionDispatcher (this, methodToExecute, parameters); 
                    try {
                        componentActionDispatcher.Do ();
                    }
                    catch (TargetInvocationException exception) {
                        this.MapException (exception);
                    }
                    return componentActionDispatcher.ResponseMethodDTO;
                }
            }
            //return null;
        }


        /*B�squeda de propiedades*/

        private PropertyInfo GetPropertyToExecute (string propertyName) {
            if ((propertyName == null) || (propertyName.Length != 0)) {
                Type type = this.GetType ();
                PropertyInfo propertyInfo = type.GetProperty (propertyName);
                if (propertyInfo == null) {
                    throw new PropertyNotFoundException ("Property " + propertyName + " not found");
                }
                return propertyInfo;
            }
            return null;
        }
        
        public object GetProperty (string propertyName) {
            PropertyInfo propertyInfo = this.GetPropertyToExecute (propertyName);
            if (propertyInfo.CanRead) {
                MethodInfo methodInfo = propertyInfo.GetGetMethod ();
                return methodInfo.Invoke (this, null);
                //Suponemos que como se puede leer no va a ser null el
                //methodInfo.
            }
            else {
                throw new PropertyCanReadException ("Property can't access to getter Method.");
            }
        }

        public void SetProperty (string propertyName, object valor) {
            PropertyInfo propertyInfo = this.GetPropertyToExecute (propertyName);
            if (propertyInfo.CanWrite) {
                MethodInfo methodInfo = propertyInfo.GetSetMethod ();
                methodInfo.Invoke (this, new object[]{valor});
                //Suponemos que como se puede escribir no va a ser null el
                //methodInfo.
            }
            else {
                throw new PropertyCanWriteException ("Property can't access to setter Method.");
            }
        }
    }
}

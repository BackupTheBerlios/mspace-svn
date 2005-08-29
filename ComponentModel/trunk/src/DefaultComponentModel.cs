using System;
using System.Reflection;
using ComponentModel.Interfaces;
using ComponentModel.VO;
using ComponentModel.Container;
using ComponentModel.Container.Dao;
using ComponentModel.Factory;
using ComponentModel.ExceptionManager;
using ComponentModel.Threading;
using ComponentModel.Exceptions;
using NLog;

namespace ComponentModel {
    // TODO: En los getMethodXXX --> se debería realizar también una búsqueda
    // con parámetros para poder permitir la sobrecarga de métodos.  Y para las
    // respuestas nos taparía un error muy majo :)
    public class DefaultComponentModel : IComponentModel {
        //Logging
        private Logger logger = LogManager.GetLogger ("ComponentModel.DefaultComponentModel");
        //Value object with information associated to component
        private ComponentModelVO vO;
        //Exception manager to process exceptions.
        private IExceptionManager defaultExceptionManager;

        private VirtualMethod virtualMethod;

        public VirtualMethod VirtualMethod {
            get {return virtualMethod;}
            set {virtualMethod = value;}
        }
        
        public IExceptionManager DefaultExceptionManager {
            get {return defaultExceptionManager;}
            set {defaultExceptionManager = value;}
        }
 
        //Properties
        public ComponentModelVO VO {
            get {return vO;}
        }
        
        //Ctor
        protected DefaultComponentModel () {
            logger.Debug ("Executing ctor for: " + this.GetType ().FullName);
        }
      
        private Type GetTypeExceptionManager (string exceptionManagerClassName) {

            //Precondition: exceptionManagerClassName != null &&
            //exceptionManagerClassName != String.Empty
            if ((exceptionManagerClassName == null) | (exceptionManagerClassName.Equals (String.Empty))) {
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
        
        private MethodInfo GetMethodToExecute (string methodName, Type componentType) {
            //Precondition: methodName != null && methodName != String.Empty &&
            //componentType != null
            logger.Debug ("Entering GetMethodToExecute. Searching: " + methodName + " in: " + componentType.ToString ());
            MethodInfo methodInfo = componentType.GetMethod (methodName, BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
            //Checkeamos que lo haya encontrado
            if (methodInfo == null) {
                throw new MethodNotFoundException ("Method to execute: " + methodName + " not found.");
            }
            else {
                logger.Debug ("Finded method to execute: " + methodInfo.ToString ());
            }
            //PostCondition: methodInfo != null
            return methodInfo;
        }

        private MethodInfo GetMethodToExecute (string methodName) {
            return this.GetMethodToExecute (methodName, this.GetType ());
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
                    Type typeManager = this.GetTypeExceptionManager (this.VO.ExceptionManagerClassName);
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
                throw new ViewNotFoundException ("View" + viewType + "not found.");
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


        private ResponseMethodVO ExecuteRedirectNewView (MethodInfo methodToExecute, object[] parameters, Type viewType, MethodInfo methodToResponse) {
            ResponseMethodVO responseMethodVO = FactoryVO.Instance.CreateResponseMethodVO (); 
            try {
                object ret = methodToExecute.Invoke (this, parameters);
                responseMethodVO.MethodResult = ret;
                object obj = viewType.GetConstructor (null).Invoke (null);
                responseMethodVO.SetExecutionSuccess (true);
                if (VirtualMethod != null) {
                    VirtualMethod (responseMethodVO);
                    VirtualMethod = null;
                }
                methodToResponse.Invoke (obj, new object[] {responseMethodVO});
                return responseMethodVO;
            }
            catch (TargetInvocationException exception) {
                this.MapException (exception);
            }
            return responseMethodVO;
        }

        private ResponseMethodVO ExecuteRedirectView (MethodInfo methodToExecute, object[] parameters, IViewHandler viewHandler, MethodInfo methodToResponse) {
            ResponseMethodVO responseMethodVO =  FactoryVO.Instance.CreateResponseMethodVO ();
            try {
                object ret = methodToExecute.Invoke (this, parameters);
                responseMethodVO.MethodResult = ret;
                responseMethodVO.SetExecutionSuccess (true);
                if (VirtualMethod != null) {
                    VirtualMethod (responseMethodVO);
                    VirtualMethod = null;
                }
                methodToResponse.Invoke (viewHandler, new object[] {responseMethodVO});
            }
            catch (TargetInvocationException exception) {
                this.MapException (exception);
            }
            return responseMethodVO;
        }
        
        private ResponseMethodVO ExecuteNoRedirect (MethodInfo methodToExecute, object[] parameters) {
            ResponseMethodVO responseMethodVO = FactoryVO.Instance.CreateResponseMethodVO ();
            try {
                object ret = methodToExecute.Invoke (this, parameters);
                responseMethodVO.MethodResult = ret;
                responseMethodVO.SetExecutionSuccess (true);
                if (VirtualMethod != null) {
                    VirtualMethod (responseMethodVO);
                    VirtualMethod = null;
                }
            }
            catch (TargetInvocationException exception) {
                this.MapException (exception);
            }
            return responseMethodVO;
        }
        
        /*Executor overloads*/
        public ResponseMethodVO Execute (string methodName, object[] parameters) {
            return this.Execute (methodName, parameters, true, null, true);
        }
        
        public ResponseMethodVO Execute (string methodName, object[] parameters, bool redirect) {
            return this.Execute (methodName, parameters, redirect, null, true);
        }
        
        public ResponseMethodVO Execute (string methodName, object[] parameters, IViewHandler viewHandler) {
            return this.Execute (methodName, parameters, true, viewHandler, true);
        }       
        
        /*Executor commander !*/
        public ResponseMethodVO Execute (string methodName, object[] parameters, bool redirect, IViewHandler viewHandler, bool block) {
            /*Existen cosas que siempre deben de buscarse*/
            MethodInfo methodToExecute = this.GetMethodToExecute (methodName); 
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
                        return componentActionDispatcher.ResponseMethodVO;
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
                        return componentActionDispatcher.ResponseMethodVO;
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
                    return componentActionDispatcher.ResponseMethodVO;
                }
            }
            //return null;
        }

    }
}

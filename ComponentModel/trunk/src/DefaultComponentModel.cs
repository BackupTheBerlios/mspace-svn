using System;
using System.Reflection;
using ComponentModel.Interfaces;
using ComponentModel.VO;
using ComponentModel.Container;
using ComponentModel.Container.Dao;
using ComponentModel.Factory;
using ComponentModel.ExceptionManager;
using ComponentModel.Exceptions;
using NLog;

namespace ComponentModel {
    public class DefaultComponentModel : IComponentModel {
        //Logging
        private Logger logger = LogManager.GetLogger ("ComponentModel.DefaultComponentModel");
        //Value object with information associated to component
        private ComponentModelVO vO;
        //Exception manager to process exceptions.
        private IExceptionManager defaultExceptionManager;

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
        
        private MethodInfo GetMethodResponse (Type viewType, ComponentMethodAttribute componentMethodAttribute) {
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
        
        private void InstantiateExceptionManager () {
            if (defaultExceptionManager == null) {
                logger.Debug ("Trying to instantiate ExceptionManager.");
                Type typeManager = this.GetTypeExceptionManager (this.VO.ExceptionManagerClassName);
                this.defaultExceptionManager = (IExceptionManager)typeManager.GetConstructor (null).Invoke (null);
                logger.Debug ("Exception manager instatiated.");
            }
            logger.Debug ("Exception manager already instatiated.");
        }
        
        private ResponseMethodVO ExecuteCompleteSequence (MethodInfo methodToInvoke, object[] parameters, Type viewType, MethodInfo methodResponse) {
            try {
                ResponseMethodVO responseMethodVO =  FactoryVO.Instance.CreateResponseMethodVO ();
                logger.Debug ("Method " + methodToInvoke + " executing.");
                object ret = methodToInvoke.Invoke (this, parameters);
                responseMethodVO.MethodResult = ret;
                logger.Debug ("Redirecting to view: " + viewType.ToString () + " to response Method: " + methodResponse.ToString ());
                object obj = viewType.GetConstructor (null).Invoke (null);
                logger.Debug ("Executing response method: " + methodResponse.ToString ());
                responseMethodVO.SetExecutionSuccess (true);
                logger.Debug ("Setting excecuttion success as true.");
                methodResponse.Invoke (obj, new object[] {responseMethodVO});
                return responseMethodVO;
            }
            catch (TargetInvocationException exception) {
                if (exception.InnerException is ComponentModelException) 
                    throw exception.InnerException;
                else {
                    this.InstantiateExceptionManager ();
                    //Console.WriteLine (exception.InnerException.GetType ().ToString ());
                    defaultExceptionManager.ProcessException (exception.InnerException);
                }
            }
            return null;
        }
        

        private ResponseMethodVO ExecuteCompleteSequence (MethodInfo methodToInvoke, object[] parameters, IViewHandler viewHandler, MethodInfo methodResponse) {
            try {
                ResponseMethodVO responseMethodVO =  FactoryVO.Instance.CreateResponseMethodVO ();
                logger.Debug ("Method " + methodToInvoke + " executing.");
                object ret = methodToInvoke.Invoke (this, parameters);
                responseMethodVO.MethodResult = ret;
                logger.Debug ("Executing response method: " + methodResponse.ToString ());
                responseMethodVO.SetExecutionSuccess (true);
                logger.Debug ("Setting excecuttion success as true.");
                methodResponse.Invoke (viewHandler, new object[] {responseMethodVO});
                logger.Debug ("Returning ResponseMethodVO");
                return responseMethodVO;
            }
            catch (TargetInvocationException exception) {
                if (exception.InnerException is ComponentModelException) 
                    throw exception.InnerException;
                else {
                    this.InstantiateExceptionManager ();
                    //Console.WriteLine (exception.InnerException.GetType ().ToString ());
                    defaultExceptionManager.ProcessException (exception.InnerException);
                }
            }
            return null;
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
        
        //Public Methods
        public ResponseMethodVO Execute (string methodName, object[] parameters) {
            Type type = this.GetType ();

            //Recollecting data for execution.
            // Check if exists use case to execute
            MethodInfo methodToExecute = this.GetMethodToExecute (methodName, type);
            // Read attributes for this use case.
            Attribute[] attributes = (Attribute[]) methodToExecute.GetCustomAttributes (typeof (ComponentMethodAttribute), false);
            ComponentMethodAttribute componentMethodAttribute = (ComponentMethodAttribute) attributes[0];
            //Gets viewType from information of attributes.
            Type viewType = this.GetViewType (componentMethodAttribute);
            // Si existe algún miembro requerido para la ejecución que no es
            // encontrado; no ejecutará nada denada.
            //
            //Execute method
            // Get information from respone for view.
            MethodInfo responseMethod = this.GetMethodResponse (viewType, componentMethodAttribute);
            //Executing response to method
            ResponseMethodVO responseMethodVO = this.ExecuteCompleteSequence (methodToExecute, parameters, viewType, responseMethod); 
            return responseMethodVO;
        }
        
        public ResponseMethodVO Execute (string methodName, bool redirect, bool block, object[] parameters) {
            throw new Exception ("Not implemented yet.");
        }

        public ResponseMethodVO Execute (string methodName, bool redirect, bool block, object[] parameters, IViewHandler view) {
            Type type = this.GetType ();
            
            MethodInfo methodToExecute = this.GetMethodToExecute (methodName, type);
            Attribute[] attributes = (Attribute[]) methodToExecute.GetCustomAttributes (typeof (ComponentMethodAttribute), false);
            ComponentMethodAttribute componentMethodAttribute = (ComponentMethodAttribute) attributes[0];
            MethodInfo responseMethod = this.GetMethodResponse (view.GetType (), componentMethodAttribute);
            
            ResponseMethodVO responseMethodVO = this.ExecuteCompleteSequence (methodToExecute, parameters, view, responseMethod);
            return responseMethodVO;
        }

    }
}

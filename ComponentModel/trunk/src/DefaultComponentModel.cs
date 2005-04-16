using System;
using System.Reflection;
using ComponentModel.Interfaces;
using ComponentModel.VO;
using ComponentModel.Container;
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
            foreach (Assembly assembly in DefaultContainer.Instance.Assemblies) {
                foreach (Type type in assembly.GetTypes ()) {
                    if (type.IsSubclassOf (typeof (DefaultExceptionManager)) || type.IsSubclassOf (typeof (IExceptionManager))) {
                        if (type.ToString().Equals (exceptionManagerClassName)) {
                            logger.Debug ("Getting type of exception manager: " + type.ToString ());
                            return type;
                        }
                    }
                }
            }
            //PostCondition: return != null
            //Si llega aquí, no ha encontrado el tipo del exceptionManager.
            throw new ExceptionManagerNotFoundException ("Exception Manager Can't be found in Component.");
            //return null;
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
            if (responseMethod == null) {
                throw new ResponseNotFoundException ("Response: " + componentMethodAttribute.ResponseName + " not found in: " + viewType.ToString ());
            } 
            else {
                logger.Debug ("Finded response to execute: " + responseMethod.ToString () + " in: " + viewType.ToString ());
            }
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
                ResponseMethodVO responseMethodVO = new ResponseMethodVO ();
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
                if (exception.InnerException is ComponentModelException) {
                    throw exception.InnerException;
                }
                else {
                    this.InstantiateExceptionManager ();
                    defaultExceptionManager.ProcessException (exception);
                }
            }
            return null;
        }
        
        private Type GetViewType (ComponentMethodAttribute componentMethodAttribute) {
            if (componentMethodAttribute == null)
                throw new ViewNotFoundException ("Please set up attributes correctly.");
            foreach (Assembly assembly in DefaultContainer.Instance.Assemblies) {
                foreach (Type type in assembly.GetTypes ()) {
                    if ((type.ToString ()).Equals (componentMethodAttribute.ViewName)) {
                        logger.Debug ("ViewType finded: " + type.ToString ());
                        if (type.GetInterface ("IViewHandler") != null)    
                            return type;
                    }
                }
            }
            throw new ViewNotFoundException ("View " +componentMethodAttribute.ViewName + " not found");
        }
        
        //Public Methods
        public ResponseMethodVO Execute (string methodName, params object[] parameters) {
            Type type = this.GetType ();

            //Recollecting data for execution.
            MethodInfo methodToExecute = this.GetMethodToExecute (methodName, type);
            Attribute[] attributes = (Attribute[]) methodToExecute.GetCustomAttributes (typeof (ComponentMethodAttribute), false);
            ComponentMethodAttribute componentMethodAttribute = (ComponentMethodAttribute) attributes[0];
            Type viewType = this.GetViewType (componentMethodAttribute);
            
            //Execute method
            MethodInfo responseMethod = this.GetMethodResponse (viewType, componentMethodAttribute);
            //Executing response to method
            ResponseMethodVO responseMethodVO = this.ExecuteCompleteSequence (methodToExecute, parameters, viewType, responseMethod); 
            return responseMethodVO;
        }
    }
}

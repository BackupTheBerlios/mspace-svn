using System;
using System.Reflection;
using ComponentModel.Interfaces;
using ComponentModel.VO;
using NLog;
using ComponentModel.Container;
using ComponentModel.ExceptionManager;
using ComponentModel.Exceptions;

namespace ComponentModel {
    public class DefaultComponentModel : IComponentModel {
        //Logging
        private Logger logger = LogManager.GetLogger ("ComponentModel.DefaultComponentModel");
        private ComponentModelVO vO;
        private DefaultExceptionManager defaultExceptionManager;

        public DefaultExceptionManager DefaultExceptionManager {
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
            //Si llega aquí, no ha encontrado el exceptionManager.
            throw new ExceptionManagerNotFoundException ("Exception Manager Can't be found in Component.");
            //return null;
        }
        
        private MethodInfo GetMethodToExecute (string methodName, Type componentType) {
            logger.Debug ("Entering GetMethodToExecute. Searching: " + methodName + " in: " + componentType.ToString ());
            MethodInfo methodInfo = componentType.GetMethod (methodName, BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
            //Checkeamos que lo haya encontrado
            if (methodInfo == null) {
                throw new MethodNotFoundException ("Method to execute: " + methodName + " not found.");
            }
            else {
                logger.Debug ("Finded method to execute: " + methodInfo.ToString ());
            }
            return methodInfo;
        }
        
        private MethodInfo GetMethodResponse (Type viewType, ComponentMethodAttribute componentMethodAttribute) {
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
                Type typeManager = this.GetTypeExceptionManager (this.VO.ExceptionClassName);
                this.defaultExceptionManager = (DefaultExceptionManager)typeManager.GetConstructor (null).Invoke (null);
                logger.Debug ("Exception manager instatiated.");
            }
            logger.Debug ("Exception manager already instatiated.");
        }
        
        private ResponseMethodVO ExecuteCompleteSequence (MethodInfo methodToInvoke, object[] parameters, Type viewType, MethodInfo methodResponse) {
            try {
                ResponseMethodVO responseMethodVO = new ResponseMethodVO ();
                logger.Debug ("Method " + methodToInvoke + " executing.");
                object ret = methodToInvoke.Invoke (this, parameters);
                responseMethodVO.ResponseValue = ret;
                logger.Debug ("Redirecting to view: " + viewType.ToString () + " to response Method: " + methodResponse.ToString ());
                object obj = viewType.GetConstructor (null).Invoke (null);
                logger.Debug ("Executing response method: " + methodResponse.ToString ());
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
            /**
            catch (Exception exception) {
                //if (exception is ComponentModelException) {
                if (exception is TargetInvocationException) {
                    //Tirar la exception para arriba.
                    logger.Debug ("A ComponentModelException has been caugth");
                    logger.Debug (exception.InnerException.ToString ());
                    throw new ComponentModelException("ComponentModel Exception.");
                }
                else {
                    this.InstantiateExceptionManager ();
                    defaultExceptionManager.ProcessException (exception);
                }
                logger.Debug ("A exception has been caught.");
            }
            */
            return null;
        }
        
        private Type GetViewType (ComponentMethodAttribute componentMethodAttribute) {
            foreach (Assembly assembly in DefaultContainer.Instance.Assemblies) {
                foreach (Type type in assembly.GetTypes ()) {
                    if ((type.ToString ()).Equals (componentMethodAttribute.ViewName)) {
                        logger.Debug ("ViewType finded: " + type.ToString ());
                        return type;
                    }
                }
            }
            throw new ViewNotFoundException ("View " +componentMethodAttribute.ViewName + " not found");
            //return null;
        }
        
        //Other (Aux @DEPRECATED)
        internal void SetVO (ComponentModelVO vo) {
            this.vO = vo;
        }
        
        //Public Methods
        public ResponseMethodVO Execute (string methodName, object[] parameters) {
            Type type = this.GetType ();
            logger.Debug ("Type of component invoke. " + type.ToString ());

            //Recollecting data for execution.
            MethodInfo methodToExecute = this.GetMethodToExecute (methodName, type);
            Attribute[] attributes = (Attribute[]) methodToExecute.GetCustomAttributes (typeof (ComponentMethodAttribute), false);
            ComponentMethodAttribute componentMethodAttribute = (ComponentMethodAttribute) attributes[0];
            Type viewType = this.GetViewType (componentMethodAttribute);
            MethodInfo responseMethod = this.GetMethodResponse (viewType, componentMethodAttribute);
            //Executing 
            ResponseMethodVO responseMethodVO = this.ExecuteCompleteSequence (methodToExecute, parameters, viewType, responseMethod); 
            return responseMethodVO;
        }
    }
}

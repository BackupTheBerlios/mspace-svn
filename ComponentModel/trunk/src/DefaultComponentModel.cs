using System;
using System.Reflection;
using ComponentModel.Interfaces;
using ComponentModel.VO;
using NLog;
using ComponentModel.Container;

namespace ComponentModel {
    public class DefaultComponentModel : IComponentModel {
        //Logging
        Logger logger = LogManager.GetLogger ("ComponentModel.DefaultComponentModel");
        ComponentModelVO vO;
        
        //Properties
        public ComponentModelVO VO {
            get {return vO;}
        }
        //Ctor
        protected DefaultComponentModel () {
            logger.Debug ("Executing ctor for: " + this.GetType ().FullName);
        }
        
        public ResponseMethodVO Execute (string methodName, params object[] parameters) {
            logger.Debug ("Enter method Execute.");
            Type type = this.GetType ();
            logger.Debug ("Type of component invoke: " + type.ToString ());
            ResponseMethodVO responseMethodVO = new ResponseMethodVO ();
            // Solo se aceptan métodos no estáticos, declarados y públicos.
            MethodInfo methodInfo = type.GetMethod (methodName, BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
            logger.Debug ("Finded method to execute: " + methodInfo.ToString ());
            //Cumple requisitos de ejecución?
            Attribute[] attributes = (Attribute[])methodInfo.GetCustomAttributes (typeof (ComponentMethodAttribute),false);
            if (attributes.Length == 1) {
                ComponentMethodAttribute componentMethodAttribute = (ComponentMethodAttribute)attributes[0];
                logger.Debug ("Finded Attribute: " + componentMethodAttribute.GetType ());
                logger.Debug ("Reading attribute viewName: " + componentMethodAttribute.ViewName);
                Type viewType = this.GetViewType (componentMethodAttribute);
                logger.Debug ("Finded view Type: " + viewType.ToString ());
                //Execute Method here !! 
                object ret = methodInfo.Invoke (this, parameters);
                responseMethodVO.ResponseValue = ret;
                //Redirecting to view.
                object obj = viewType.GetConstructor (null).Invoke (null);
                methodInfo = viewType.GetMethod (componentMethodAttribute.ResponseName, BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
                //Executing response.
                methodInfo.Invoke (obj, new object[]{responseMethodVO});
                logger.Debug ("Finded response Method in view: " + viewType.ToString() + " " + methodInfo.ToString ());
            }
            logger.Debug ("Exiting method Execute");
            if (responseMethodVO.ResponseValue != null)
                return responseMethodVO;
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
            return null;
        }
        
        //Other (Aux @DEPRECATED)
        internal void SetVO (ComponentModelVO vo) {
            this.vO = vo;
        }
         
    }
}

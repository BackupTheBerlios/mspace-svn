using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.IO;
using System.Text;
using ComponentModel;
using ComponentBuilder.DTO;

namespace ComponentBuilder.Bo {
    [Component ("ComponentBuilder", "ComponentBuilder.Exceptions.ComponentBuilderExceptionManager")]
    public sealed class ComponentBuilderComponentModel : DefaultComponentModel {
        
        [ComponentMethod ("ComponentBuilder.Forms.MainComponentBuilderForm", "ResponseShowForm")]
        public void ShowForm () {
        }
        
        /**
         *
         * A la hora de generar código vamos a hacer lo siguiente.
         *
         * 1) Obtenemos las templates.
         * 2) Rellenamos las templates básicas.  Empezaremos por los métodos,
         * bajaremos a los parámetros y luego escribiremos el método en el BO y
         * finalmente la response en la vista que proceda.
         * 3) Escribiremos en el disco los archivos y crearemos la jerarquía de
         * directorios.
         *
         *  StringBuffer a destajo !!
         *      -Las cadenas son inmutables, así que nos evitaremos el estar
         *      tocandole las peloticas al heap creando y eliminando.
         */ 
        
        // Obtendrá las templates a pelo.
        private Hashtable GetTemplates () {
            Hashtable hashtable = new Hashtable ();
            hashtable = AddToTemplateTable (hashtable, TemplateNamesDTO.BusinessObject);
            hashtable = AddToTemplateTable (hashtable, TemplateNamesDTO.ExceptionManager);
            hashtable = AddToTemplateTable (hashtable, TemplateNamesDTO.MethodBody);
            hashtable = AddToTemplateTable (hashtable, TemplateNamesDTO.ResponseMethod);
            hashtable = AddToTemplateTable (hashtable, TemplateNamesDTO.ViewHandler);
            return hashtable;
        }

        private Hashtable AddToTemplateTable (Hashtable hashtable, string templateName) {
            StreamReader streamReader = new StreamReader (this.GetType ().Assembly.GetManifestResourceStream (templateName));
            hashtable.Add (templateName, streamReader.ReadToEnd ());
            streamReader.Close ();
            return hashtable;
        }

        private Hashtable InitComponentTable (ComponentSettingsDTO componentSettingsDTO, Hashtable templateTable) {
            Hashtable componentTable = new Hashtable ();
            //Key fileName without path, string value. 
            componentTable.Add (componentSettingsDTO.ComponentName+"ComponentModel.cs", templateTable[TemplateNamesDTO.BusinessObject]);
            //Añadimos el gestor de exceptions
            componentTable.Add (componentSettingsDTO.ClassExceptionManager+".cs", templateTable[TemplateNamesDTO.ExceptionManager]); 
            //Añaidmos las vistas
            StringEnumerator enumerator = componentSettingsDTO.ViewsCollection.GetEnumerator ();
            while (enumerator.MoveNext ()) {
                string currentView = enumerator.Current;
                componentTable.Add (currentView + ".cs", templateTable[TemplateNamesDTO.ViewHandler]); 
            }
            return componentTable;
        }

        private Hashtable FillTable (ComponentSettingsDTO componentSettingsDTO, Hashtable componentTable) {
            StringCollection stringCollection = new StringCollection ();
            IEnumerator enumerator = componentTable.Keys.GetEnumerator ();
            while (enumerator.MoveNext ()) {
                stringCollection.Add ((string) enumerator.Current);
            }
           
            enumerator = (stringCollection as IEnumerable).GetEnumerator ();
            while (enumerator.MoveNext ()) {
                string currentKey = (string)enumerator.Current;
                StringBuilder stringBuilder = new StringBuilder ((string)componentTable[currentKey]);
                stringBuilder = stringBuilder.Replace (TagValuesDTO.ComponentName, componentSettingsDTO.ComponentName);
                stringBuilder = stringBuilder.Replace (TagValuesDTO.ExceptionManager, componentSettingsDTO.ClassExceptionManager);
                //Ahora para las vistas se discernira para cada una.
                foreach (String view in componentSettingsDTO.ViewsCollection) { 
                    if (currentKey.StartsWith (view)) {
                        stringBuilder = stringBuilder.Replace (TagValuesDTO.ViewName, view);
                    }
                }
                componentTable[currentKey] = stringBuilder.ToString ();
            }
            return componentTable;
        }
        
        private Hashtable FillMethods (ComponentSettingsDTO componentSettingsDTO, Hashtable componentTable, Hashtable templateTable) {
            StringBuilder stringBuilder = new StringBuilder ();
            foreach (MethodDTO methodDTO in componentSettingsDTO.MethodsCollection) {
                stringBuilder = stringBuilder.Append (templateTable[TemplateNamesDTO.MethodBody]);
                stringBuilder = stringBuilder.Replace (TagValuesDTO.ReturnType, methodDTO.ReturnType);
                stringBuilder = stringBuilder.Replace (TagValuesDTO.MethodName, methodDTO.MethodName);
                //TODO:Build parameters
                StringBuilder methodStringBuilder = new StringBuilder ();
                foreach (ParameterDTO parameterDTO in methodDTO.ParametersCollection) {
                    methodStringBuilder.Append (parameterDTO.TypeName);
                    methodStringBuilder.Append (" ");
                    methodStringBuilder.Append (parameterDTO.VarName);
                    methodStringBuilder.Append (", ");
                }
                //Eliminamos la coma y el ultimo espacio.
                if (methodStringBuilder.Length != 0)
                    methodStringBuilder.Remove (methodStringBuilder.Length -2, 2);
                stringBuilder = stringBuilder.Replace (TagValuesDTO.Parameters, methodStringBuilder.ToString ());
                //Attributes.
                stringBuilder = stringBuilder.Replace (TagValuesDTO.ComponentName, componentSettingsDTO.ComponentName);
                stringBuilder = stringBuilder.Replace (TagValuesDTO.ViewName, methodDTO.ViewToResponse);
                stringBuilder = stringBuilder.Replace (TagValuesDTO.ResponseName, methodDTO.ResponseMethod);
            }
            //Obtenemos el BO del componente y seteamos el valor, reemplazando.
            //
            StringCollection stringCollection = new StringCollection ();
            IEnumerator enumerator = componentTable.Keys.GetEnumerator ();
            while (enumerator.MoveNext ()) {
                stringCollection.Add ((string) enumerator.Current);
            }
            enumerator = (stringCollection as IEnumerable).GetEnumerator ();
            string currentValue = String.Empty;
            while (enumerator.MoveNext ()) {
                currentValue = (string) enumerator.Current;
                if (currentValue.EndsWith ("ComponentModel.cs")) {
                    break;
                }
            }
            if (!currentValue.Equals (String.Empty)) {
                //Esta comprobación igual sobra.
                StringBuilder componentTemplateBuilder = new StringBuilder ((string)componentTable[currentValue]);
                componentTemplateBuilder = componentTemplateBuilder.Replace (TagValuesDTO.Body, stringBuilder.ToString ());
                componentTable[currentValue] = componentTemplateBuilder.ToString ();
            }
            
            return componentTable;
        }
        
        private Hashtable FillResponses (ComponentSettingsDTO componentSettingsDTO, Hashtable componentTable, Hashtable templateTable) {
            foreach (string viewName in componentSettingsDTO.ViewsCollection) {
                StringBuilder responseBuilder = new StringBuilder ();
                foreach (MethodDTO methodDTO in componentSettingsDTO.MethodsCollection) {
                    if (methodDTO.ViewToResponse.Equals (viewName)) {
                        responseBuilder = responseBuilder.Append (templateTable[TemplateNamesDTO.ResponseMethod]);
                        responseBuilder = responseBuilder.Replace (TagValuesDTO.ResponseName, methodDTO.ResponseMethod);
                    }
                }
                Console.WriteLine (responseBuilder.ToString () + " at ViewName " +  viewName);
                StringBuilder viewBuilder = new StringBuilder ((string)componentTable [viewName + ".cs"]);
                Console.WriteLine (viewBuilder.ToString ());
                viewBuilder = viewBuilder.Replace (TagValuesDTO.Body, responseBuilder.ToString ());
                componentTable [viewName + ".cs"] = viewBuilder.ToString ();
            }
            return componentTable;
        }
        
        [ComponentMethod ("ComponentBuilder.Forms.MainComponentBuilderForm", "ResponseGenerateComponent")]
        public void GenerateComponent (ComponentSettingsDTO componentSettingsDTO) {
            Console.WriteLine ("/----/");
            Hashtable templateTable = GetTemplates ();
            Hashtable componentTable = InitComponentTable (componentSettingsDTO, templateTable);
            componentTable = FillTable (componentSettingsDTO, componentTable);
            //Haremos una segunda pasada para construir los métodos.
            componentTable = FillMethods (componentSettingsDTO, componentTable, templateTable);
            componentTable = FillResponses (componentSettingsDTO, componentTable, templateTable);
            
            IDictionaryEnumerator enumerator = componentTable.GetEnumerator ();
            while (enumerator.MoveNext ()) {
                Console.WriteLine (enumerator.Key + "\n" + enumerator.Value);
            }
            Console.WriteLine ("/-END-/");
        }

    }
}

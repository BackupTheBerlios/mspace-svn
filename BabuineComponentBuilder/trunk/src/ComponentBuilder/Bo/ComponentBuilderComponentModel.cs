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
                Console.WriteLine (currentKey);
           
                StringBuilder stringBuilder = new StringBuilder ((string)componentTable[currentKey]);
               
                stringBuilder = stringBuilder.Replace (TagValuesDTO.ComponentName, componentSettingsDTO.ComponentName);
                stringBuilder = stringBuilder.Replace (TagValuesDTO.ExceptionManager, componentSettingsDTO.ClassExceptionManager);
                //Ahora para las vistas se discernirá.
                
                componentTable[currentKey] = stringBuilder.ToString ();
            
            }
            return componentTable;
        }
        
        [ComponentMethod ("ComponentBuilder.Forms.MainComponentBuilderForm", "ResponseGenerateComponent")]
        public void GenerateComponent (ComponentSettingsDTO componentSettingsDTO) {
            Console.WriteLine ("/----/");
            Hashtable templateTable = GetTemplates ();
            Hashtable componentTable = InitComponentTable (componentSettingsDTO, templateTable);
            componentTable = FillTable (componentSettingsDTO, componentTable);
            
            IDictionaryEnumerator enumerator = componentTable.GetEnumerator ();
            while (enumerator.MoveNext ()) {
                Console.WriteLine (enumerator.Key + "\n" + enumerator.Value);
            }
            Console.WriteLine ("/-END-/");
        }

    }
}

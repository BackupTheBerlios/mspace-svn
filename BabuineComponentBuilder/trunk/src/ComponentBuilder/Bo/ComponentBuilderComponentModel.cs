using System;
using System.Collections;
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
        
        [ComponentMethod ("ComponentBuilder.Forms.MainComponentBuilderForm", "ResponseGenerateComponent")]
        public void GenerateComponent (ComponentSettingsDTO componentSettingsDTO) {
            Console.WriteLine ("/----/");
            Hashtable templateTable = GetTemplates ();
            IDictionaryEnumerator enumerator = templateTable.GetEnumerator ();
            while (enumerator.MoveNext ()) {
                StringBuilder stringBuilder = new StringBuilder ((string)enumerator.Value);
                stringBuilder = stringBuilder.Replace (TagValuesDTO.ComponentName, componentSettingsDTO.ComponentName);
                Console.WriteLine (enumerator.Key + "\n" + stringBuilder.ToString ());
            }
            Console.WriteLine ("/-END-/");
        }

    }
}

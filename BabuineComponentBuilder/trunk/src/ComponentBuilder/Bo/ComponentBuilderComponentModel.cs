using System;
using System.Reflection;
using System.IO;
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
        
        [ComponentMethod ("ComponentBuilder.Forms.MainComponentBuilderForm", "ResponseGenerateComponent")]
        public void GenerateComponent (ComponentSettingsDTO componentSettingsDTO) {
            Console.WriteLine ("/----/");
            Stream stream = this.GetType ().Assembly.GetManifestResourceStream (TemplateNamesDTO.BusinessObject); 
            StreamReader streamReader = new StreamReader (stream);
            Console.WriteLine (streamReader.ReadToEnd ().Replace ("${component_name}", componentSettingsDTO.ComponentName));
            streamReader.Close ();
            stream.Close ();
            Console.WriteLine ("/-END-/");
        }

    }
}

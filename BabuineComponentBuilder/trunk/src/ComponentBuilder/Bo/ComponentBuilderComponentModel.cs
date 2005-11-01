using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ComponentModel;
using ComponentBuilder.DTO;

namespace ComponentBuilder.Bo {
    [Component ("ComponentBuilder", "ComponentBuilder.Exceptions.ComponentBuilderExceptionManager")]
    public sealed class ComponentBuilderComponentModel : DefaultComponentModel {
        
        private PreferencesDTO preferencesDTO;
        
        public PreferencesDTO PreferencesDTO {
            get {return preferencesDTO;}
            set {preferencesDTO = value;}
        }

        public ComponentBuilderComponentModel () {
            try {
                preferencesDTO = DeserializePreferences ();
            }
            catch (Exception exception) {
                preferencesDTO = new PreferencesDTO ();
            }
        }
        
        [ComponentMethod ("ComponentBuilder.Forms.MainComponentBuilderForm", "ResponseSerializePreferences")]
        public void SerializePreferences () {
            XmlSerializer xmlSerializer = new XmlSerializer (typeof (PreferencesDTO));
            StreamWriter streamWriter = new StreamWriter (Path.Combine ("resources", "ComponentBuilder.config"));
            xmlSerializer.Serialize (streamWriter, preferencesDTO);
            streamWriter.Close ();
        }

        [ComponentMethod ("ComponentBuilder.Forms.MainComponentBuilderForm", "ResponseDeserializePreferences")]
        public PreferencesDTO DeserializePreferences () {
            XmlSerializer xmlSerializer = new XmlSerializer (typeof (PreferencesDTO));
            FileStream fileStream = new FileStream (Path.Combine ("resources", "ComponentBuilder.config"), FileMode.Open);
            preferencesDTO = (PreferencesDTO) xmlSerializer.Deserialize (fileStream);
            fileStream.Close ();
            return preferencesDTO;
        }

        
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
            hashtable = AddToTemplateTable (hashtable, TemplateNamesDTO.NAntBuildfile);
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
            //Añadimos el file nant.
            if (preferencesDTO.GenerateBuildfile) {
                componentTable.Add (componentSettingsDTO.ComponentName+".build", templateTable[TemplateNamesDTO.NAntBuildfile]);
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
                if (currentKey.EndsWith (".build")) 
                    stringBuilder = stringBuilder.Replace (TagValuesDTO.NAntComponentName, componentSettingsDTO.ComponentName);
                else 
                    stringBuilder = stringBuilder.Replace (TagValuesDTO.ComponentName, componentSettingsDTO.ComponentName);
                stringBuilder = stringBuilder.Replace (TagValuesDTO.ExceptionManager, componentSettingsDTO.ClassExceptionManager);
                if (preferencesDTO.PrefixNamespace.Length != 0) {
                    //Lo seteará y añadirá un punto.
                    StringBuilder prefixBuilder = new StringBuilder (preferencesDTO.PrefixNamespace);
                    prefixBuilder = prefixBuilder.Append (".");
                    stringBuilder = stringBuilder.Replace (TagValuesDTO.Prefix, prefixBuilder.ToString ());
                }
                else {
                    //No lo seteara el prefix namespace
                    stringBuilder = stringBuilder.Replace (TagValuesDTO.Prefix, preferencesDTO.PrefixNamespace);
                }
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
                //Set up prefix
                
                if (preferencesDTO.PrefixNamespace.Length != 0) {
                    //Lo seteará y añadirá un punto.
                    StringBuilder prefixBuilder = new StringBuilder (preferencesDTO.PrefixNamespace);
                    prefixBuilder = prefixBuilder.Append (".");
                    stringBuilder = stringBuilder.Replace (TagValuesDTO.Prefix, prefixBuilder.ToString ());
                }
                else {
                    //No lo seteara el prefix namespace
                    stringBuilder = stringBuilder.Replace (TagValuesDTO.Prefix, preferencesDTO.PrefixNamespace);
                } 
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
                StringBuilder viewBuilder = new StringBuilder ((string)componentTable [viewName + ".cs"]);
                viewBuilder = viewBuilder.Replace (TagValuesDTO.Body, responseBuilder.ToString ());
                componentTable [viewName + ".cs"] = viewBuilder.ToString ();
            }
            return componentTable;
        }
        
        private DirectoryInfo CreateSkeleton (ComponentSettingsDTO componentSettingsDTO) {
            DirectoryInfo directoryInfo = new DirectoryInfo (preferencesDTO.OutputPath); 
            DirectoryInfo componentDirectoryInfo = directoryInfo.CreateSubdirectory (componentSettingsDTO.ComponentName);
            //Ahora crearemos uno para cada cosa, Bo, Forms, Dto, Exceptions ...
            componentDirectoryInfo.CreateSubdirectory ("Bo");
            componentDirectoryInfo.CreateSubdirectory ("Forms");
            componentDirectoryInfo.CreateSubdirectory ("Dto");
            componentDirectoryInfo.CreateSubdirectory ("Exceptions");
            componentDirectoryInfo.CreateSubdirectory ("Resources");
        
            return componentDirectoryInfo;
        }

        private void WriteFiles (Hashtable componentTable, DirectoryInfo componentDirectory, ComponentSettingsDTO componentSettingsDTO) {
            StringCollection stringCollection = new StringCollection ();
            IEnumerator enumerator = componentTable.Keys.GetEnumerator ();
            while (enumerator.MoveNext ()) {
                stringCollection.Add ((string) enumerator.Current);
            }
           
            enumerator = (stringCollection as IEnumerable).GetEnumerator ();
            while (enumerator.MoveNext ()) {
                string fileName = (string) enumerator.Current;
                string content = (string) componentTable[fileName];
                if (fileName.EndsWith ("ComponentModel.cs")) {
                    WriteFileAt (fileName, componentDirectory.GetDirectories ("Bo")[0], content);
                }
                if (fileName.StartsWith (componentSettingsDTO.ClassExceptionManager)) {
                    WriteFileAt (fileName, componentDirectory.GetDirectories ("Exceptions")[0], content);
                }
                IEnumerator auxEnumerator = (componentSettingsDTO.ViewsCollection as IEnumerable).GetEnumerator ();
                while (auxEnumerator.MoveNext ()) {
                    if (fileName.StartsWith ((string)auxEnumerator.Current)) {
                        WriteFileAt (fileName, componentDirectory.GetDirectories ("Forms")[0], content);
                    }
                }
            } 

            if (preferencesDTO.GenerateBuildfile) {
                enumerator.Reset ();
                while (enumerator.MoveNext ()) {
                    string fileName = (string) enumerator.Current;
                    if (fileName.StartsWith (componentSettingsDTO.ComponentName) && fileName.EndsWith (".build")) {
                        string content = (string) componentTable [fileName];
                        WriteFileAt (fileName, componentDirectory, content);
                    }
                }
            }
        }

        private void WriteFileAt (string fileName, DirectoryInfo directoryTo, string content) {
            FileStream fileStream = new FileStream (Path.Combine (directoryTo.ToString (),fileName), FileMode.Create);
            StreamWriter streamWriter = new StreamWriter (fileStream);
            streamWriter.Write (content);
            streamWriter.Close ();
            fileStream.Close ();
        }
        
        [ComponentMethod ("ComponentBuilder.Forms.MainComponentBuilderForm", "ResponseGenerateComponent")]
        public void GenerateComponent (ComponentSettingsDTO componentSettingsDTO) {
            Hashtable templateTable = GetTemplates ();
            Hashtable componentTable = InitComponentTable (componentSettingsDTO, templateTable);
            componentTable = FillTable (componentSettingsDTO, componentTable);
            //Haremos una segunda pasada para construir los métodos.
            componentTable = FillMethods (componentSettingsDTO, componentTable, templateTable);
            //Tercera pasada para conseguir las respuestas.
            componentTable = FillResponses (componentSettingsDTO, componentTable, templateTable);
            
            //Ahora solo resta guardarlo en archivos.  Con la ruta por
            //defecto que se haya configurado.
            DirectoryInfo componentDirectory = CreateSkeleton (componentSettingsDTO); 
            WriteFiles (componentTable, componentDirectory, componentSettingsDTO); 
            
            IDictionaryEnumerator enumerator = componentTable.GetEnumerator ();
            while (enumerator.MoveNext ()) {
                Console.WriteLine (enumerator.Key + "\n" + enumerator.Value);
            }
        }
    }
}

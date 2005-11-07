using System;
using System.IO;
using System.Xml.Serialization;
using ComponentModel.Interfaces;
using ComponentBuilder.Interfaces;
using ComponentBuilder.DTO;

namespace ComponentBuilder.DAO {
    public sealed class ProjectDAO : IFileDAO {
        public void Serialize (Stream stream, IDataTransferObject dataTransferObject) {
            XmlSerializer xmlSerializer = new XmlSerializer (dataTransferObject.GetType ()); 
            StreamWriter streamWriter = new StreamWriter (stream);
            xmlSerializer.Serialize (streamWriter, dataTransferObject);
            streamWriter.Close ();
        }

        public IDataTransferObject Deserialize (Stream stream) {
            XmlSerializer xmlSerializer = new XmlSerializer (typeof (ProjectDTO));
            return (ProjectDTO)xmlSerializer.Deserialize (stream);
        }
    }
}

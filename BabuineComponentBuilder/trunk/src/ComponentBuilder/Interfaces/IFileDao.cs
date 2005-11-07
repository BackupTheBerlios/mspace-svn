//IFileDao.cs 
//
//For operations in a persistent file only.
//

using System;
using System.IO;
using ComponentModel.Interfaces;

namespace ComponentBuilder.Interfaces {
    public interface IFileDAO {
        void Serialize (Stream stream, IDataTransferObject dataTransferObject);
        IDataTransferObject Deserialize (Stream stream);
    }
}

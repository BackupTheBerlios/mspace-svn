using System;
using System.Xml;

using ComponentModel.Vo;

namespace ComponentModel.DefaultComponentModel.Dao {
    public class DefaultComponentModelDao {
        XmlDocument document;
        
        private const string component = "component";
        private const string methods = "methods";
        private const string method = "method";
        private const string response = "response";
        
        internal DefaultComponentModelDao (string xmlFile) {
            document.Load (xmlFile);
        }

        internal ComponentModelVO FillVO () {
            ComponentModelVO vo = new ComponentModelVO ();
            GetHeader (vo);
            return vo; 
        }

        private void GetHeader (ComponentModelVO vo) {
            XmlNodeList xmlNodeList = document.GetElementsByTagName (component);
            XmlNode xmlNode = xmlNodeList[0];
            vo.ClassName = xmlNode.Attributes["class"].Value;
            vo.ExceptionClassName = xmlNode.Attributes["exception"].Value;
        }
        
        
    }
}

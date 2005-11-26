using System.Xml.Serilization;
using System.IO;

namespace libmonotimer
{
	public class XMLSource : ISource {

		string filename;
		SProject[] projects;

		public XMLSource (string filename) {
			this.filename = filename;
			this.projects = this.Projects;
		}

		public SProject[] Projects {
			get {
				XmlSerializer serializer = new XmlSerializer (typeof (SProject[]));
				FileStream stream = new FileStream (FileMode.Open, FileAccess.Read);
				projects = (SProject[]) serializer.Deserialize (stream);
				return projects;
			}
		}	
		
		Project loadProject () {
		
		}
		
		void saveProject (SProject[] projects) {
			XmlSerializer serializer = new XmlSerializer (projets.GetType ());
			FileStream stream = new FileStream (this.filename, FileMode.Create);
			serializer.Serialize (stream, projects);
		}
		
	}
}




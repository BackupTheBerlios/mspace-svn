using System.XML;
using System.IO;

namespace libmonotimer
{
	public class XMLSource : ISource {

		string filename;

		public XMLSource (string filename) {
			this.filename = filename;
			loadProject ();
		}

		SProject[] getProjects () {
			SProject[] projects;
			XmlSerializer serializer = new XmlSerializer (projects.getType ());
			FileStream stream = new FileStream (FileMode.Open, FileAccess.Read);
			projects = (SProject[]) serializer.Deserialize (stream);
			return projects;
		}
		
		Project loadProject () {
		
		}
		
		void saveProject (SProject[] projects) {
			XmlSerializer serializer = new XmlSerializer (projets.getType ());
			FileStream stream = new FileStream (this.filename, FileMode.Create);
			serializer.Serialize (stream, projects);
		}
		
	}
}




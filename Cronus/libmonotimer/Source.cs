using System;

namespace libmonotimer
{
	public interface ISource {
		void GetName ();
	}


	public class SourceXML : ISource {
		public void GetName () {
			Console.WriteLine ("Soy un Source de tipo XML");
		}	
	}

	public class SourceRemote : ISource {
		public void GetName () {
			Console.WriteLine ("Soy un Source de tipo XML");
		}	
	}


	public class Factory {
		public ISource GetObject (int type){
			ISource source = null;
			switch (type) {
				case 0:
					source = new SourceXML ();
					break;
				case 1:
					source = new SourceRemote ();
					break;
				default:
					source = null;
					break;
			}
			return source;
		}
	}

	public class Launcher {
		public static void Main () {
			Factory factory = new Factory ();
			ISource source = factory.GetObject (1);
			source.GetName ();
		}
	}
	
}




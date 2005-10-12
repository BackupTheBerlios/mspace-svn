using System;

namespace libmonotimer
{
	/* Depending on the parameters passed to the GetObjetc function, the factory will return
	 * the proper source.
	 */

	public class Factory {
		
		public ISource GetObject (string url, string login, string password) {
			ISource source = new RPCSource ();
			return source;
		}

		public ISource GetObjetc (string path) {
			ISource source = new XMLSource ();
			return source;
		}
	}
}

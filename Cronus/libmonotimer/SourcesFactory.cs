using System;

namespace libmonotimer
{
	/* Depending on the parameters passed to the GetObjetc function, the factory will return
	 * the proper source.
	 */

	public class Factory {
		
		ISource source;
		
		public static ISource GetObject (string[] args) {
			switch (args.Length) {
				case 1:
					source = new XMLSource ();
					break;
				case 3:
					source = new RPCSource ();
					break;
				default:
					source = null;
			}
			return source;
		}

	}
}

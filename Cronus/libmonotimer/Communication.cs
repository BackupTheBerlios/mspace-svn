using System;
using CookComputing.XmlRpc;
using System.Security.Cryptography;
using System.Text;

namespace libmonotimer {
	
	[XmlRpcUrl("http://fragg.homeip.net/phpcollab/xmlrpc_server.php")]
	interface IStateName
	{
	
		[XmlRpcMethod("phpcollab.timeControl")]
		CollabProject[] timeControl (string usuario, string pass); 
	}
	
	public class Communication {
		
	
		public static CollabProject[] login (string name, string password) {		
			IStateName proxy = (IStateName)XmlRpcProxyGen.Create(typeof(IStateName));	
			CollabProject[] valor = proxy.timeControl (login, name);
			foreach ( CollabProject i in valor ) {
				Console.WriteLine (i.id);
				Console.WriteLine (i.nombre);
			}
			return valor;
		}
		
	
	}
		
}

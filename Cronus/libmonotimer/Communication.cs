using System;
using CookComputing.XmlRpc;
using System.Security.Cryptography;
using System.Text;


namespace libmonotimer {
	

[XmlRpcUrl("URL_del_server_xmlrpc")]
interface IStateName
{

	[XmlRpcMethod("phpcollab.timeControl")]
	CollabProject[] timeControl (string usuario, string pass); 
}

public class Communication {
	

	public static CollabProject[] login (string name, string password) {		
		IStateName proxy = (IStateName)XmlRpcProxyGen.Create(typeof(IStateName));	
		CollabProject[] valor = proxy.timeControl ("", "");
		foreach ( CollabProject i in valor ) {
			Console.WriteLine (i.id);
			Console.WriteLine (i.nombre);
		}
		return valor;
	}
	

}
		
}

/*
		string textToHash = "";
		byte[] byteRepresentation = UnicodeEncoding.UTF8.GetBytes (textToHash);
		byte[] hashedTextInBytes = null;
		MD5CryptoServiceProvider myMD5 = new MD5CryptoServiceProvider();
		hashedTextInBytes = myMD5.ComputeHash (byteRepresentation);
		foreach ( byte i in hashedTextInBytes ) {
			Console.WriteLine (i);
		}
		string hashedText = Convert.ToBase64String (hashedTextInBytes);
		*/


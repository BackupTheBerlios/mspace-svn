using System;
using CookComputing.XmlRpc;
using System.Security.Cryptography;
using System.Text;

public struct proyecto {
	public int id;
	public string nombre;
}

[XmlRpcUrl("URL_del_servidor_xmlrpc")]
interface IStateName
{

	[XmlRpcMethod("phpcollab.timeControl")]
	proyecto[] timeControl (string usuario, string pass); 
}

public class Launcher {
	
	public static void Main () {

		
		IStateName proxy = (IStateName)XmlRpcProxyGen.Create(typeof(IStateName));	
		proyecto[] valor = proxy.timeControl ("", "");
		if (valor.Length == 0) {
			Console.WriteLine ("Respuesta vacia");
		}
		else {
			foreach ( proyecto i in valor ) {
				Console.WriteLine (i.id);
				Console.WriteLine (i.nombre);
			}
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

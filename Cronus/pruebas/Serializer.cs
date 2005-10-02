// Simple XML serialization
using System;
using System.IO;
using System.Xml.Serialization;
using libmonotimer;
using Control;


public class MyObject {
	public int X;
	public int Y;

	public MyObject() {
	}

	public MyObject(int x, int y) {
		this.X = x;
		this.Y = y;
	}    

	public static void Main(string[] args)
	{
		Project p1 = Project.Instance;
		p1.Name = "Proyecto 1";
		Console.WriteLine ("OK");
		using (FileStream fs = new FileStream("test.xml", FileMode.Create)) {        
			XmlSerializer serializer = new XmlSerializer(p1.GetType()); 
			serializer.Serialize(Console.Out, p1);     
		}
     }
}

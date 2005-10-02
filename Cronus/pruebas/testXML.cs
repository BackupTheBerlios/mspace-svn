// Simple XML serialization
using System;
using System.IO;
using System.Xml.Serialization;

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
          MyObject[] array = new MyObject[2];
	  array[0] = new MyObject (5, 8);
	  array[1] = new MyObject (12, 3);
          using (FileStream fs = new FileStream("test.xml", FileMode.Create)) {        
               XmlSerializer serializer = new XmlSerializer(array.GetType()); 
               serializer.Serialize(Console.Out, array);     
          }
     }
}

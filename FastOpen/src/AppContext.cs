namespace FastOpen
{
    using System;
    using System.IO;
    using System.Reflection;

    public class AppContext
    {

	private static string fastopenrc;

	public static readonly string ConfigDir = Environment.GetEnvironmentVariable ("HOME") + Path.DirectorySeparatorChar +
								+ ".config" + Path.DirectorySeparatorChar +
								+ "fastopen";
	public static readonly string ShortcutsFile = ConfigDir + Path.DirectorySeparatorChar + "fastopenrc";
	

	static AppContext ()
	{
	    LoadDefaultFastopenrc ();
	    try {
		DirectoryInfo dirInfo = new DirectoryInfo (ConfigDir);
		if (!dirInfo.Exists)
		    dirInfo.Create ();
		FileInfo fileInfo = new FileInfo (ShortcutsFile);
		if (!fileInfo.Exists)
		{
		    StreamWriter writer = fileInfo.CreateText ();
		    writer.Write (fastopenrc);
		    writer.Close ();
		}
	    } catch {
		Console.WriteLine ("ERROR: Could not write default settings");
	    }
	}

	public static void Init ()
	{
	}

	public string Fastopenrc
	{
	    get {
		return fastopenrc;
	    }
	}

	private static void LoadDefaultFastopenrc ()
	{
	    Assembly assembly = System.Reflection.Assembly.GetCallingAssembly ();
	    System.IO.Stream s = assembly.GetManifestResourceStream ("fastopenrc");
	    StreamReader reader = new StreamReader (s);
	    fastopenrc = reader.ReadToEnd ();
	}
    }
}

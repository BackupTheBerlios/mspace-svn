namespace WeekPlanner
{
	using System;
	using System.IO;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Reflection;
	
	public sealed class App 
	{
		public static readonly string Home = Environment.GetEnvironmentVariable ("HOME");
		public static readonly string AppDir = Home + Path.DirectorySeparatorChar + ".weekplanner";
		public static readonly string ConfigFile = AppDir + Path.DirectorySeparatorChar + "settings.xml";
		public static readonly string PluginsDir = AppDir + Path.DirectorySeparatorChar + "plugins";
		private static readonly char separator = Path.DirectorySeparatorChar;
		private static string pluginsFileName = "plugins.xml";

		static App ()
		{
			Init ();
			PluginManager = new PluginManager ();
		}

		public static void Init ()
		{
				FileInfo fi = new FileInfo (AppDir + separator + "firstTime");
				if (!fi.Exists)
				{
						DirectoryInfo di = new DirectoryInfo (AppDir);
						if (!di.Exists)
								di.Create ();
						di = new DirectoryInfo (PluginsDir);
						if (!di.Exists)
								di.Create ();

						//Copy the plugins.xml to userdir
						Assembly assembly = Assembly.GetCallingAssembly ();
						Stream s = assembly.GetManifestResourceStream (pluginsFileName);
						StreamReader sreader = new StreamReader (s);
						string xml = sreader.ReadToEnd ();
						sreader.Close ();
						StreamWriter writer = new StreamWriter (AppDir + separator + pluginsFileName);
						writer.Write (xml);
						writer.Close ();
						// Init finished. Create flag file
						fi.Create ();
				}
		}

		private static UserConfiguration _configuration;
		public static UserConfiguration Configuration {
			get {
				if (_configuration == null)
				{
					_configuration = (UserConfiguration)Utils.DeSerializeObject 
							(ConfigFile, typeof (UserConfiguration), false);
					if (_configuration == null)
							_configuration = new UserConfiguration ();
				}
				return _configuration;
			}
		}

		private static TaskSet _currentTaskSet;
		//FIXME:
		// Proper error checking
		public static TaskSet CurrentTaskSet {
				get {
						if (_currentTaskSet != null)
							return _currentTaskSet;
						else {
								FileStream stm;
								FileInfo fi = new FileInfo (AppDir + separator + Configuration.CurrentTaskSet);
								if (!fi.Exists)
										_currentTaskSet = new TaskSet (Configuration.CurrentTaskSet);
								else {
									stm = fi.Open (FileMode.OpenOrCreate);
									BinaryFormatter bf = new BinaryFormatter ();
									_currentTaskSet = (TaskSet) bf.Deserialize (stm);
								}
								return _currentTaskSet;
						}
							
				}

				set {
						_currentTaskSet = value;
						Configuration.CurrentTaskSet = _currentTaskSet.Name;
				}
		}

		public static PluginManager PluginManager;

		public static void SaveSettings ()
		{
				Utils.SerializeObject (App.Configuration, ConfigFile, false);
		}
			
			
	}
}

namespace SimpleP

import Nini.Config
import System
import System.IO
import System.Reflection

[DefaultMember ("Item")]
class ConfigurationService:
	
	configSource as IConfigSource
	appData = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData)
	configPath as string

	def constructor ():
		configPath = Path.Combine (appData, "SimpleP" + Path.DirectorySeparatorChar + "config.ini")
		CopyDefaultConfigFile () if not File.Exists (configPath)
		configSource = IniConfigSource (configPath)
	
	public MainConfig:
		get:
			return configSource.Configs["Main"]
	
	//Default config accessor
	public Item (key as string) as string:
		get:
			return MainConfig.Get (key)
		set:
			MainConfig.Set (key, value)

	def CreateConfig ([required]name as string):
		configSource.AddConfig (name)

	def GetConfig ([required]name as string) as IConfig:
		return configSource.Configs[name]

	def SaveConfig ():
		configSource.Save ()

	private def CopyDefaultConfigFile ():
		s = Globals.Resources.GetManifestResourceStream ("config.ini")
		reader = StreamReader (s)
		config = reader.ReadToEnd ()
		reader.Close ()
		writer = StreamWriter (FileStream (configPath, FileMode.CreateNew))
		writer.Write (config)
		writer.Close ()
	

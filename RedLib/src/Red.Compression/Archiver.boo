namespace Red.Compression

import System
import System.IO
import ICSharpCode.SharpZipLib.Zip
import System.Collections


public class Archiver:

	name as string

	public def constructor (name as string):
		self.name = name
	
	[Property (AppendDate)]
	_appendDate as bool = false
	
	[Property (AddEntryNotify)]
	_addEntryNotify as bool = false
	
	[Property (ExcludeEntryNotify)]
	_excludeEntryNotify as bool = false

	public def BuildZipFile (files as IList):
		ZipEntryAdded (files, null)

	public def BuildZipFile (files as IList, excludePatterns as IList) as string:
		if (_appendDate):
			name = name + "-${DateTime.Now.Year}-${DateTime.Now.Month}-${DateTime.Now.Day}-${DateTime.Now.Hour}_${DateTime.Now.Minute}"
		name = name + ".zip"

		stream = ZipOutputStream (File.Create (name))
		stream.SetLevel (5)
		for f as string in files:
			fs = File.OpenRead(f)
			entry = ZipEntry(ZipEntry.CleanName (f, true))
			if (excludePatterns != null and MatchExcludePattern (f, excludePatterns)):
				if (_excludeEntryNotify):
					args = ZipEntryExcludedArgs ()
					args.Entry = entry
					ZipEntryExcluded (self, args)
				continue
			
			buffer = array (byte, fs.Length)
			fs.Read(buffer, 0, buffer.Length)
			stream.PutNextEntry(entry)
			
			stream.Write(buffer, 0, buffer.Length)

			if (_addEntryNotify):
				args = ZipEntryExcludedArgs ()
				args.Entry = entry
				ZipEntryExcluded (self, args)
		stream.Finish ()
		stream.Close ()
		fs.Close ()
		return name

	private def MatchExcludePattern (f as string, patterns as IList) as bool:
		mustExclude = false
		for pat as string in patterns:
			if f.IndexOf (pat) != -1:
				mustExclude = true
			 
				break
		return mustExclude

	public event ZipEntryAdded as callable (object, ZipEntryAddedArgs)
	public event ZipEntryExcluded as callable (object, ZipEntryExcludedArgs)



public class ZipEntryAddedArgs (EventArgs) :

	public Entry as ZipEntry

public class ZipEntryExcludedArgs (EventArgs) :

	public Entry as ZipEntry

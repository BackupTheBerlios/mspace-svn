namespace Red.Resources

import System
import System.Reflection
import System.IO
import System.Diagnostics
import System.Resources
import System.Collections

public class ProcessBuilder:
"""Description of ProcessBuilder"""

	static tmpdir = Environment.GetEnvironmentVariable ("TMP")
	
	def constructor():
		pass
		
	public static def BuildProcessFromResource (resName as string, resFileName as string, asm as Assembly) as Process:
		assert resName != null and resFileName != null
		manager = ResourceManager (resFileName, asm)
		bytes as (byte) = manager.GetObject (resName) as (byte)
		stream = MemoryStream (bytes)
		process = BuildProcessFromStream (stream)
		stream.Close ()
		return process
		
			
	public static def BuildProcessFromResource (name as string, asm as Assembly) as Process:
		assert name != null
		stream = asm.GetManifestResourceStream (name)
		return BuildProcessFromStream (stream)
			
			
		
	private static def BuildProcessFromStream (stream as Stream) as Process:
		tmpFile = "RedLib.Process.${Environment.TickCount}.exe"
		assert stream != null
		file = Path.Combine (tmpdir, tmpFile)
		fs = FileStream (file, FileMode.Create)
		buffer = array(byte, 1024)
		while (stream.Read (buffer, 0, len (buffer)) != 0):
			fs.Write (buffer, 0, buffer.Length)
		fs.Close ()
		pinfo = ProcessStartInfo ()
		pinfo.FileName = file
		process = Process ()
		process.StartInfo = pinfo
		return process
			


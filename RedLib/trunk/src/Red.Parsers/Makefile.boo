import System
import System.IO
import System.Text.RegularExpressions
import System.Text
import System.Collections

class Makefile:
	
	static def Parse ([required]file as string) as Makefile:
		mk = Makefile ()
		reader = StreamReader (file)
		rx = @/(?<targets>^\S*:)/
		builder = StringBuilder ()
		while line = reader.ReadLine ():
			builder.Append (line)
			match = rx.Match (line)
			if match.Success:
				mk._targets.Add (match.ToString ())
		mk._content = builder.ToString ()
		reader.Close ()
		return mk

	_targets = []
	public Targets as ICollection:
		get:
			return _targets

	[Getter (Content)]
	_content as string

mk = Makefile.Parse ("Makefile")
print mk.Targets

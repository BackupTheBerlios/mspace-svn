FILES = src/Red.*/*.boo
all:
	booc $(FILES) -out:build/Red.dll -target:library -r:./ext/ICSharpCode.SharpZipLib.dll
	cp ext/* build/	

docs:
	monodocer --assembly:build/Red.dll --path:doc/

html-docs:
	monodocs2html --source:doc --dest:doc-html/

all-docs: docs html-docs

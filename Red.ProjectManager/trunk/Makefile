FILES = src/Red.*/*.boo
all:
	booc $(FILES) -out:build/Red.ProjectManager.dll -target:library -r:ext/Red.dll
	cp ext/* build/	

docs:
	monodocer --assembly:build/Red.ProjectManager.dll --path:doc/

html-docs: docs
	monodocs2html --source:doc --dest:doc-html/

all-docs: docs html-docs

FILES = src/Red.*/*.boo
all: update-refs
	booc $(FILES) -out:build/Red.ProjectManager.dll -target:library -r:ext/Red.dll
	cp ext/* build/	

docs:
	monodocer --assembly:build/Red.ProjectManager.dll --path:doc/

html-docs: docs
	monodocs2html --source:doc --dest:doc-html/

all-docs: docs html-docs

update-refs:
	cp ../../RedLib/trunk/build/Red.dll ext

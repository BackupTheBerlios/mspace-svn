ASSEMBLER = monodoc --assemble
UPDATER = monodoc --update

all: assemble

assemble: chicken-framework.zip chicken-framework.tree

chicken-framework.zip chicken-framework.tree: en/*/*.xml
	$(ASSEMBLER) --ecma $(srcdir)/en -o chicken-framework 

update:
	monodoc --update ../src/Chicken.dll -o ./en

docsdir = `monodoc --get-sourcesdir`
docs_DATA = chicken-framework.source chicken-framework.tree chicken-framework.zip

CLEANFILES = chicken-framework.tree chicken-framework.zip 

EXTRA_DIST =			\
	chicken-framework.source

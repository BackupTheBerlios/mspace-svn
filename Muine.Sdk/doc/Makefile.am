ASSEMBLER = monodoc --assemble
UPDATER = monodoc --update

all: assemble

assemble: muine-sdk.zip muine-sdk.tree

muine-sdk.zip muine-sdk.tree: en/*/*.xml
	$(ASSEMBLER) --ecma $(srcdir)/en -o muine-sdk 

update:
	monodoc --update ../src/Muine.Sdk.dll -o ./en

docsdir = `monodoc --get-sourcesdir`
docs_DATA = muine-sdk.source muine-sdk.tree muine-sdk.zip

CLEANFILES = muine-sdk.tree muine-sdk.zip 

EXTRA_DIST =			\
	muine-sdk.source

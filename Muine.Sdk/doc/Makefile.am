SUBDIRS = updater
ASSEMBLER = monodoc --assemble
UPDATER = mono updater/updater.exe

all: assemble

assemble: muine-sdk.zip muine-sdk.tree

muine-sdk.zip muine-sdk.tree: $(srcdir)/en/*/*.xml $(srcdir)/en/*.xml
	$(ASSEMBLER) --ecma $(srcdir)/en -o muine-sdk 

update:
	monodoc --update ../src/Muine.Sdk.dll -o ./en

CLEANFILES = muine-sdk.tree muine-sdk.zip 

EXTRA_DIST =			\
	muine-sdk.source

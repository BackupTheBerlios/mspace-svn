ASSEMBLER = monodoc --assemble
UPDATER = monodoc --update

all: assemble

assemble: chicken-framework.zip chicken-framework.tree

chicken-framework.zip chicken-framework.tree: en/*/*.xml en/*.xml
	$(ASSEMBLER) --ecma $(srcdir)/en -o chicken-framework 

update:
	$(UPDATER) ../src/Chicken.dll -o ./en

dist-hook:
	mkdir -p $(distdir)/en	
	cp -a $(srcdir)/en/* $(distdir)/en

docsdir = $(libdir)/monodoc/sources
docs_DATA = chicken-framework.source chicken-framework.tree chicken-framework.zip

CLEANFILES = chicken-framework.tree chicken-framework.zip 

EXTRA_DIST =			\
	chicken-framework.source

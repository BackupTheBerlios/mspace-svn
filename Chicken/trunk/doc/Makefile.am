ASSEMBLER = monodoc --assemble
UPDATER = monodoc --update
NAMESPACES=Chicken.Configuration Chicken.Gnome.TrayIcon Chicken.Gnome.Notification Nini.Util Nini.Ini Nini.Config

all: assemble

assemble: chicken-framework.zip chicken-framework.tree

chicken-framework.zip chicken-framework.tree: en/*/*.xml en/*.xml
	$(ASSEMBLER) --ecma $(srcdir)/en -o chicken-framework 

update:
	$(UPDATER) ../src/Chicken.dll -o ./en

dist-hook:
	mkdir -p $(distdir)/en
	cp en/*.xml $(distdir)/en/
	for i in $(NAMESPACES); do				\
		mkdir -p $(distdir)/en/$$i;			\
		cp $(srcdir)/en/$$i/*.xml $(distdir)/en/$$i;	\
	done

docsdir = $(libdir)/monodoc/sources
docs_DATA = chicken-framework.source chicken-framework.tree chicken-framework.zip

CLEANFILES = chicken-framework.tree chicken-framework.zip 

EXTRA_DIST =			\
	chicken-framework.source

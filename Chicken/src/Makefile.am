SUBDIRS = Gnome
include sources.list
REFERENCES = -pkg:gtk-sharp -pkg:gecko-sharp
	
Chicken.dll: $(CSSOURCES)
	mcs $(CSSOURCES) -o $@ -target:library $(REFERENCES)

all: Chicken.dll

frameworklibdir = $(libdir)
frameworklib_DATA = Chicken.dll Chicken.dll.config

pkgconfigdir = $(libdir)/pkgconfig
pkgconfig_DATA = chicken.pc

CLEANFILES = *.dll Chicken.dll.config
EXTRA_DIST = Chicken.dll.config.in $(CSSOURCES)

SUBDIRS = Gnome
include sources.list
REFERENCES = -pkg:gtk-sharp
	
FLFramework.dll: $(CSSOURCES)
	mcs $(CSSOURCES) -o $@ -target:library $(REFERENCES)

all: FLFramework.dll

frameworklibdir = $(libdir)
frameworklib_DATA = FLFramework.dll FLFramework.dll.config

pkgconfigdir = $(libdir)/pkgconfig
pkgconfig_DATA = fl-framework.pc

CLEANFILES = *.dll FLFramework.dll.config
EXTRA_DIST = FLFramework.dll.config.in $(CSSOURCES)

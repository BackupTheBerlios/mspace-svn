SUBDIRS = Gnome
include sources.list
REFERENCES = -pkg:gtk-sharp -pkg:gecko-sharp -pkg:gtkhtml-sharp -pkg:rsvg-sharp
RESOURCES = /resource:../images/Info.svg,Info.svg \
	/resource:../images/Warning.svg,Warning.svg \
	/resource:../images/Error.svg,Error.svg \
	/resource:../images/tray-icon.png,tray-icon.png
	
Chicken.dll: $(CSSOURCES)
	mcs $(CSSOURCES) -o $@ -target:library $(REFERENCES) $(RESOURCES)

all: Chicken.dll

frameworklibdir = $(libdir)
frameworklib_DATA = Chicken.dll Chicken.dll.config

pkgconfigdir = $(libdir)/pkgconfig
pkgconfig_DATA = chicken.pc

CLEANFILES = *.dll Chicken.dll.config
EXTRA_DIST = Chicken.dll.config.in $(CSSOURCES)

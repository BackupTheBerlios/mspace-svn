include sources.list
REFERENCES = -pkg:gtk-sharp
	
FLFramework.dll: $(CSSOURCES)
	mcs $(CSSOURCES) -o $@ -target:library $(REFERENCES)

all: FLFramework.dll

frameworklibdir = $(libdir)/FLFramework
frameworklib_DATA = FLFramework.dll

CLEANFILES = *.dll

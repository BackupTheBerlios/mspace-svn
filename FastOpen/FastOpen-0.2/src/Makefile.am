RESOURCES= 									\
	/resource:${top_srcdir}/data/fastopenrc,fastopenrc			\
	/resource:${top_srcdir}/data/fastopen.png,fastopen.png
all: fastopen.exe

fastopen.exe: *.cs
	mcs -o $@ *.cs -pkg:gtk-sharp -pkg:gnome-sharp -pkg:glade-sharp $(RESOURCES)

fastopenlibdir = ${prefix}/lib/fastopen
fastopenlib_DATA = fastopen.exe fseditor.exe

EXTRA_DIST =				\
	*.cs				\	
	fseditor.exe

CLEANFILES =				\
	fastopen.exe

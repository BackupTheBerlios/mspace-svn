RESOURCES= 									\
	/resource:${top_srcdir}/glade/fastopen.glade,fastopen.glade 		\
	/resource:${top_srcdir}/data/fastopenrc,fastopenrc			\
	/resource:${top_srcdir}/data/fastopen.png,fastopen.png
all: fastopen.exe

fastopen.exe: *.cs
	mcs -o $@ *.cs -pkg:gtk-sharp -pkg:gnome-sharp -pkg:glade-sharp $(RESOURCES)

fastopenlibdir = ${prefix}/lib/fastopen
fastopenlib_DATA = fastopen.exe

EXTRA_DIST =				\
	*.cs				

CLEANFILES =				\
	fastopen.exe

EXTRA_DIST = eggtrayicon.h

INCLUDES = \
  $(LIBTRAYICON_CFLAGS) \
  -DEGG_COMPILATION \
  -DGTK_DISABLE_DEPRECATED \
  -DGDK_DISABLE_DEPRECATED \
  -DG_DISABLE_DEPRECATED

chickenlibdir = $(libdir)
chickenlib_LTLIBRARIES = libchicken.la

libchicken_la_SOURCES = \
  eggtrayicon.c		

libchicken_la_LDFLAGS = -export-dinamyc -module -avoid-version
libchicken_la_LIBADD = $(LIBTRAYICON_LIBS)

maintainer-clean-local:
	rm -f Makefile.in

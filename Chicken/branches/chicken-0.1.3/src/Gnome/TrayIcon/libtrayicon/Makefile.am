TRAY_ICON_CFLAGS = $(GDK20_CFLAGS) $(ATK_CFLAGS)
TRAY_ICON_LIBS = $(GDK20_LIBS) $(ATK_LIBS)

EXTRA_DIST = eggtrayicon.h

INCLUDES = \
  $(TRAY_ICON_CFLAGS) \
  -DEGG_COMPILATION \
  -DGTK_DISABLE_DEPRECATED \
  -DGDK_DISABLE_DEPRECATED \
  -DG_DISABLE_DEPRECATED

trayiconlibdir = $(libdir)
trayiconlib_LTLIBRARIES = libchicken.la

libchicken_la_SOURCES = \
  eggtrayicon.c		

maintainer-clean-local:
	rm -f Makefile.in

PREFIXDIR  = $(prefix)
PKGDATADIR = $(datadir)/matchbox
DATADIR    = $(datadir)

INCLUDES = -DDATADIR=\"$(DATADIR)\" -DPKGDATADIR=\"$(PKGDATADIR)\" -DPREFIX=\"$(PREFIXDIR)\"

bindir = $(libdir)/WebNotes
bin_PROGRAMS = didiwiki

didiwiki_SOURCES = didi.c  didi.h  http.c  http.h  \
                   util.c  util.h  wiki.c  wiki.h  \
                   wikitext.h





SUBDIRS = 		\
	data		\
	src

bin_SCRIPTS = fastopen fseditor

EXTRA_DIST =				\
	fastopen.in			\
	fseditor.in			\
	NEWS				\
	README				\
	TODO

CLEANFILES = fastopen fseditor

release: dist
	scp FastOpen-$(VERSION).tar.gz shell.berlios.de:~/htdocs/FastOpen/Releases

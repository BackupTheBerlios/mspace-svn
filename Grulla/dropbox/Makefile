VERSION=1.1
PACKAGE=dropbox-$(VERSION).tar.gz

tarball: 
	tar czvf $(PACKAGE) create_dropbox.sh index.html html.css images dropbox.xml
release: tarball
	scp $(PACKAGE) shell.berlios.de:~/Grulla/dropbox

SUBDIRS = src doc images

release: dist
	scp $(PACKAGE_NAME)-$(VERSION).tar.gz shell.berlios.de:~/htdocs/$(PACKAGE_NAME)/Releases

SUBDIRS = src doc images
TMPDIR=/tmp/chicken-$(VERSION)
SRC=chicken-$(VERSION)-src.tar.gz

RELDIR=~/public_html/Projects/Chicken-Framework/Releases
UPDIR=~/public_html/gunther-user/uploads/pages/Chicken-Framework

push: all dist
	mv chicken-$(VERSION).tar.gz $(SRC)
	cp chicken*.tar.gz $(RELDIR)
	cp $(RELDIR)/chicken-$(VERSION)*.tar.gz  $(UPDIR)/

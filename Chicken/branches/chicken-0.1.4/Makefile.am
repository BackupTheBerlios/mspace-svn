SUBDIRS = src doc images examples

RELDIR=~/public_html/Projects/Chicken-Framework/Releases
UPDIR=~/public_html/gunther-user/uploads/pages/Chicken-Framework

push: all dist
	cp chicken*.tar.gz $(RELDIR)
	cp $(RELDIR)/chicken-$(VERSION)*.tar.gz  $(UPDIR)/

SUBDIRS = images src
TMPDIR=/tmp/MonkeyPop-$(VERSION)
BIN=monkeypop-$(VERSION)-bin.tar.gz
SRC=monkeypop-$(VERSION)-src.tar.gz

RELDIR=~/public_html/Projects/MonkeyPop/Releases
UPDIR=~/public_html/gunther-user/uploads/pages/MonkeyPop

bin: all
	mkdir $(TMPDIR)
	cp src/MonkeyPop.exe $(TMPDIR)
	cp README NEWS AUTHORS COPYING $(TMPDIR)
	tar czvf monkeypop-$(VERSION)-bin.tar.gz $(TMPDIR)
	rm -rf $(TMPDIR)

push: all dist bin
	mv monkeypop-$(VERSION).tar.gz $(SRC)
	cp monkeypop*.tar.gz ~/public_html/Projects/MonkeyPop/Releases
	cp $(RELDIR)/monkeypop-$(VERSION)*.tar.gz  $(UPDIR)/
	

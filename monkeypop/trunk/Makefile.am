SUBDIRS = images src

if ENABLE_DBUS
SUBDIRS += monkeypopd
endif

TMPDIR=/tmp/MonkeyPop-$(VERSION)
BIN=monkeypop-$(VERSION)-bin.tar.gz
SRC=monkeypop-$(VERSION)-src.tar.gz

RELDIR=~/public_html/Projects/MonkeyPop/Releases
UPDIR=~/public_html/gunther-user/uploads/pages/MonkeyPop

push: all dist
	cp monkeypop*.tar.gz ~/public_html/Projects/MonkeyPop/Releases
	cp $(RELDIR)/monkeypop-$(VERSION)*.tar.gz  $(UPDIR)/
	

CSSOURCES = MonkeyPop.cs
REFERECES = $(CHICKEN_LIBS) $(GTKSHARP_LIBS) -r:Mono.GetOptions

#RESOURCES = \
#	/resource:../images/notification.png,notification.png \
#	/resource:../images/simple.svg,simple.svg				\
#	/resource:../images/notification-white.png,notification-white.png

all: MonkeyPop.exe

MonkeyPop.exe: $(CSSOURCES)
	mcs $(CSSOURCES) -o $(@) -target:exe $(REFERECES) $(RESOURCES)

monkeypopdir = $(prefix)/lib/MonkeyPop
monkeypop_DATA = MonkeyPop.exe

bin_SCRIPTS = monkeypop

CLEANFILES = *.exe monkeypop
EXTRA_DIST = monkeypop.in $(CSSOURCES)

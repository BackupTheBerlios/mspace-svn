CSC = $(MCS) -codepage:utf8 $(MCS_FLAGS)

include sources.list

RESOURCES = \
/resource:images/muine-about.png,muine-about.png	\
/resource:images/muine-nothing.png,muine-nothing.png	\
/resource:images/muine.png,muine.png	\
/resource:glade/AddWindow.glade,AddWindow.glade	\
/resource:glade/ErrorDialog.glade,ErrorDialog.glade	\
/resource:glade/GroupEditor.glade,GroupEditor.glade	\
/resource:glade/InfoWindow.glade,InfoWindow.glade	\
/resource:glade/NoMusicFoundWindow.glade,NoMusicFoundWindow.glade	\
/resource:glade/PlaylistFilling.glade,PlaylistFilling.glade	\
/resource:glade/PlaylistWindow.glade,PlaylistWindow.glade	\
/resource:glade/ProgressWindow.glade,ProgressWindow.glade	\
/resource:glade/SkipToWindow.glade,SkipToWindow.glade	\
/resource:glade/YesNoDialog.glade,YesNoDialog.glade

ASSEMBLIES = /r:../Sdk/Muine.Sdk.dll

muine.exe: $(SOURCES)
	$(CSC) -o $@ $(CSFLAGS) $(SOURCES) $(ASSEMBLIES) $(RESOURCES) $(GTKSHARP_LIBS)

all: muine.exe

muinelibdir = $(prefix)/lib/muine
muinelib_DATA = muine.exe muine.exe.config

bin_SCRIPTS = muine

EXTRA_DIST =					\
	$(SOURCES)				\
	muine.in				\
	muine.exe.config.in

CLEANFILES =					\
	muine					\
	muine.exe

CSC = $(MCS) -codepage:utf8 $(MCS_FLAGS)

SOURCES = \
./About.cs	\
./AddAlbumWindow.cs	\
./AddSongWindow.cs	\
./ColoredCellRendererPixbuf.cs	\
./CoverImage.cs	\
./EllipsizingLabel.cs	\
./FileSelector.cs	\
./HandleView.cs	\
./InfoWindow.cs	\
./KeyUtils.cs	\
./MessageConnection.cs	\
./MmKeys.cs	\
./Muine.cs	\
./NoMusicFoundWindow.cs	\
./NotificationAreaIcon.cs	\
./Player.cs	\
./PlaylistWindow.cs	\
./ProgressWindow.cs	\
./SkipToWindow.cs	\
./VolumeButton.cs	\
./YesNoDialog.cs

RESOURCES = \
/resource:./images/muine-about.png	\
/resource:./images/muine-nothing.png	\
/resource:./images/muine.png	\
/resource:./glade/AddWindow.glade	\
/resource:./glade/ErrorDialog.glade	\
/resource:./glade/GroupEditor.glade	\
/resource:./glade/InfoWindow.glade	\
/resource:./glade/NoMusicFoundWindow.glade	\
/resource:./glade/PlaylistFilling.glade	\
/resource:./glade/PlaylistWindow.glade	\
/resource:./glade/ProgressWindow.glade	\
/resource:./glade/SkipToWindow.glade	\
/resource:./glade/YesNoDialog.glade

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

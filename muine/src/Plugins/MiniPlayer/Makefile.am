ASSEMBLIES = /r:../../Sdk/Muine.Sdk.dll
RESOURCES =					\
	/resource:glade/miniplayer.glade,miniplayer.glade

all: MiniPlayer.dll

MiniPlayer.dll: *.cs
	mcs /target:library -o $@ $(ASSEMBLIES) *.cs $(RESOURCES) -pkg:gtk-sharp -pkg:glade-sharp

miniplayerpluginlibdir = @prefix@/lib/muine/plugins
miniplayerpluginlib_DATA = MiniPlayer.dll

CLEANFILES =				\
	MiniPlayer.dll

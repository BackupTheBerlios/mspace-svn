ASSEMBLIES = /r:../../Sdk/Muine.Sdk.dll
RESOURCES =					\
	/resource:glade/monitor-window.glade,monitor-window.glade

all: MonitorPlugin.dll

MonitorPlugin.dll: *.cs
	mcs /target:library -o $@ $(ASSEMBLIES) *.cs $(RESOURCES) -pkg:gtk-sharp -pkg:glade-sharp

monitorpluginlibdir = ~/.gnome2/muine/plugins
monitorpluginlib_DATA = MonitorPlugin.dll

CLEANFILES =				\
	MonitorPlugin.dll

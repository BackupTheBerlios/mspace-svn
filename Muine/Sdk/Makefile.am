include sources.list

RESOURCES = \
/resource:images/muine-add-album-16.png,muine-add-album-16.png	\
/resource:images/muine-add-album.png,muine-add-album.png	\
/resource:images/muine-cover-downloading.png,muine-cover-downloading.png	\
/resource:images/muine-default-cover.png,muine-default-cover.png	\
/resource:images/muine-forward.png,muine-forward.png	\
/resource:images/muine-next-16.png,muine-next-16.png	\
/resource:images/muine-next.png,muine-next.png	\
/resource:images/muine-pause-16.png,muine-pause-16.png	\
/resource:images/muine-pause.png,muine-pause.png	\
/resource:images/muine-paused.png,muine-paused.png	\
/resource:images/muine-play-16.png,muine-play-16.png	\
/resource:images/muine-play.png,muine-play.png	\
/resource:images/muine-playing.png,muine-playing.png	\
/resource:images/muine-previous-16.png,muine-previous-16.png	\
/resource:images/muine-previous.png,muine-previous.png	\
/resource:images/muine-queue.png,muine-queue.png	\
/resource:images/muine-rewind.png,muine-rewind.png	\
/resource:images/muine-tray-paused.png,muine-tray-paused.png	\
/resource:images/muine-tray-playing.png,muine-tray-playing.png	\
/resource:images/muine-volume-max.png,muine-volume-max.png	\
/resource:images/muine-volume-medium.png,muine-volume-medium.png	\
/resource:images/muine-volume-min.png,muine-volume-min.png	\
/resource:images/muine-volume-zero.png,muine-volume-zero.png	\
/resource:glade/ErrorDialog.glade,ErrorDialog.glade

# FIXME MONO_XMLSERIALIZER_THS=no works around Ximian Bugzilla #58194
Muine/AmazonSearchService.cs: Muine/AmazonSearchService.wsdl
	MONO_XMLSERIALIZER_THS=no wsdl Muine/AmazonSearchService.wsdl -o:Muine/AmazonSearchService.cs

Muine.Sdk.dll: Muine/AmazonSearchService.cs $(SOURCES)
	mcs -g -target:library -pkg:gtk-sharp -pkg:gnome-sharp -pkg:gconf-sharp -pkg:glade-sharp /r:System.Web.Services /define:DEBUG_PLUGINS -o Muine.Sdk.dll /recurse:$(SOURCES) $(RESOURCES)

all: Muine.Sdk.dll

muinelibdir = $(prefix)/lib/muine
muinelib_DATA = Muine.Sdk.dll.config Muine.Sdk.dll

EXTRA_DIST =					\
	#$(SOURCES)				\
	Muine/AmazonSearchService.wsdl		\
	Muine.Sdk.dll.config.in

CLEANFILES =					\
	Muine/AmazonSearchService.cs		\
	Muine.Sdk.dll				\
	Muine.Sdk.dll.config

SOURCES=App/*.cs
FRAMEWORK_SOURCES= Sdk/Plugin/*.cs		\
		   Sdk/Event/*.cs		\
		   Sdk/Action/*.cs		\
		   Sdk/Event.BusMembers/*.cs		\
		   Sdk/Event.Filters/*.cs		\
		   Sdk/Muine/*.cs		\
		   Sdk/Bus/*.cs	
		   

RESOURCES =												\
	/resource:glade/PlaylistWindow.glade,PlaylistWindow.glade			\
	/resource:glade/SkipToWindow.glade,SkipToWindow.glade			\
	/resource:glade/NoMusicFoundWindow.glade,NoMusicFoundWindow.glade		\
	/resource:glade/ErrorDialog.glade,ErrorDialog.glade				\
	/resource:glade/YesNoDialog.glade,YesNoDialog.glade				\
	/resource:glade/AddWindow.glade,AddWindow.glade				\
	/resource:glade/ProgressWindow.glade,ProgressWindow.glade			\
	/resource:glade/InfoWindow.glade,InfoWindow.glade				\
	/resource:images/muine-nothing.png,muine-nothing.png				\
	/resource:images/muine-about.png,muine-about.png				\
	/resource:images/muine.png,muine.png						

FRAMEWORK_RESOURCES =												\
	/resource:glade/ErrorDialog.glade,ErrorDialog.glade				\
	/resource:images/muine-previous.png,muine-previous.png			\
	/resource:images/muine-previous-16.png,muine-previous-16.png			\
	/resource:images/muine-play.png,muine-play.png				\
	/resource:images/muine-play-16.png,muine-play-16.png				\
	/resource:images/muine-pause.png,muine-pause.png				\
	/resource:images/muine-pause-16.png,muine-pause-16.png			\
	/resource:images/muine-next.png,muine-next.png				\
	/resource:images/muine-next-16.png,muine-next-16.png				\
	/resource:images/muine-rewind.png,muine-rewind.png				\
	/resource:images/muine-forward.png,muine-forward.png				\
	/resource:images/muine-queue.png,muine-queue.png				\
	/resource:images/muine-add-album.png,muine-add-album.png			\
	/resource:images/muine-add-album-16.png,muine-add-album-16.png		\
	/resource:images/muine-playing.png,muine-playing.png				\
	/resource:images/muine-paused.png,muine-paused.png				\
	/resource:images/muine-tray-playing.png,muine-tray-playing.png		\
	/resource:images/muine-tray-paused.png,muine-tray-paused.png			\
	/resource:images/muine-default-cover.png,muine-default-cover.png		\
	/resource:images/muine-cover-downloading.png,muine-cover-downloading.png	\
	/resource:images/muine-volume-zero.png,muine-volume-zero.png			\
	/resource:images/muine-volume-min.png,muine-volume-min.png			\
	/resource:images/muine-volume-medium.png,muine-volume-medium.png		\
	/resource:images/muine-volume-max.png,muine-volume-max.png

all: AmazonSearchService.dll framework muine.exe

framework: AmazonSearchService.dll $(FRAMEWORK_SOURCES)
	mcs -g -target:library -pkg:gtk-sharp -pkg:gnome-sharp -pkg:gconf-sharp -pkg:glade-sharp /r:AmazonSearchService.dll /define:DEBUG_PLUGINS -o Muine.Sdk.dll /recurse:$(FRAMEWORK_SOURCES) $(FRAMEWORK_RESOURCES)


muine.exe: $(SOURCES)
	mcs -debug+ $(SOURCES) $(RESOURCES) /r:Muine.Sdk.dll -pkg:gtk-sharp -pkg:gnome-sharp -pkg:glade-sharp -pkg:gconf-sharp /r:AmazonSearchService.dll -r:System.Web.Services -o muine.exe
	
AmazonSearchService.cs: App/AmazonSearchService.wsdl
	        MONO_XMLSERIALIZER_THS=no wsdl App/AmazonSearchService.wsdl

AmazonSearchService.dll: App/AmazonSearchService.cs
	        mcs -target:library $(LIBFLAGS) -out:AmazonSearchService.dll App/AmazonSearchService.cs -r:System.Web.Services

#install:
#	cp muine.exe /usr/local/lib/muine

devel: muine.exe
	sh ./install-devel.sh

clean:
	rm *.exe
clean-all: clean
	rm *.dll

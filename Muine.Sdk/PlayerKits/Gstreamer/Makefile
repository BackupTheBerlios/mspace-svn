SOURCES = player-gst.c

kit:
	mcs GstPlayer.cs /target:library -pkg:gtk-sharp /r:../../src/Muine.Player.Sdk.dll
all: kit
	gcc -o libgstplayer.so -fPIC -shared $(SOURCES) `pkg-config --libs --cflags gstreamer-0.8 gstreamer-gconf-0.8 gstreamer-libs-0.8 gstreamer-play-0.8` 
run: 
	LD_LIBRARY_PATH=/home/rubiojr/svn/mspace/Muine.Sdk/PlayerKits/Gstreamer/:/opt/local/lib:/opt/mono/lib MONO_PATH=../../src/:/home/rubiojr/monolibs/lib:/home/rubiojr/monolibs/lib::/usr/local/lib:/opt/mono-extra/lib:/usr/local/lib:/opt/mono-extra/lib mono GstPlayer.exe

REFERENCES = -pkg:gtk-sharp /r:$(top_srcdir)/src/Muine.Sdk.dll

SOURCES = GstPlayer.cs

GstPlayer.dll: $(SOURCES)
	mcs /out:$@ $(SOURCES) /target:library $(REFERENCES)

#FIXME: move pkg-config stuff to configure
libgstplayer.so: player-gst.c player.h
	gcc -o $@ -shared -fPIC player-gst.c `pkg-config --cflags --libs gstreamer-0.8 gstreamer-gconf-0.8 gstreamer-play-0.8`

all: GstPlayer.dll libgstplayer.so

libgstplayerdir = $(prefix)/lib/Muine.Sdk/PlayerKits
libgstplayer_DATA = GstPlayer.dll libgstplayer.so GstPlayer.dll.config

EXTRA_DIST = GstPlayer.dll.config

CLEANFILES = GstPlayer.dll libgstplayer.so

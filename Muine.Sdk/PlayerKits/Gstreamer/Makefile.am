REFERENCES = -pkg:gtk-sharp /r:$(top_srcdir)/src/Muine.Sdk.dll

CSSOURCES = GstPlayer.cs
CSOURCES = player-gst.c
CHEADERS = player.h

GstPlayer.dll: $(CSSOURCES)
	mcs /out:$@ $(CSSOURCES) /target:library $(REFERENCES)

#FIXME: move pkg-config stuff to configure
libgstplayer.so: $(CSOURCES) $(CHEADERS)
	gcc -o $@ -shared -fPIC player-gst.c `pkg-config --cflags --libs gstreamer-0.8 gstreamer-gconf-0.8 gstreamer-play-0.8`

all: GstPlayer.dll libgstplayer.so

libgstplayerdir = $(prefix)/lib/Muine.Sdk/PlayerKits
libgstplayer_DATA = GstPlayer.dll libgstplayer.so GstPlayer.dll.config

EXTRA_DIST = GstPlayer.dll.config $(CSSOURCES) $(CSOURCES) $(CHEADERS)

CLEANFILES = GstPlayer.dll libgstplayer.so

SOURCES = player-gst.c player.h
all: 
	gcc -o libgstplayer.so -shared $(SOURCES) `pkg-config --libs --cflags gstreamer-0.8 gstreamer-gconf-0.8` 

#!/bin/bash
SOURCE="$1"
DEST="$2"
VIDEO_EXT="\.avi|\.mpg|\.mpeg|\.divx|\.xvid|\.wmv"
MUSIC_EXT="\.mp3|\.ogg"
EXT=$VIDEO_EXT
case $1 in
    -music)
	EXT=$MUSIC_EXT
	SOURCE=$2
	DEST=$3
	;;
    *)
	;;
esac

find "$SOURCE" | grep -E $EXT$ |
while read line
do
    md5=`head -c 512k "$line" | md5sum | sed -e 's/-$//'`
    echo "$md5 $line" >> source.hash
done

find "$DEST" | grep -E $EXT$ |
while read line
do
    md5=`head -c 512k "$line" | md5sum | sed -e 's/-$//'`
    echo "$md5 $line" >> dest.hash
done

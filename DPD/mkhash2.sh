#!/bin/bash
#
# Script to create a file containing md5sum and filenames of each file
# found in <source dir> and <dest dir>
# Example:
#   mkhash2.sh Movies1/ Movies2/
# 
# It will create 2 files, source.hash and dest.hash
#
# dpd can process this files and synchronize both <source dir> and <dest dir>
# The -music flag should be used if you want to compare directories containing
# music files
#

SOURCE="$1"
DEST="$2"
VIDEO_EXT="\.avi|\.mpg|\.mpeg|\.divx|\.xvid|\.wmv"
MUSIC_EXT="\.mp3|\.ogg"
EXT=$VIDEO_EXT

#FIXME: check arguments properly
if test -z $1 || test -z $2; then
    echo "Usage: mkhash2.sh [-music] <source dir> <dest dir>"

else
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
fi

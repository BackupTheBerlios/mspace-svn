#!/bin/bash
DIR="$1"
FILE="$2"
EXT=".mp3"
find "$DIR" | grep $EXT$ |
while read line
do
    md5sum "$line" >> $FILE
done
#for i in `find "$DIR" | grep $EXT`; do
    ##md5sum "$i" >> hash.list
    #echo "$i"
#done



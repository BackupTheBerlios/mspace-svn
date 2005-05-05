#!/bin/sh

FILEPATH=$1
FILENAME=$2

## Copy the file to the server using kfmclient
kfmclient copy $FILEPATH fish://rubiojr.no-ip.org/home/rubiojr/public_html/Download/

## Set the URL in clipboard
dcop klipper klipper setClipboardContents "http://rubiojr.no-ip.org/~rubiojr/Download/$FILENAME"

## Notify when finished
dcop knotify Notify notify "URL received" "<b>Web Uploader</b>" "Download URL <b><font color='red'>ready</font></b> in klipper" "" "" 16 0

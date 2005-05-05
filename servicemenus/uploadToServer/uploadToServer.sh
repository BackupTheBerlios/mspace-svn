#!/bin/sh

FILEPATH=$1
FILENAME=$2
SERVER_URL="fish://rubiojr.no-ip.org/home/rubiojr/public_html/Download/"
DOWNLOAD_URL="http://rubiojr.no-ip.org/~/rubiojr/Download/$FILENAME"

## Copy the file to the server using kfmclient
kfmclient copy $FILEPATH $SERVERURL

## Set the URL in clipboard
dcop klipper klipper setClipboardContents "$DOWNLOAD_URL"

## Notify when finished
dcop knotify Notify notify "URL received" "<b>File Uploader</b>" "Download URL <b><font color='red'>ready</font></b> in klipper" "" "" 16 0

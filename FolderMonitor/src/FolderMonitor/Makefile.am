RESOURCES = \
	    /resource:../../images/dropbox-changed.png,dropbox-changed.png \
	    /resource:../../images/dropbox-changed-red.png,dropbox-changed-red.png

FolderMonitor.exe: FolderMonitor.cs
	mcs FolderMonitor.cs -pkg:gtk-sharp /r:../TrayIcon/TrayIcon.dll $(RESOURCES) -o $@

all: FolderMonitor.exe

bin_SCRIPTS = foldermonitor

monitorlibdir = $(prefix)/lib/FolderMonitor
monitorlib_DATA = FolderMonitor.exe

EXTRA_DIST = FolderMonitor.cs foldermonitor.in
CLEANFILES = *.exe foldermonitor


RESOURCES = /resource:../../images/dropbox-changed.png,dropbox-changed.png

FolderMonitor.exe: FolderMonitor.cs
	mcs FolderMonitor.cs -pkg:gtk-sharp /r:../TrayIcon/TrayIcon.dll $(RESOURCES) -o $@

all: FolderMonitor.exe

bin_SCRIPTS = foldermonitor

monitorlibdir = $(prefix)/lib/FolderMonitor
monitorlib_DATA = FolderMonitor.exe

EXTRA_DIST = FolderMonitor.cs foldermonitor.in
CLEANFILES = *.exe foldermonitor


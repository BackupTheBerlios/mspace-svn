RESOURCES = \
	    /resource:../../images/dropbox-changed.png,dropbox-changed.png \
	    /resource:../../images/dropbox-changed-red.png,dropbox-changed-red.png

REFERENCES = -pkg:fl-framework -pkg:gtk-sharp
FolderMonitor.exe: FolderMonitor.cs
	mcs FolderMonitor.cs $(RESOURCES) $(REFERENCES) -o $@

all: FolderMonitor.exe

bin_SCRIPTS = foldermonitor

monitorlibdir = $(prefix)/lib/FolderMonitor
monitorlib_DATA = FolderMonitor.exe

EXTRA_DIST = FolderMonitor.cs foldermonitor.in
CLEANFILES = *.exe foldermonitor


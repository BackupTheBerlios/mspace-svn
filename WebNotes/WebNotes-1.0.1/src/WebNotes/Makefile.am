include sources.list
include ../../images/imagers.list
include ../../glade/gladers.list

#MISCRS = /resource:../MuineSdk.ini,MuineSdk.ini
REFERENCES = -pkg:gtk-sharp -pkg:gecko-sharp /r:../TrayIcon/TrayIcon.dll -pkg:glade-sharp

WebNotes.exe: $(CSSOURCES) 
	mcs /out:$@ $(CSSOURCES) /target:exe $(REFERENCES) $(IMAGERS) $(GLADERS) /resource:WikiHome,WikiHome

all: WebNotes.exe 

bin_SCRIPTS = webnotes

sdklibdir = $(prefix)/lib/WebNotes
sdklib_DATA = WebNotes.exe


EXTRA_DIST = sources.list webnotes.in $(CSSOURCES) WikiHome
CLEANFILES = WebNotes.exe webnotes

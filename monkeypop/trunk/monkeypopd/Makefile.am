CSFILES=*.cs
REFERENCES=-pkg:gtk-sharp -pkg:chicken -pkg:dbus-sharp

all: notification-daemon.exe

notification-daemon.exe: $(CSFILES)
	mcs $(CSFILES) -o $@ $(REFERENCES)

EXTRA_DIST = *.cs
CLEANFILES= *.exe

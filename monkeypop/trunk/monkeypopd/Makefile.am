REFERENCES= \
	    $(GTKSHARP_LIBS)		\
	    $(CHICKEN_LIBS)	 	\
	    $(DBUS_SHARP_LIBS)

all: NotificationDaemon.dll notification-daemon.exe client.exe

NotificationDaemon.dll: NotificationDaemon.cs
	mcs NotificationDaemon.cs -t:library -o $@ $(REFERENCES)

notification-daemon.exe: DaemonLauncher.cs
	mcs DaemonLauncher.cs -o $@ $(REFERENCES) -r:NotificationDaemon.dll

client.exe: Client.cs
	mcs Client.cs -o $@ $(REFERENCES) -r:NotificationDaemon.dll

EXTRA_DIST = *.cs
CLEANFILES= *.exe *.dll

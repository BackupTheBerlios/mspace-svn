MUINE_SDK = ../Muine.Sdk.dll
TESTSOURCES = \
	PlaylistTest.cs

all: databasetest.exe playertest.exe
	
test:
	mcs $(TESTSOURCES) /target:library /r:nunit.framework /r:../Muine.Sdk.dll -o Tests.dll -pkg:gtk-sharp

databasetest.exe: DatabaseTest.cs
	mcs DatabaseTest.cs /target:exe /r:$(MUINE_SDK) -o $@ -pkg:gtk-sharp

playertest.exe: PlayerTest.cs
	mcs PlayerTest.cs /target:exe /r:$(MUINE_SDK) -o $@ -pkg:gtk-sharp
EXTRA_DIST =					\
	*.cs

CLEANFILES =					\
	*.exe

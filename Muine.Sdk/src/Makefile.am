include sources.list

MISCRS = /resource:../MuineSdk.ini,MuineSdk.ini
REFERENCES = -pkg:gtk-sharp /r:../ext/Nini.dll

Muine.Player.Sdk.dll: $(SOURCES)
	mcs /out:$@ $(SOURCES) /target:library $(REFERENCES) $(MISCRS)

all: Muine.Player.Sdk.dll

EXTRA_DIST =

CLEANFILES = Muine.Player.Sdk.dll

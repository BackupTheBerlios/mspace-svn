include sources.list

REFERENCES = -pkg:gtk-sharp

Muine.Player.Sdk.dll: $(SOURCES)
	mcs /out:$@ $(SOURCES) /target:library $(REFERENCES)

all: Muine.Player.Sdk.dll

EXTRA_DIST =

CLEANFILES = Muine.Player.Sdk.dll

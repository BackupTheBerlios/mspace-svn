include sources.list

MISCRS = /resource:../MuineSdk.ini,MuineSdk.ini
REFERENCES = -pkg:gtk-sharp

Muine.Sdk.dll: $(SOURCES)
	mcs /out:$@ $(SOURCES) /target:library $(REFERENCES) $(MISCRS)

all: Muine.Sdk.dll

sdklibdir = $(prefix)/lib
sdklib_DATA = Muine.Sdk.dll muine-sdk.pc

EXTRA_DIST = sources.list muine-sdk.pc.in

CLEANFILES = Muine.Sdk.dll muine-sdk.pc

include sources.list

MISCRS = /resource:../MuineSdk.ini,MuineSdk.ini
REFERENCES = -pkg:chicken

Muine.Sdk.dll: $(CSSOURCES)
	mcs /out:$@ $(CSSOURCES) /target:library $(MISCRS) $(REFERENCES)

all: Muine.Sdk.dll

sdklibdir = $(prefix)/lib
sdklib_DATA = Muine.Sdk.dll
pkgconfigdir = $(libdir)/pkgconfig
pkgconfig_DATA = muine-sdk.pc

EXTRA_DIST = sources.list muine-sdk.pc.in $(CSSOURCES)
CLEANFILES = Muine.Sdk.dll muine-sdk.pc

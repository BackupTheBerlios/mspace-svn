include sources.list

MISCRS = /resource:../PlayerSdk.ini,PlayerSdk.ini
REFERENCES = -pkg:chicken

Player.Sdk.dll: $(CSSOURCES)
	mcs /out:$@ $(CSSOURCES) /target:library $(MISCRS) $(REFERENCES)

all: Player.Sdk.dll

sdklibdir = $(prefix)/lib
sdklib_DATA = Player.Sdk.dll
pkgconfigdir = $(libdir)/pkgconfig
pkgconfig_DATA = player-sdk.pc

EXTRA_DIST = sources.list player-sdk.pc.in $(CSSOURCES)
CLEANFILES = Player.Sdk.dll player-sdk.pc

REFERENCES= /r:Mono.Data.SqliteClient /r:../../src/Muine.Sdk.dll

all: SqliteDataKit.dll

SqliteDataKit.dll: *.cs
	mcs -o $@ -target:library *.cs $(REFERENCES)

kitdir = $(prefix)/lib/Muine.Sdk/DataKits
kit_DATA = SqliteDataKit.dll

CLEANFILES= *.dll
EXTRA_DIST= *.cs

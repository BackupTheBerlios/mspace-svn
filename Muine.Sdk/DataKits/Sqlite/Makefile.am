REFERENCES= /r:Mono.Data.SqliteClient /r:../../src/Player.Sdk.dll

all: SqliteDataKit.dll

SqliteDataKit.dll: *.cs
	mcs -o $@ -target:library *.cs $(REFERENCES)

kitdir = $(prefix)/lib/Player.Sdk/DataKits
kit_DATA = SqliteDataKit.dll

CLEANFILES= *.dll
EXTRA_DIST= *.cs

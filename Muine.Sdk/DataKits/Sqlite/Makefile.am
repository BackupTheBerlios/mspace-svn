REFERENCES= /r:Mono.Data.SqliteClient /r:../../src/Muine.Sdk.dll

all: SqliteDataKit.dll

SqliteDataKit.dll: *.cs
	mcs -o $@ -target:library *.cs $(REFERENCES)

CLEANFILES= *.dll
EXTRA_DIST= *.cs

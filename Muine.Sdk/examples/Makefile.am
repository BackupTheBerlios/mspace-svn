REFERENCES =				\
	-pkg:gtk-sharp				\
	-r:../src/Muine.Sdk.dll

all: db-test.exe

db-test.exe: DbTest.cs
	mcs DbTest.cs -o $@ $(REFERENCES)

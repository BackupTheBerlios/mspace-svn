all: gksu gksuui
	cp *.config bin/

gksu: gksu/generated/*.cs
	mcs -unsafe gksu/generated/*.cs -out:bin/Gksu.dll -target:library -pkg:gtk-sharp-2.0
	
gksuui: gksu.ui/generated/*.cs
	mcs gksu.ui/generated/*.cs -out:bin/Gksu.UI.dll -target:library -pkg:gtk-sharp-2.0

clean:
	rm bin/*

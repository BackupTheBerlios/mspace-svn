all:
	booc gksu-test.boo -r:../bin/Gksu.dll -r:../bin/Gksu.UI.dll -r:gtk-sharp

run:
	MONO_PATH=../bin:$(MONO_PATH) mono gksu-test.exe

clean:
	rm gksu-test.exe

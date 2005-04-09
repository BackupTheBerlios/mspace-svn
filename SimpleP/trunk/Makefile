include images/images.list

FILES =						\
		src/*.boo			\
		src/*/*/*.boo			\
		src/*/*.boo

REFERENCES=`pkg-config --libs gtk-sharp-2.0`		 		\
				`pkg-config --libs glade-sharp-2.0`		 		\
				-r:ext/Red.Gtk.dll						\
				-r:ext/Nini.dll						\
				-r:ext/Red.dll							\
				-r:ext/NLog.dll

all: update-refs Resources.dll
	booc $(FILES) -out:build/SimpleP.exe -target:winexe -r:./ext/Red.ProjectManager.dll $(REFERENCES)

Resources.dll: resources/res.cs
	mcs -t:library -out:build/Resources.exe -resource:ui-data/simplep.glade $(IMAGES) -resource:config.ini resources/res.cs

update-refs:
	cp ../../RedLib/trunk/build/Red.dll ext
	cp ../../RedLib/trunk/build/ICSharpCode.SharpZipLib.dll ext
	cp ../../Red.Gtk/trunk/build/Red.Gtk.dll ext
	cp ../../Red.ProjectManager/trunk/build/Red.ProjectManager.dll ext
	cp ext/*dll* build/	

clean:
	rm -rf build/*

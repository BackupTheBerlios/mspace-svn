FILES =						\
		src/*.boo			\
		src/*/*/*.boo			\
		src/*/*.boo

REFERENCES=`pkg-config --libs gtk-sharp-2.0`		 		\
				`pkg-config --libs glade-sharp-2.0`		 		\
				-r:ext/Red.Gtk.dll						\
				-r:ext/Red.dll							\
				-r:ext/NLog.dll
all: update-refs
	booc $(FILES) -out:build/SimpleP.exe -target:winexe -r:./ext/Red.ProjectManager.dll $(REFERENCES)

update-refs:
	cp ext/* build/	
	cp ui-data/*.glade build/
	cp -a images build

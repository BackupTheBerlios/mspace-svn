REFERENCES=														\
		-r:/usr/share/dotnet/mono/gtk-sharp-2.0/gtk-sharp.dll 	\
    	-r:/usr/share/dotnet/mono/gtk-sharp-2.0/glade-sharp.dll \
		-r:/usr/share/dotnet/mono/gtk-sharp-2.0/gdk-sharp.dll

all: build/Red.Gtk.dll
									
build/Red.Gtk.dll: src/*.boo
	booc src/*.boo -out:build/Red.Gtk.dll -target:library $(REFERENCES)

copy-ext:
	cp ext/* build
	
clean:
	rm build/*.dll

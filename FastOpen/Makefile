INSTALLDIR=~/bin
SOURCES=src/*.cs
RESOURCES=	\
	-resource:glade/fastopen.glade,fastopen.glade	\
	-resource:data/fastopenrc,fastopenrc
PKGS= -pkg:gtk-sharp,gnome-sharp,glade-sharp

all: fastopen.exe

fastopen.exe: 
	mcs $(SOURCES) $(PKGS) $(RESOURCES) -o bin/fastopen.exe
	
clean:
	rm bin/*.exe

release: fastopen.exe
	cp bin/fastopen.exe README TODO ~/public_html/fastopen

install:
	cp bin/fastopen.exe $(INSTALLDIR)

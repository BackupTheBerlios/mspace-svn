SOURCES = *.cs

all: idb-sharp.dll

idb-sharp.dll: $(SOURCES)
	mcs *.cs -o $@ /target:library

#EXTRA_DIST =				\
#	fastopen.in			\
#	README.en			\
#	TODO

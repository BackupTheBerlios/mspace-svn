PKG_REFERENCES=\
gtk-sharp-2.0 \
gtksourceview-sharp \
rsvg-sharp-2.0

PKG_REFERENCES_BUILD=$(addprefix -pkg:,$(PKG_REFERENCES))

LIB_REFERENCES = lib/log4net.dll 

LIB_REFERENCES_BUILD=$(addprefix -reference:,$(LIB_REFERENCES))

RESOURCES_REFERENCES= \
resources/log4net.config

RESOURCES_REFERENCES_BUILD=$(foreach res, $(RESOURCES_REFERENCES),$(addprefix -resource:,$(res)),$(notdir $(res)))

SOURCES_REFERENCES=\
org.vim.Components/BufferedVimControl/Model/*.cs\
org.vim.Components/BufferedVimControl/Controller/*.cs\
org.vim.Components/BufferedVimControl/View/*.cs\
org.vim.Components/BufferedVimControl/*.cs\
org.vim.Components/VimControl/Model/*.cs\
org.vim.Components/VimControl/Controller/*.cs\
org.vim.Components/VimControl/View/*.cs\
org.vim.Components/VimControl/*.cs\
org.vim.Components/VimBuffer/Model/*.cs\
org.vim.Components/VimBuffer/Controller/*.cs\
org.vim.Components/VimBuffer/View/*.cs\
org.vim.Components/VimBuffer/*.cs\
org.vim.Components/VimEntry/Model/*.cs\
org.vim.Components/VimEntry/Controller/*.cs\
org.vim.Components/VimEntry/View/*.cs\
org.vim.Components/VimEntry/*.cs\
org.vim.Components/*.cs\
Launcher.cs \
AssemblyInfo.cs

SOURCES_REFERENCES_BUILD=$(addprefix $(srcdir)/, $(SOURCES_REFERENCES))

all:
	$(MCS) $(PKG_REFERENCES_BUILD) $(LIB_REFERENCES_BUILD) $(RESOURCES_REFERENCES_BUILD) $(SOURCES_REFERENCES) -o MonoVim.exe

install:
	cp MonoVim.exe /usr/local/bin
	cp monovim /usr/local/bin
clean:
	rm MonoVim.exe
	rm AssemblyInfo.cs

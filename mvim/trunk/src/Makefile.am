PKG_REFERENCES=\
gtk-sharp-2.0 \
gtksourceview-sharp \
rsvg-sharp-2.0

PKG_REFERENCES_BUILD=$(addprefix -pkg:,$(PKG_REFERENCES))

LIB_REFERENCES = log4net.dll nunit.framework.dll

LIB_REFERENCES_BUILD=$(addprefix -reference:,$(LIB_REFERENCES))

RESOURCES_REFERENCES= 
#resources/log4net.config

RESOURCES_REFERENCES_BUILD=$(foreach res, $(RESOURCES_REFERENCES),$(addprefix -resource:,$(res)),$(notdir $(res)))

SOURCES_REFERENCES=\
Vim.Components.BufferedVimControl/*.cs\
Vim.Components.VimControl/*.cs\
Vim.Components.VimBuffer/*.cs\
Vim.Components.VimEntry/*.cs\
Vim.Components/*.cs\
Vim.Components.Gtk/*.cs\
Vim.Components.SWF/*.cs\
Vim.Main/Launcher.cs \
Vim.Test/*.cs\
AssemblyInfo.cs

SOURCES_REFERENCES_BUILD=$(addprefix $(srcdir)/, $(SOURCES_REFERENCES))

all:
	$(MCS) $(PKG_REFERENCES_BUILD) $(LIB_REFERENCES_BUILD) $(RESOURCES_REFERENCES_BUILD) $(SOURCES_REFERENCES) -o MonoVim.exe
	$(MCS) $(PKG_REFERENCES_BUILD) $(LIB_REFERENCES_BUILD) $(RESOURCES_REFERENCES_BUILD) $(SOURCES_REFERENCES) -t:library -o MonoVimTests.dll
install:
	cp MonoVim.exe /usr/local/bin
	cp monovim /usr/local/bin
clean:
	rm MonoVim.exe
	rm AssemblyInfo.cs

all:
	mono monodocer.exe --assembly:../src/MonoVim.exe --path:monodoc/
	monodoc --assemble --ecma monodoc/ --out org.vim
install:
	cp org.vim.* `monodoc --get-sourcesdir`
clean:
	rm -fr monodoc/
	rm org.vim.tree org.vim.zip

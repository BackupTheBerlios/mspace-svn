include sources.list
REFERENCES = -pkg:gtk-sharp
	
FLFramework.dll: $(CSSOURCES)
	mcs $(CSSOURCES) -o $@ -target:library $(REFERENCES)

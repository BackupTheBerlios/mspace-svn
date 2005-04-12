parse:
	gapi2-parser gksuui-sources.xml
	gapi2-parser gksu-sources.xml

fix:
	gapi2-fixup --metadata=gksuui-metadata.xml --api=gksuui-api.xml
	gapi2-fixup --metadata=gksu-metadata.xml --api=gksu-api.xml

gen:
	gapi2-codegen --generate gksuui-api.xml
	gapi2-codegen --generate gksu-api.xml

all: parse fix gen

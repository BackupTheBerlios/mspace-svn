compile:
	mcs SdkServicesTest.cs /r:../../Muine.Player.Sdk.dll -pkg:gtk-sharp

run:
	MONO_PATH="../../:../../../PlayerKits/Gstreamer:$(MONO_PATH)" mono SdkServicesTest.exe

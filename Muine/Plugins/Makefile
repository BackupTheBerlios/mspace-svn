SDK=../Muine.Sdk.dll

all: dashboard testplugin

dashboard:
	mcs /target:library -o DashboardFrontend.dll /r:$(SDK) Dashboard/*.cs

testplugin:
	mcs /target:library TestPlugin/*.cs -o TestPlugin.dll /r:$(SDK)


install-all:
	cp *.dll ~/.gnome2/muine/plugins

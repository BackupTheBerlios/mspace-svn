ASSEMBLIES = /r:../../Sdk/Muine.Sdk.dll
all: DashboardFrontend.dll

DashboardFrontend.dll: *.cs
	mcs /target:library -o $@ $(ASSEMBLIES) *.cs

dashboardfrontendlibdir = ~/.gnome2/muine/plugins
dashboardfrontendlib_DATA = DashboardFrontend.dll

CLEANFILES =				\
	DashboardFrontend.dll

all: DashboardFrontend.dll

DashboardFrontend.dll: *.cs
	mcs /target:library -o $@ $(ASSEMBLIES) *.cs -pkg:muine-sdk

dashboardfrontendlibdir = ~/.gnome2/muine/plugins
dashboardfrontendlib_DATA = DashboardFrontend.dll

CLEANFILES =				\
	DashboardFrontend.dll

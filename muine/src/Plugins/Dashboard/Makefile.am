ASSEMBLIES = /r:../../Sdk/Muine.Sdk.dll
all: DashboardFrontend.dll

DashboardFrontend.dll: *.cs
	mcs /target:library -o $@ $(ASSEMBLIES) *.cs

dashboardfrontendlibdir = @prefix@/lib/muine/plugins
dashboardfrontendlib_DATA = DashboardFrontend.dll

CLEANFILES =				\
	DashboardFrontend.dll

RESOURCES=						\
	/resource:images/Coolgrey-squared.png,Coolgrey-squared.png		\
	/resource:images/Table-blue.png,Table-blue.png		\
	/resource:images/click.png,click.png		\
	/resource:images/poing.png,poing.png		\
	/resource:images/oh.png,oh.png		\
	/resource:glade/NewTask.glade,NewTask.glade	\
	/resource:glade/ReadonlyTaskInfoDialog.glade,ReadonlyTaskInfoDialog.glade	\
	/resource:glade/PlannerWindow.glade,PlannerWindow.glade 					\
	/resource:src/Plugin/plugins.xml,plugins.xml
all:
	mcs --recurse *.cs -pkg:gnome-sharp -pkg:glade-sharp $(RESOURCES) -o weekplanner.exe

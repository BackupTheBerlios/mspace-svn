REFERENCES = -pkg:gtk-sharp /r:../src/Chicken.dll
all: svg-bubble.exe factory-example.exe html-bubble.exe notification-area-message.exe

svg-bubble.exe: SvgBubble.cs
	mcs SvgBubble.cs -o $@ $(REFERENCES)

factory-example.exe: FactoryExample.cs
	mcs FactoryExample.cs -o $@ $(REFERENCES)

html-bubble.exe: HtmlBubble.cs
	mcs HtmlBubble.cs -o $@ $(REFERENCES)

notification-area-message.exe: NotificationAreaMessageExample.cs
	mcs NotificationAreaMessageExample.cs -o $@ $(REFERENCES)

CLEANFILES = *.exe
EXTRA_DIST = *.cs

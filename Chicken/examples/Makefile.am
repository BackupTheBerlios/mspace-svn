all: svg-bubble.exe factory-example.exe html-bubble.exe notification-area-message.exe

svg-bubble.exe: SvgBubble.cs
	mcs SvgBubble.cs -o $@ -pkg:gtk-sharp -pkg:chicken

factory-example.exe: FactoryExample.cs
	mcs FactoryExample.cs -o $@ -pkg:gtk-sharp -pkg:chicken

html-bubble.exe: HtmlBubble.cs
	mcs HtmlBubble.cs -o $@ -pkg:gtk-sharp -pkg:chicken

notification-area-message.exe: NotificationAreaMessageExample.cs
	mcs NotificationAreaMessageExample.cs -o $@ -pkg:gtk-sharp -pkg:chicken

CLEANFILES = *.exe
EXTRA_DIST = *.cs

all: bubble-example.exe message-example.exe html-bubble.exe notification-area-message.exe

bubble-example.exe: BubbleExample.cs
	mcs BubbleExample.cs -o $@ -pkg:gtk-sharp -pkg:chicken

message-example.exe: MessageExample.cs
	mcs MessageExample.cs -o $@ -pkg:gtk-sharp -pkg:chicken

html-bubble.exe: HtmlBubble.cs
	mcs HtmlBubble.cs -o $@ -pkg:gtk-sharp -pkg:chicken

notification-area-message.exe: NotificationAreaMessageExample.cs
	mcs NotificationAreaMessageExample.cs -o $@ -pkg:gtk-sharp -pkg:chicken

CLEANFILES = *.exe
EXTRA_DIST = *.cs


    using DBus;
    using System;
    using Chicken.Gnome.Notification;
    using Gtk;
    
    [DBus.Interface ("org.rubiojr.MonkeyPop.NotificationDaemon")]
    public class NotificationDaemon
    {
	public const string Path = "/org/rubiojr/MonkeyPop/NotificationDaemon";
	public const string Service = "org.rubiojr.MonkeyPop";
	    
	// Shows a standard notification message.
	// type is one of Error, Info, Warning. Case sensitive.
	// header: the header of the message
	// body: the body of the message
	[DBus.Method]
	public void ShowMessageNotification (string header, string body, string type)
	{
	    NotificationType ntype = (NotificationType)Enum.Parse (typeof (NotificationType), type);
	    NotificationFactory.ShowMessageNotification (header, body, ntype, null);
	}
	

    }

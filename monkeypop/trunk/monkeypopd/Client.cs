namespace Foo
{
	using System;
	using DBus;
	using System.Runtime.InteropServices;

	public class Client
	{
		[DllImport ("dbus-glib-1")]
		private extern static void dbus_g_thread_init ();
		public static void Main(string [] args)
		{
			dbus_g_thread_init ();
			Connection connection = Bus.GetSessionBus();
			Service service = Service.Get(connection, "org.rubiojr.MonkeyPop");
			NotificationDaemon daemon = (NotificationDaemon) 
				service.GetObject(typeof(NotificationDaemon), "/org/rubiojr/MonkeyPop/NotificationDaemon");
		}
	}
}

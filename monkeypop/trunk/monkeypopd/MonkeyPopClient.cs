namespace Foo
{
  using System;
  using DBus;
  using MonkeyPop.NotificationDaemon;

  public class MonkeyPopClient
  {
    public static int Main(string [] args)
    {
      Connection connection = Bus.GetSessionBus();
      Service service = Service.Get(connection, "org.rubiojr.MonkeyPop");
      NotificationDaemon daemon = (NotificationDaemon) 
	    service.GetObject(typeof(NotificationDaemon), "/org/rubiojr/MonkeyPop/NotificationDaemon");
      daemon.ShowMessageNotification ("test", "TEST", "Info");
      
      return 0;
    }
  }
}

namespace Foo
{
  using System;
  using DBus;

  public class Client
  {
    public static void Main(string [] args)
    {
      Connection connection = Bus.GetSessionBus();
      Service service = Service.Get(connection, "org.rubiojr.MonkeyPop");
      NotificationDaemon daemon = (NotificationDaemon) 
      		service.GetObject(typeof(NotificationDaemon), "/org/rubiojr/MonkeyPop/NotificationDaemon");
    }
  }
}

"""Lists services on the system bus
"""
import dbus

bus = dbus.SystemBus()

dbus_service = bus.get_service('org.rubiojr.MonkeyPop')
    
dbus_object = dbus_service.get_object('/org/rubiojr/MonkeyPop/NotificationDaemon',
				     'org.rubiojr.MonkeyPop.NotificationDaemon')
            
# One of the member functions in the org.freedesktop.DBus interface
# is ListServices(), which provides a list of all the other services
# registered on this bus. Call it, and print the list.
dbus_object.ShowMessageNotification ("hola", "hola", "Info")


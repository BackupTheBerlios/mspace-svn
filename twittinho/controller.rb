$KCODE = 'u'

require 'gconf2'
require 'gui'
require 'model'
require 'nucleo'

gconfPath = "/apps/twitinho/"
client = GConf::Client.default

if client.dir_exists?(gconfPath.chomp("\/"))
	config = readConfig(client)
else
	WindowPreferences.new(client, false)
end

initState = true

nucleo = Nucleo.new(config)
mainWindow = MainWindow.new(nucleo, config)
statusIcon = StatusIcon.new(mainWindow, client)


thread = Thread.new do
        loop {
		nucleo.refreshStatuses(statusIcon, initState, config)
		initState = false
		sleep(config["refresh"])
	}
end

Gtk.main


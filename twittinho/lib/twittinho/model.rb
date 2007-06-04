$KCODE = 'u'

require 'twitter'

GCONF_PATH = '/apps/twitinho/'

def connect(email, password)
	return Twitter::Base.new(email, password)
end

def readConfig (client)
	gconfPath = GCONF_PATH
        configs = Hash.new
        configs["user"] = client["#{gconfPath}user"]
        configs["password"] = client["#{gconfPath}password"]
        configs["refresh"] =  client["#{gconfPath}refresh"]
        configs["notifications"] = client["#{gconfPath}notifications"]

        return configs
end


def writeConfig (client, config)
	gconfPath = GCONF_PATH
        client["#{gconfPath}user"] = config["user"]
        client["#{gconfPath}password"] = config["password"]
        client["#{gconfPath}refresh"] = config["refresh"].to_i
        client["#{gconfPath}notifications"] = config["notifications"]
end

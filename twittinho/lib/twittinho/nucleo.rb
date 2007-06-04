$KCODE = 'u'

require 'gtk2'
require 'twitter'
require 'curb'
require 'fileutils'

class Nucleo

	def initialize(config)
		@connection = connect(config["user"], config["password"])
		@lastStatusID = 0
		@listStore = Gtk::ListStore.new(Gdk::Pixbuf, String)
	end

	def setLastStatusId(statusID)
		@lastStatusID = statusID
	end

	def connection
		@connection
	end
		
	def listStore
		@listStore
	end

	def checkNewStates
        	return @connection.timeline(:friends)
	end

	def refreshStatuses(statusIcon, initState, config)
        	statusesToUpdate = updateStatus
        	if !statusesToUpdate.empty?
        	        statusesToUpdate.each do |state|
        	                getFaces(state.user)
        	                insertState(state)
        	                sendNotify(state.user, state.text, statusIcon) if config["notifications"] && ! initState
        	                setLastStatusId(state.id)
        	        end
        	end
	end

	def updateStatus
		newStates = checkNewStates
		newStates = newStates.reverse!
                statesArray = Array.new
		newStates.each do |state|
                	if state.id.to_i > @lastStatusID.to_i
				statesArray << state
			end
		end
		return statesArray
	end

	def insertState(state)
        	## Rows are been inserted at the first position on the model. Surely it could be improved.
        	iter = @listStore.insert(0)
        	iter[0] = Gdk::Pixbuf.new(face(state.user.screen_name))
        	iter[1] = state.text
		@lastStatusID = state.id
		#if listStore.get_iter(Gtk::TreePath.new(19))
                #	listStore.remove(listStore.get_iter(Gtk::TreePath.new(19)))
                #end

	end

	def face(twitterUser)
		return ENV['HOME'] + '/.twitinho/images/' +  twitterUser + '.png'
	end
	
	def getFaces(user)
        	if File.exist?(face(user.screen_name))
        	        return
        	else
			from = ENV['HOME'] + '/.twitinho/default_face.png'
			FileUtils.copy(from, face(user.screen_name))
			Thread.new do
	        	        sleep(1)
				url = user.profile_image_url
        		        Curl::Easy.download(url, face(user.screen_name))
			end
        	end
	end

	def sendNotify(user, state, statusIcon) 
                LibNotify.init( "Twittinho" )
		test = LibNotify::Notification.new(user.name, state, face(user.screen_name), statusIcon)
                test.timeout= 10000      # 5 seconds
                test.show
	end
	
end

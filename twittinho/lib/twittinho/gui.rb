$KCODE = 'u'

require 'gtk2'
require 'libnotify'

class MainWindow < Gtk::Window

	def initialize (nucleo, config)
		
		@pixbufRend = Gtk::CellRendererPixbuf.new()

		@textRend = Gtk::CellRendererText.new()
		@textRend.set_property('wrap_mode', Pango::Layout::WRAP_WORD_CHAR)
		@textRend.set_single_paragraph_mode(false)
		@textRend.set_width(50)

		@imageCol = Gtk::TreeViewColumn.new("Cara", @pixbufRend, :pixbuf => 0)
		@imageCol.set_resizable(false)
		@imageCol.set_spacing(5)
		
		@textCol = Gtk::TreeViewColumn.new("Nombre", @textRend, :markup => 1)
		@textCol.set_resizable(true)
		@textCol.set_fixed_width(500)
		@textCol.set_sizing(Gtk::TreeViewColumn::FIXED)
		@textCol.set_spacing(20)
		
		@listView = Gtk::TreeView.new()
		@listView.set_model(nucleo.listStore)
		@listView.append_column(@imageCol)
		@listView.append_column(@textCol)
		@listView.headers_visible = false
		@listView.rules_hint = true

		@scrolledWindow = Gtk::ScrolledWindow.new()
		@scrolledWindow.set_policy(Gtk::POLICY_NEVER, Gtk::POLICY_AUTOMATIC)
		@scrolledWindow.add(@listView)

		@vBox = Gtk::VBox.new()
		@vBox.set_spacing(6)
		@vBox.add(@scrolledWindow)

		@btnHome = Gtk::Button.new(Gtk::Stock::HOME)
		@btnHome.set_relief(Gtk::RELIEF_HALF)
		@btnHome.signal_connect( "clicked" ) { |w| 
			system("gnome-open http://twitter.com/#{config['user']}")
		}
		
		@entry = Gtk::Entry.new
		@entry.max_length = 140
		@entry.signal_connect( "changed" ) { |w|OnPostEntryChanged(w, @labelCarac) }
		@entry.signal_connect( "activate" ) { |w| OnPostEntryActivated(w, nucleo.connection) }
		
		@labelCarac = Gtk::Label.new("140")
		
		@hBox = Gtk::HBox.new
		
		@hBox.pack_start(@btnHome, expand=false)
		@hBox.add(@entry)
		@hBox.pack_start(@labelCarac, expand=false)
		
		@vBox.pack_start(@hBox, expand=false)
		
		@statusBar = Gtk::Statusbar.new
		
		@vBox.pack_start(@statusBar, expand=false)

		super()
		
		set_title("Twitinho Alpha 0.2")		
		set_default_size(300, 500)
                set_border_width(6)
		resizable = false
		signal_connect("delete_event") { Gtk.main_quit; exit! }
		add(@vBox)

		show_all

	end		

	def getListView
		return @listView
	end

	def OnPostEntryChanged(texto,label)
	        label.text = (140 - texto.text.length).to_s
	end
	
	def OnPostEntryActivated(entry, connection)
		entry.editable = false
		connection.update(entry.text)
		entry.text = ""
		entry.editable = true
	end
	

end

class StatusMenu < Gtk::Menu

	def initialize (client)
		#refreshItem = Gtk::ImageMenuItem.new(Gtk::Stock::REFRESH)
		#refreshItem.signal_connect("activate") { Thread.new { nucleo.refreshStatuses } }
		#refreshItem.show
		
		preferencesItem = Gtk::ImageMenuItem.new(Gtk::Stock::PREFERENCES)
		preferencesItem.signal_connect("activate") { WindowPreferences.new(client, true) }
		preferencesItem.show
		
		quitItem = Gtk::ImageMenuItem.new(Gtk::Stock::QUIT)
		quitItem.signal_connect("activate") { Gtk.main_quit }
		quitItem.show
		
		
		super()

		#add(refreshItem)
		add(preferencesItem)
		add(quitItem)
	end
end

class StatusIcon < Gtk::StatusIcon

	def initialize(window, client)
		@menu = StatusMenu.new(client)
		super()
		
		set_icon_name(Gtk::Stock::HOME)
		set_tooltip("Twitinho")
		signal_connect("popup_menu") { |w, button, time| OnStatusIconPopUp(w, button, time, @menu) }
		signal_connect("activate") { |w, button, time| OnStatusIconActivated(w, window) }
	end	
	
	def OnStatusIconPopUp (w, button, time, menu)
	        menu.popup(nil, nil, button, time)
	
	end
	
	def OnStatusIconActivated (w, window)
		if window.visible?
			window.hide
		else
			window.show
		end
	end
end



class WindowPreferences < Gtk::Window

	def initialize (client, configState)
		if configState
			config = readConfig(client)		
		else
			config = Hash.new
			config["user"] = ""
			config["password"] = ""
			config["refresh"] = 120
			config["notifications"] = false
		end

		labelAuthentication = Gtk::Label.new()
		labelAuthentication.set_markup('<b>Authentication</b>')
		
		labelUser = Gtk::Label.new('Username')
		
		entryUser = Gtk::Entry.new()
		entryUser.set_text(config["user"])
		
		labelPassword = Gtk::Label.new('Password')
		
		entryPassword = Gtk::Entry.new()
		entryPassword.set_visibility(false)
		entryPassword.set_text(config["password"])
		
		
		labelOthers = Gtk::Label.new()
		labelOthers.set_markup('<b>Others</b>')
		labelOthers.set_justify(Gtk::JUSTIFY_LEFT)
		
		checkButtonNotifications = Gtk::CheckButton.new('Active Notifications?')
		checkButtonNotifications.set_active(config["notifications"])	
		
		refreshLabel = Gtk::Label.new('States Refresh Time (in seconds)')

		refreshEntry = Gtk::Entry.new()
		refreshEntry.set_text(config["refresh"].to_s)
		acceptButton = Gtk::Button.new(Gtk::Stock::OK)
		acceptButton.signal_connect("clicked") {
			OnAcceptButtonClicked(client, entryUser, entryPassword, checkButtonNotifications, refreshEntry) 
			destroy	
		}
		cancelButton = Gtk::Button.new(Gtk::Stock::CANCEL)
		cancelButton.signal_connect("clicked") { destroy }
		
		hButtonBox = Gtk::HButtonBox.new()
		hButtonBox.pack_start(cancelButton, padding = 5)	
		hButtonBox.pack_start(acceptButton, padding = 5)	
		hButtonBox.set_layout_style(Gtk::ButtonBox::Style::END)
		hButtonBox.set_spacing(20)
		
		
		table = Gtk::Table.new(7,4,homogeneus = false)
		
		table.attach(labelAuthentication, 1, 2, 1, 2, Gtk::FILL, Gtk::FILL, 8, 8)
		table.attach(labelUser, 1, 2, 2, 3, Gtk::FILL, Gtk::FILL, 8, 8)
		table.attach(entryUser, 2, 3, 2, 3, Gtk::FILL, Gtk::FILL, 8, 8)
		table.attach(labelPassword, 1, 2, 3, 4, Gtk::FILL, Gtk::FILL, 8, 8)
		table.attach(entryPassword, 2, 3, 3, 4, Gtk::FILL, Gtk::FILL, 8, 8)
		table.attach(labelOthers, 1, 2, 4, 5, Gtk::FILL, Gtk::FILL, 8, 8)
		table.attach(checkButtonNotifications, 1, 3, 5, 6, Gtk::FILL, Gtk::FILL, 8, 8)
		table.attach(refreshLabel, 1, 2, 6, 7, Gtk::FILL, Gtk::FILL, 8, 8)
		table.attach(refreshEntry, 2, 3, 6, 7, Gtk::FILL, Gtk::FILL, 8, 8)
		
		vbox = Gtk::VBox.new()
		vbox.add(table)
		vbox.add(hButtonBox)
		vbox.set_spacing(6)		
	
		super()		
		set_title("Preferences")	
		set_default_size(320, 250)
		set_border_width(6)
		set_resizable = false
		set_modal(true)
		signal_connect("delete_event") { destroy }
		
		add(vbox)
		show_all
	end
	
	def OnAcceptButtonClicked (client, user, password, notifications, refresh)
		config = Hash.new
		config["user"] = user.text	
		config["password"] = password.text
		config["notifications"] = notifications.active?
		config["refresh"] = refresh.text
		writeConfig(client, config)
	end

	
	

end

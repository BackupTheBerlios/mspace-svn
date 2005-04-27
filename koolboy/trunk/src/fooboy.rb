#!/usr/bin/ruby

require 'Korundum'

class SysTrayThing < KDE::SystemTray
	def initialize(parent, name )
		super(parent, name)
		setPixmap( KDE::SystemTray::loadIcon("kgpg"));
	end

	def hideWindow
		parentWidget.hide
	end

	def relocateWindow
		size = parentWidget.sizeHint
		winPos = Qt::Point.new(
			width - size.width/2,
			height - size.height/2)

		winPos = mapToGlobal(winPos)
		parentWidget.move(winPos)
	end

	def showWindow
		relocateWindow
		parentWidget.show
		KDE::Win::activateWindow( parentWidget.winId );
	end

	def mousePressEvent( ev )
		case ev.button
		when Qt::LeftButton
			if parentWidget.isVisible
				hideWindow
			else
				showWindow
			end

		when Qt::MidButton
			
		when Qt::RightButton
			if parentWidget
				action = actionCollection.action("minimizeRestore")
				if parentWidget.isVisible
					action.setText(i18n("&Minimize"))
				else
					action.setText(i18n("&Restore"))
				end 
			end
			contextMenuAboutToShow( contextMenu )
			contextMenu.popup(ev.globalPos)
		end
	end
end

class MainWindow < KDE::MainWindow
	slots   'aboutToQuit()'

	def initialize( name )
		super(nil, name)
		@shuttingDown = false

		@systemTray = SysTrayThing.new( self, "our fooboy" )
		connect(@systemTray, SIGNAL('quitSelected()'),
			self, SLOT('aboutToQuit()'))

		@text = KDE::TextEdit.new(self)
		loadText
		self.setCentralWidget(@text)

		@systemTray.show
	end

	def loadText
		file = KDE::StandardDirs::locateLocal("appdata","note")
		test ?r, file and
			File.open(file) { |f| @text.setText(f.gets(nil)) }
	end

	def aboutToQuit
		@shuttingDown = true
	end

	def queryClose
		if not @shuttingDown
			hide
			return false
		else
			file = KDE::StandardDirs::locateLocal("appdata","note")
			File.open(file,"w") { |f| f.print(@text.text) }
			return true	
		end
	end
end

about = KDE::AboutData.new("fooboy", "fooboy - nice stuff", "0.1")
KDE::CmdLineArgs.init(ARGV, about)
a = KDE::UniqueApplication.new()

window = MainWindow.new( "our fooboy" )

a.mainWidget = window
a.exec 

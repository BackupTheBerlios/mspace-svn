#!/usr/bin/ruby

require 'Korundum'

class SysTrayThing < KDE::SystemTray
	slots   'sAboutToQuit()',
		'sNewNote()'

	def initialize(name )
		super(nil, name)
		setPixmap( KDE::SystemTray::loadIcon("kgpg"))

		@windows = []
		@shuttingDown = false

		connect(self, SIGNAL('quitSelected()'),
			self, SLOT('sAboutToQuit()'))

		@leftMenu = KDE::PopupMenu.new(self)
		@leftMenu.insertItem( i18n( "&New note" ), self, SLOT('sNewNote()') )
	end

	def sAboutToQuit
		@shuttingDown = true
	end

	def sNewNote
		p "New note!"
		window = NoteWindow.new(nil)
		window.show
		@windows << window
	end

	def mousePressEvent( ev )
		case ev.button
		when Qt::LeftButton
			@leftMenu.popup(ev.globalPos)

		when Qt::MidButton
			
		when Qt::RightButton
			contextMenuAboutToShow( contextMenu )
			contextMenu.popup(ev.globalPos)
		end
	end
end

class NoteWindow < KDE::MainWindow

	def initialize( name )
		name or name = "New note"
		super(nil, name)
		setCaption(name)

		@text = KDE::TextEdit.new(self)
		loadText
		self.setCentralWidget(@text)
	end

	def loadText
		file = KDE::StandardDirs::locateLocal("appdata","note")
		test ?r, file and
			File.open(file) { |f| @text.setText(f.gets(nil)) }
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

about = KDE::AboutData.new("koolboy", "Koolboy", "0.1")
KDE::CmdLineArgs.init(ARGV, about)
#a = KDE::UniqueApplication.new()
a = KDE::Application.new()

thing = SysTrayThing.new( "our fooboy" )
thing.show

a.mainWidget = thing
a.exec 

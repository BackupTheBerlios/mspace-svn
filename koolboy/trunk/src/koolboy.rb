#!/usr/bin/ruby

require 'Korundum'
require 'libkoolnotes.rb'

class SysTrayThing < KDE::SystemTray
	slots   'sAboutToQuit()',
		'sNewNote()'

	def initialize(name )
		super(nil, name)
		setPixmap( KDE::SystemTray::loadIcon("kgpg"))

		# stuff related with closing app
		@shuttingDown = false

		connect(self, SIGNAL('quitSelected()'),
			self, SLOT('sAboutToQuit()'))

		# array to keep a list of windows
		@windows = []

		# create left menu
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

		# we need to increase refCount to be able
		# to use kmainwindows in this manner
		$kapp.ref

		# load note from database
		@note = KoolNoteWell.instance.getNote(name)

		@text = KDE::TextEdit.new(self)
		@text.setText(@note.contents)
		self.setCentralWidget(@text)
	end

	def queryClose
		@note.contents = @text.text
		KoolNoteWell.instance.storeNote(@note)
		return true
	end
end

about = KDE::AboutData.new("koolboy", "Koolboy", "0.1")
KDE::CmdLineArgs.init(ARGV, about)
#a = KDE::UniqueApplication.new()
a = KDE::Application.new()

thing = SysTrayThing.new( "our fooboy" )
thing.show

KoolNoteWell.setFile(KDE::StandardDirs::locateLocal("appdata","koolnotes.db"))

a.mainWidget = thing
a.exec 

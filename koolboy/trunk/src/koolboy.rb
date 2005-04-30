#!/usr/bin/ruby

require 'Korundum'
require 'libkoolnotes.rb'

class SysTrayThing < KDE::SystemTray
	slots   'sAboutToQuit()',
		'sNewNote()',
		'sShowNote(int)'

	def initialize(name )
		super(nil, name)
		setPixmap( KDE::SystemTray::loadIcon("kgpg"))
	end

	def regenMenu
		# create left menu
		@leftMenu = KDE::PopupMenu.new(self)
		@leftMenu.insertItem( i18n( "&New note" ), self, SLOT('sNewNote()') )
		@leftMenu.insertSeparator

		@menuTitles = {}

		KoolNoteWell.instance.lastNotes.each { |note|
			id = @leftMenu.insertItem( note['title'], self, SLOT('sShowNote(int)') )
			# we need to map from menu id to note title
			@menuTitles[id] = note['title']
		}
	end

	def sNewNote
		window = NoteWindow.new(nil)
		size = window.sizeHint
		winPos = Qt::Point.new( width - size.width/2, 
			height - size.height/2)
		winPos = mapToGlobal(winPos)
		window.move(winPos)
		window.show
	end

	def sShowNote (id)
		window = NoteWindow.new(@menuTitles[id])
		size = window.sizeHint
		winPos = Qt::Point.new( width - size.width/2, 
			height - size.height/2)
		winPos = mapToGlobal(winPos)
		window.move(winPos)
		window.show
	end

	def mousePressEvent( ev )
		case ev.button
		when Qt::LeftButton
			regenMenu
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
		text = @text.text.strip
		if text.length != 0
		then
			@note.title = text.split("\n").first
			@note.contents = @text.text
		else
			@note.title = ''
		end 
		KoolNoteWell.instance.storeNote(@note)
		return true
	end
end

about = KDE::AboutData.new("koolboy", "Koolboy", "0.1")
KDE::CmdLineArgs.init(ARGV, about)
a = KDE::UniqueApplication.new()

thing = SysTrayThing.new( "koolboy" )
thing.show

a.mainWidget = thing
a.exec 

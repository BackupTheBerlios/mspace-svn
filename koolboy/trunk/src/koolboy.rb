#!/usr/bin/ruby

require 'Korundum'
require 'libkoolnotes.rb'

class Koolboy < KDE::SystemTray
	slots   'sNewNote()', 'sShowNote(int)',
			'sRemoveWindow(char*)', 'showAbout()'
	
	k_dcop 'QStringList lastNotes()'

	def initialize(name )
		super(nil, name)
		setPixmap( KDE::SystemTray::loadIcon("kgpg"))
		@windows = {}
		@icons = KDE::IconLoader.new()
		#add about to rightclick menu
		contextMenu().insertItem( Qt::IconSet.new(@icons.loadIcon("about", KDE::Icon::Toolbar)),
		                          "About Koolboy", self, SLOT('showAbout()') )
	end

	def regenMenu
		
		
		# create left menu
		@leftMenu = KDE::PopupMenu.new(self)
		@leftMenu.insertItem( i18n( "&New note" ), self, SLOT('sNewNote()') )
		@leftMenu.insertSeparator
		
		@menuTitles = {}

		NoteManager.instance.lastNotes.each { |note|
			id = @leftMenu.insertItem( note, self, SLOT('sShowNote(int)') )
			# we need to map from menu id to note title
			@menuTitles[id] = note
		}
	end

	def sNewNote
		window = NoteWindow.new(nil)
		connect(window, SIGNAL('aboutToClose(char*)'),
			self, SLOT('sRemoveWindow(char*)'))
		window.show
		@windows[window.title] = window
	end

	def sShowNote (id)
		title = @menuTitles[id]
		if @windows.key?(title)
			@windows[title].setActiveWindow
			@windows[title].raise
		else
			window = NoteWindow.new(title)
			connect(window, SIGNAL('aboutToClose(char*)'),
				self, SLOT('sRemoveWindow(char*)'))
			window.show
			@windows[window.title] = window
		end
	end

	def sRemoveWindow (title)
		@windows.delete(title)
	end

	def showAbout
		KDE::AboutApplication.new.show
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

	#DCOP methods
	def lastNotes
		NoteManager.instance.lastNotes
	end
end

class NoteWindow < KDE::MainWindow
	signals 'aboutToClose(char*)'

	def initialize( name )
		if not name
			name = NoteManager.instance.unusedName
			new = true
		else
			new = false
		end
		super(nil, name)
		setCaption(name)

		# we need to increase refCount to be able
		# to use kmainwindows in this manner
		$kapp.ref

		# get note
		@note = NoteManager.instance.getNote(name)

		if not new
			resize(@note.size)
			move(@note.pos)
		end

		@text = KDE::TextEdit.new(self)
		@text.setText(@note.text)
		self.setCentralWidget(@text)
	end

	def title
		@note.title
	end

	def queryClose
		text = @text.text.strip
		emit aboutToClose(@note.title)
		if text.length != 0
		then
			title = text.split("\n").first
			NoteManager.instance.change(@note,title,text,size,pos)
		else
			NoteManager.instance.delete(@note)
		end 
		return true
	end
end

if $0 == __FILE__
	description = "Note taking app"
	version     = "0.1"
	about = KDE::AboutData.new("koolboy", "Koolboy", version,
						   description, KDE::AboutData::License_GPL,
						   "")
	about.addAuthor("Isaac Clerencia", "Core developer", "isaac@warp.es")
	about.addAuthor("Sergio Rubio", "Core developer", "sergio.rubio@hispalinux.es")
	about.setBugAddress("mspace-koolboy@lists.berlios.de")

	KDE::CmdLineArgs.init(ARGV,about)
	a = KDE::UniqueApplication.new()
	koolboy = Koolboy.new( "koolboy" )
	koolboy.show
	a.mainWidget = koolboy
	a.exec
end

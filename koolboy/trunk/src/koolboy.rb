#!/usr/bin/ruby

require 'Korundum'
require 'libkoolnotes.rb'
require 'noteview.rb'

class Koolboy < KDE::SystemTray
	slots 'sNewNote()', 'sShowNote(int)', 'sRecentChanges()',
		'sRemoveWindow(char*)', 'showAbout()', 'sSearchNotes()'
	
	k_dcop 'QStringList lastNotes()'

	def initialize(name )
		super(nil, name)
		setPixmap( KDE::SystemTray::loadIcon("kgpg"))
		@windows = {}
		@icons = KDE::IconLoader.new()
		#add about to rightclick menu
		contextMenu().insertItem( Qt::IconSet.new(@icons.loadIcon("about", KDE::Icon::Toolbar)),
		                          i18n("About Koolboy"), self, SLOT('showAbout()') )
	end

	def regenMenu
		# create left menu
		@leftMenu = KDE::PopupMenu.new(self)
		@leftMenu.insertItem( Qt::IconSet.new(@icons.loadIcon("file_new", KDE::Icon::Toolbar)),
		                      i18n( "&Create new note" ), self, SLOT('sNewNote()') )
		@menuTitles = {}
		NoteManager.instance.lastNotes.each { |note|
			id = @leftMenu.insertItem( Qt::IconSet.new(@icons.loadIcon("knotes", KDE::Icon::Toolbar)),
			                          note, self, SLOT('sShowNote(int)') )
			# we need to map from menu id to note title
			@menuTitles[id] = note
		}

		@leftMenu.insertSeparator
		
		@leftMenu.insertItem( Qt::IconSet.new(@icons.loadIcon("window_list", KDE::Icon::Toolbar)),
		                     i18n( "&Recent changes" ), self, SLOT('sRecentChanges()') )
		@leftMenu.insertItem( Qt::IconSet.new(@icons.loadIcon("find", KDE::Icon::Toolbar)),
		                      i18n( "&Search notes..." ), self, SLOT('sSearchNotes()') )
	end

	def sNewNote
		note = NoteManager.newNote
		window = NoteView.new(note)
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
			# get note
			note = NoteManager.instance.getNote(title)

			window = NoteView.new(note)
			connect(window, SIGNAL('aboutToClose(char*)'),
				self, SLOT('sRemoveWindow(char*)'))
			window.show
			@windows[note.title] = window
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

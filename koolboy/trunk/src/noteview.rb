require "Korundum"
include KDE
require "libkoolnotes"

#TODO
#How can I use custom icons in actions?
class NoteView < MainWindow
	slots 'slotLink()',
	'noteDeleted()', 'slotFind()', 'formatBold()', 'formatUnderline()',
	'formatStrikeout()', 'formatItalic()'

	# Receives an XmlNote and handles its content
	def initialize(note)
		msecs = Time.now.usec.to_s
		puts "new note view at " + msecs
		super(nil, "note-" + Time.now.usec.to_s)	
		@note = note
		@icons = KDE::IconLoader.new()
		initWidgets()
		defineActions()
		initToolBar()
		
		self.caption = @note.title
		@textEdit.text = @note.text if @note.text
		resize(@note.size) if @note.size
		move(@note.pos) if @note.size
	end

	def queryClose
		puts "CLOSING"
		puts "Title: " + self.name
		return true
	end
	
	#####################
	#TEXT formatting	
	def formatItalic
		puts "FORMAT"
	end

	def formatUnderline
	end

	def formatStrikeout
	end

	def formatBold
	end
	###################

	def slotFind
	end

	def slotLink
	end

	def noteDeleted
	end
	
	def initWidgets
		@vbox = Qt::VBox.new(self)
		@vbox.show
		@textEdit = TextEdit.new(@vbox)
		@textEdit.show
		self.centralWidget = @vbox
	end
	
	def defineActions
		#@aboutAction = StdAction.aboutApp(self, SLOT('slotAbout()'), actionCollection())
		
		@linkAction = Action.new(i18n("Link"), Shortcut.new(), self, SLOT('slotLink()'), actionCollection(), "link")
		@linkAction.setIcon("connect_established")
		
		@deleteAction = Action.new(i18n("Delete"), Shortcut.new(), self, SLOT('slotDelete()'), actionCollection(), "delete")
		@deleteAction.setIcon("trashcan_empty")
		
		@findAction = StdAction.find(self, SLOT('slotFind()'), actionCollection())

		@formatAction = ActionMenu.new(i18n("Format"), "font")
		
		icon = Qt::IconSet.new(@icons.loadIcon("text_bold", KDE::Icon::Toolbar))
		@formatAction.insert(Action.new( i18n("Bold"), icon, Shortcut.new(), self, SLOT('formatBold()'), actionCollection(), "formatBold"))

		icon = Qt::IconSet.new(@icons.loadIcon("text_italic", KDE::Icon::Toolbar))
		@formatAction.insert(Action.new( i18n("Italic"), icon, Shortcut.new(), self, SLOT('formatItalic()'), actionCollection(), "formatItalic"))

		icon = Qt::IconSet.new(@icons.loadIcon("text_under", KDE::Icon::Toolbar))
		@formatAction.insert(Action.new( i18n("Underline"), icon, Shortcut.new(), self, SLOT('formatUnderline()'), actionCollection(), "formatUnderline"))

		icon = Qt::IconSet.new(@icons.loadIcon("text_strike", KDE::Icon::Toolbar))
		@formatAction.insert(Action.new( i18n("Strikeout"), icon, Shortcut.new(), self, SLOT('formatStrikeout()'), actionCollection(), "formatStrikeout"))
	end
	
	def initToolBar
		@linkAction.plug(toolBar())
		@findAction.plug(toolBar())
		@formatAction.plug(toolBar())
		@deleteAction.plug(toolBar())
	end
end

if $0 == __FILE__
	description = "Note taking app"
	version     = "1.0"
	about = AboutData.new("koolboy", "Koolboy", "0.1",
						   description, AboutData::License_GPL,
						   "")

	about.addAuthor("Isaac Clerencia", "Core developer", "isaac@sindominio.net")
	about.addAuthor("Sergio Rubio", "Core developer", "sergio.rubio@hispalinux.es")

	KDE::CmdLineArgs.init(ARGV,about)
	a = KDE::Application.new()
	note = XmlNote.new("test")
	note.text = "This is koolsoft!"
	thing = NoteView.new(note)
	thing.show

	a.mainWidget = thing
	a.exec
end
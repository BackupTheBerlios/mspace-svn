require "Korundum"
include KDE
require "libkoolnotes"

#TODO
#How can I use custom icons in actions?
class NoteView < MainWindow
	signals 'aboutToClose(char*)'

	slots 'slotLink()',
	'sNoteDeleted()', 'slotFind()', 'sFormatBold()', 'sFormatUnderline()',
	'sFormatStrikeout()', 'sFormatItalic()', 'sTitleChanged()',
	'sTextChanged()'

	attr_reader :note

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

		# we need to increase refCount to be able
		# to use kmainwindows in this manner
		$kapp.ref

		self.caption = @note.title
		@titleLineEdit.text = @note.title
		@textEdit.text = @note.text if @note.text
		resize(@note.size) if @note.size
		move(@note.pos) if @note.pos
	end
	
	#####################
	#TEXT formatting	
	def sFormatItalic
		puts "FORMAT"
	end

	def sFormatUnderline
	end

	def sFormatStrikeout
	end

	def sFormatBold
	end
	###################

	def slotFind
	end

	def slotLink
	end

	def sNoteDeleted
	end
	
	def initWidgets
		@vbox = Qt::VBox.new(self)
		@vbox.show
		@titleLineEdit = LineEdit.new(@vbox);
		regex = Qt::RegExp.new("([A-Z]([a-z]*)){2,}")
		validator = Qt::RegExpValidator.new(regex,nil)
		@titleLineEdit.setValidator(validator)
		@titleLineEdit.show
		connect(@titleLineEdit, SIGNAL('returnPressed()'),
			self, SLOT('sTitleChanged()'))
		@textEdit = TextEdit.new(@vbox)
		@textEdit.show
		connect(@textEdit, SIGNAL('textChanged()'),
			self, SLOT('sTextChanged()'))
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
		@formatAction.insert(Action.new( i18n("Bold"), icon, Shortcut.new(), self, SLOT('sFormatBold()'), actionCollection(), "formatBold"))

		icon = Qt::IconSet.new(@icons.loadIcon("text_italic", KDE::Icon::Toolbar))
		@formatAction.insert(Action.new( i18n("Italic"), icon, Shortcut.new(), self, SLOT('sFormatItalic()'), actionCollection(), "formatItalic"))

		icon = Qt::IconSet.new(@icons.loadIcon("text_under", KDE::Icon::Toolbar))
		@formatAction.insert(Action.new( i18n("Underline"), icon, Shortcut.new(), self, SLOT('sFormatUnderline()'), actionCollection(), "formatUnderline"))

		icon = Qt::IconSet.new(@icons.loadIcon("text_strike", KDE::Icon::Toolbar))
		@formatAction.insert(Action.new( i18n("Strikeout"), icon, Shortcut.new(), self, SLOT('sFormatStrikeout()'), actionCollection(), "formatStrikeout"))
	end

	def sTitleChanged
		if @titleLineEdit.hasAcceptableInput
			@note.changeTitle(@titleLineEdit.text)
		end
	end

	def sTextChanged
		@note.text = @textEdit.text
	end

	def initToolBar
		@linkAction.plug(toolBar())
		@findAction.plug(toolBar())
		@formatAction.plug(toolBar())
		@deleteAction.plug(toolBar())
	end

	def queryClose
		emit aboutToClose(@note.title)
		@note.size = size
		@note.pos = pos
		@note.write
		return true
	end
end

if $0 == __FILE__
	description = "Note taking app"
	version     = "1.0"
	about = AboutData.new("koolboy", "Koolboy", "0.1",
		   description, AboutData::License_GPL, "")

	about.addAuthor("Isaac Clerencia", "Core developer", "isaac@sindominio.net")
	about.addAuthor("Sergio Rubio", "Core developer", "sergio.rubio@hispalinux.es")
	about.setBugAddress("mspace-koolboy@lists.berlios.de")

	KDE::CmdLineArgs.init(ARGV,about)
	a = KDE::Application.new()
	note = XmlNote.new("test")
	note.text = "This is koolsoft!"
	thing = NoteView.new(note)
	thing.show

	a.mainWidget = thing
	a.exec
end

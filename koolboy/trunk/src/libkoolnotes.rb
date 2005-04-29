require 'sqlite3'
require 'singleton'

# FIXME
# Use the strategy pattern to allow diferent backends
# i.e. SQL, XML (Tomboy), etc
# NoteManager should not be data backend aware

# Implement a base NoteDB and make KoolNoteWell and
# Tomboy XML backend inherit NoteDB
# NoteDB should be the Strategy used in NoteManager

#NoteManager should be a singleton also
class NoteManager
	attr_reader :dir, :notes

	def initialize (notesDir = "#{ENV['HOME']+ "/.tomboy"}")
		@dir = notesDir
		@notes = {}
		loadNotes()
	end

	def getNote ( title )
	end
	
	def lastNotes ()
	end
	
	private
	def loadNotes ()
		Dir.foreach(@dir) do |file|
			full = @dir + "/" + file
			#if file[-1,1] != "~" && !File.directory?(full)
			if file =~ /.*note$/
				note = XmlNote.new(full)
				notes[note.title] = note
			end
		end
	end
		
end

#######################################
#			Notes implementations
#######################################
class KoolNote
	attr_accessor :title
	attr_accessor :contents
	attr_reader :id

	def initialize ( title, id=-1, contents='' )
		@title = title
		@id = id
		@contents = contents
	end
end

class XmlNote
	require "rexml/document"
	include REXML
	
	attr_accessor :title, :xml, :text
	attr_reader :file
	
	def initialize(file)
		@file = file
		@xml = IO.readlines(file).join("\n")
		begin
			@doc = Document.new(@xml)
			@title = @doc.root.elements["title"].text
			@text = @doc.root.elements["text"]
		rescue
			print "IGNORING Note: " + @file
			print $!
		end
	end

	def to_s()
		return @xml
	end
	
end

###############################################
#				NoteDB Implementations
###############################################

# FIXME
# Add DB interface here
class NoteDB
end

# FIXME
# Inherit from NoteDB and
class TomboyStore
end

class KoolNoteWell
	include Singleton

	@@file = ''

	def KoolNoteWell.setFile( file )
		@@file = file
	end

	def initialize
		@db = SQLite3::Database.new(@@file)
		@db.results_as_hash = true
		count = @db.get_first_value("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='notes'")
		count == "1" or 
			@db.execute("CREATE TABLE notes ( id integer primary key autoincrement, title text, contents text, date text)")
	end

	def lastNotes
		@db.execute("SELECT * FROM notes ORDER BY date LIMIT 5")
	end

	def getNote ( title )
		note = nil
		rows = @db.execute("SELECT * FROM notes WHERE title='#{title}'")
		if rows.length == 1
			note = KoolNote.new(title, rows[0]['id'], rows[0]['contents'])
		else
			note = KoolNote.new(title)
		end
		note
	end

	def storeNote ( note )
		count = @db.get_first_value("SELECT COUNT(*) FROM notes WHERE id=#{note.id}")
		date=Qt::DateTime.currentDateTime.toString(Qt::ISODate)
		if count == "1"
			if note.title.length != 0
				@db.execute("UPDATE notes SET title='#{note.title}', contents='#{note.contents}', date='#{date}' WHERE id=#{note.id}")
			else
				@db.execute("DELETE FROM notes WHERE id=#{note.id}")
			end
		else
			if note.title.length != 0
				@db.execute("INSERT INTO notes VALUES (NULL, '#{note.title}', '#{note.contents}', '#{date}')")
			end
		end

	end
end

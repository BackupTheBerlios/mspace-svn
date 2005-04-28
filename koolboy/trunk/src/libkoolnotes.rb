require 'sqlite3'
require 'singleton'

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
		note.title.length != 0 or return

		count = @db.get_first_value("SELECT COUNT(*) FROM notes WHERE id=#{note.id}")
		date=Qt::DateTime.currentDateTime.toString(Qt::ISODate)
		if count == "1"
			@db.execute("UPDATE notes SET title='#{note.title}', contents='#{note.contents}', date='#{date}' WHERE id=#{note.id}")
		else
			@db.execute("INSERT INTO notes VALUES (NULL, '#{note.title}', '#{note.contents}', '#{date}')")
		end

	end
end

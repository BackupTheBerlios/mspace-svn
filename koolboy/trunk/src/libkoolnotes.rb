require 'sqlite3'
require 'singleton'

class NoteManager
	
	include Singleton

	attr_reader :dir

	def initialize
		@dir = KDE::StandardDirs::locateLocal("appdata","notes/")
		@notes = {}
		loadNotes()
	end

	def each
		@notes.values.each do |note|
			yield note
		end
	end
	
	def getNote ( title )
	end
	
	def lastNotes
		#may be a little unreadable but it rocks :P
		@notes.sort {|a,b| a[1].time <=> b[1].time }.collect { |x| x[0] }[0..5]
	end
	
	private
	def loadNotes
		Dir.foreach(@dir) do |file|
			#if file[-1,1] != "~" && !File.directory?(full)
			if file =~ /(.*)\.note$/
				note = XmlNote.new($1)
				@notes[note.title] = note
			end
		end
	end
		
end

#######################################
#	Notes implementations
#######################################
class XmlNote
	require "rexml/document"
	include REXML
	
	attr_accessor :title, :time
	
	def initialize(title)
		@title = title
		@read = false
		@file = KDE::StandardDirs::locateLocal(
			"appdata","notes/#{@title}.note")
		@time = File.new(@file).mtime
	end

	def read
		@xml = IO.readlines(@file).join("\n")
		begin
			@doc = Document.new(@xml)
			@text = @doc.root.elements["text"]
			@size = Qt::Size.new(@doc.root.elements["height"],
					@doc.root.elements["width"])
			@loc = Qt::Point.new(@doc.root.elements["x"],
					@doc.root.elements["y"])
			@read = true
		rescue
			p "IGNORING Note: " + @file
			p $!
		end
	end

	def to_s()
		return @xml
	end

	private

	def self.attr_after_read (*attrs)
		attrs.each do |a|
			module_eval <<-EOF
			def #{a}
				@read or read
				@#{a}
			end
			EOF
		end
	end

	attr_after_read :doc, :text, :size, :loc
end

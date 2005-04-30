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
	
	def lastNotes ()
	end
	
	private
	def loadNotes ()
		Dir.foreach(@dir) do |file|
			#if file[-1,1] != "~" && !File.directory?(full)
			if file =~ /.*note$/
				note = XmlNote.new(file)
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
	
	attr_accessor :title
	
	def initialize(title)
		@title = title
		@read = false
	end

	def read
		file = KDE::StandardDirs::locateLocal(
			"appdata","notes/#{@title}.note")
		@xml = IO.readlines(file).join("\n")
		begin
			@doc = Document.new(@xml)
			@text = @doc.root.elements["text"]
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

	attr_after_read :doc, :text
end

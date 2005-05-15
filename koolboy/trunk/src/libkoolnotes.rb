require 'singleton'
require 'rexml/document'

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

	def unusedName
		return "New note #{Time.now.year}-#{Time.now.month}-#{Time.now.day}" +
		"(#{Time.now.hour}:#{Time.now.min}:#{Time.now.sec})"
	end

	def newNote
		note = XmlNote(unusedName)
		@notes[note.title] = note
		return note
	end
		

	def getNote ( title )
		if @notes.key?(title)
			@notes[title]
		else
			XmlNote.new(title)
		end
	end
	
	def lastNotes
		#may be a little unreadable but it rocks :P
		#@notes.sort {|a,b| a[1].time <=> b[1].time }.collect { |x| x[0] }[0..5]
		@notes.values.sort_by { |x| x.time }[0..5].map { |x| x.title }
	end

	def change (note,title,text,size,pos)
		@notes.delete(note.title)
		note.change(title,text,size,pos)
		@notes[title] = note 	
	end

	def delete (note)
		@notes.delete(note.title)
		note.delete
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
	
	attr_accessor :title, :time

	def initialize(title)
		@title = title
		@file = KDE::StandardDirs::locateLocal(
			"appdata","notes/#{@title}.note")
		if File.exists?(@file)
			@read = false
			@time = File.mtime(@file)
		else
			@read = true
			@title = title
			@text = ''
			@file = KDE::StandardDirs::locateLocal(
				"appdata","notes/#{@title}.note")
			@time = Time.new
		end
	end

	def read
		begin
			@xml = IO.readlines(@file).join("\n")
			@doc = REXML::Document.new(@xml)
			@text = @doc.root.elements["text"].text.to_s
			@size = Qt::Size.new(@doc.root.elements["height"].text.to_i,
					@doc.root.elements["width"].text.to_i)
			@pos = Qt::Point.new(@doc.root.elements["x"].text.to_i,
					@doc.root.elements["y"].text.to_i)
			@read = true
		rescue
			p "IGNORING Note: " + @file
			p $!
		end
	end

	def write
		@doc = REXML::Document.new("<note/>");
		title = REXML::Element.new("title")
		title.text = @title
		text = REXML::Element.new("text")
		text.text = @text
		width = REXML::Element.new("width")
		width.text = @size.width
		height = REXML::Element.new("height")
		height.text = @size.height
		x = REXML::Element.new("x")
		x.text = @pos.x
		y = REXML::Element.new("y")
		y.text = @pos.y
		@doc << REXML::XMLDecl.new
		@doc.root << title
		@doc.root << text
		@doc.root << width
		@doc.root << height
		@doc.root << x
		@doc.root << y
		f = File.new(@file, "w")
		@doc.write(f)
		f.close
	end

	def changeTitle(title)
		File.exists?(@file) and File.delete(@file)	
		@title = title
		@file = KDE::StandardDirs::locateLocal(
			"appdata","notes/#{@title}.note")
		@time = Time.new
		write
	end

	def delete
		File.delete(@file)	
	end

	def to_s
		@read or read
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

			def #{a}= (val)
				@#{a} = val
				write
			end
			EOF
		end
	end

	attr_after_read :text, :size, :pos
end

if $0 == __FILE__
	puts NoteManager.instance.unusedName
end

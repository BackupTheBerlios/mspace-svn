require "libkoolnotes.rb"
##reads tomboy notes
if $0 == __FILE__
	manager = NoteManager.new
	manager.notes.values.each do |note|
		puts note.title
		puts note.file
		#puts note.text
	end
end	
#!/usr/bin/ruby
require "fileutils"

def usage
	puts "Usage: #{__FILE__} <directory> | <file>"
	exit
end

def normalizeDir(dir)
	Dir.foreach(dir) { |file|
		return if File.directory?(dir)
		normalizeFile(dir + "/" + file)
	}
end

def normalizeFile(file)
	ext = file[file.rindex(".") .. file.length]
	basename = File.basename(file, ext)
	dir = File.dirname(file)
	return if basename == "." or basename == ".."
	newName = dir + "/" + basename.scan(/\w+/).join("_") + ext
	#The file does not need to be renamed
	return if File.exist?(newName)
	FileUtils.mv(file, newName)
end

if $0 == __FILE__
	arg = ARGV[0]
	usage if !arg
	if File.directory?(arg)
		normalizeDir(arg)
	else
		normalizeFile(arg)
	end
end

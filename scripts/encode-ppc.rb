#!/usr/bin/ruby
require "shell"

file = ARGV[0]
if !file
	puts "no args"
	exit
end
destfile = "PocketMovie_" + File.basename(file)
cmd = "mencoder"
args = "#{file} -aid 129 -noskip -vf scale=320:240 -of mpeg -ovc lavc -lavcopts vcodec=mpeg1video:vbitrate=400000:vrc_minrate=100:vrc_maxrate=500:vrc_buf_size=300:vpass=1 -oac lavc -lavcopts acodec=mp2:abitrate=96 -o #{destfile}"

Shell.def_system_command(cmd, args)

puts "Encoding #{file} to #{destfile}"
`#{cmd} #{args}`


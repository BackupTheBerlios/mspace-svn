#!/usr/bin/ruby

def usage
	puts "Usage: encode-ppc.rb <file>"
end

file = ARGV[0]
if !file
	usage
	exit
end

destfile = "PocketMovie_" + File.basename(file)
cmd = "mencoder"
#extraopts -aid 129
args = "#{file} -noskip -vf scale=320:240 -of mpeg -ovc lavc -lavcopts vcodec=mpeg1video:vbitrate=400000:vrc_minrate=100:vrc_maxrate=500:vrc_buf_size=300:vpass=1 -oac copy -o #{destfile}"

puts "Encoding #{file} to #{destfile}"
`#{cmd} #{args}`


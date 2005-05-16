#!/usr/bin/ruby

def usage
	puts "Usage: encode-ppc.rb <file>"
end


def encodeFile(file)
	basename = File.basename(file)
	puts "Encoding file: #{basename}"
	destfile = "PocketPC_" + basename
	cmd = "mencoder"
	## PocketDivxEncoder args for PocketPC ##
	pocketpc_opts = "-srate 22050 -oac mp3lame -lameopts mode=2:cbr:br=64 -vf scale=320:240,eq2=1.0:1.0:0.03:1.0 -sws 2 -ovc lavc -lavcopts vcodec=mpeg4:vhq:vbitrate=248 -ffourcc DX50"
	lame_args = "#{file} -noskip -vf scale=320:240 -of mpeg -ovc lavc -lavcopts vcodec=mpeg1video:vbitrate=400000:vrc_minrate=100:vrc_maxrate=500:vrc_buf_size=300:vpass=1 -oac copy -o #{destfile}"
	`#{cmd} #{file} #{pocketpc_opts} -o #{destfile}`
end

def encodeDir(dir)
	puts "Encoding al files in #{dir}"
	fcount = 0
	Dir.foreach(ARGV[0]) { |file|
		encodeFile(file) if file =~ /.*\.(rb|avi|mpeg|mpg|divx)$/
		fcount.next
	}
	puts "#{fcount} files encoded."
end

if $0 == __FILE__
	file = ARGV[0]
	if !file
		usage
		exit
	end
	if File.directory?(file)
		encodeDir(file)
	else
		encodeFile(file)
	end
end


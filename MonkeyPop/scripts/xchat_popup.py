#
# This script uses MonkeyPop and the Chicken Framework
# to notify the user about XChat events.
# 
# Get MonkeyPop at:
# http://mspace.berlios.de/gunther-user/view.php/page/MonkeyPop
# 
# Get the Chicken Framework at:
# http://mspace.berlios.de/gunther-user/view.php/page/Chicken-Framework
#
# AUTHORS
# --------
# Luis Bosque
# Sergio Rubio
#

__module_name__ = "Popup"
__module_version__ = "0.1" 
__module_description__ = "Shows a popup when something happens"

#
# set this to 0 if you don't want a popup
# when any user joins or leaves a channel
#
enable_join_leave_notifications = 1

#
# This is the popup timeout in milliseconds
# If you want the popup to stay longer, change this value
#
timeout = 3500 


#
# XChat will notify of joins only for this users, add
# as many as you want.
#
desired_users = ["fraggelu", "rubiojr"]


#
#   You don't need to modify this
#
import os
import xchat
max_length = 25

def replace_header_body (header, body):
    command = 'monkeypop --width 250 --height 50 --svg --file ~/.xchat2/XChat.svg --timeout ' + str (timeout) + ' --text "' + ellipsize (body) + '" --header "' + header + '"&'
    return command
    

# strip the line if it's too long
def ellipsize (text):
    if len(text) > max_length:
	text = text[0:max_length] + ' ...'
    return text

# Notify when a user leaves the channel
def leave_cb(word, word_eol, userdata):
    header = word[0] + ' LEAVES ' + word[1]
    body = ''
    os.system (replace_header_body (header, body))
    return xchat.EAT_XCHAT #

# Notify when a user joins the channel
def join_cb(word):
    header = word[1]
    body = word[0] + ' JOINS the channel'
    os.system (replace_header_body (header, body))
    return xchat.EAT_XCHAT #

def check_join_notifies(word, word_eol, userdata):
	if enable_join_leave_notifications == 1 or word[0] in desired_users:
		join_cb(word)
	else:
		return xchat.EAT_NONE

def check_leave_notifies(word, word_eol, userdata):
	if enable_join_leave_notifications == 1 or word[0] in desired_users:
		leave_cb(word)
	else:
		return xchat.EAT_NONE

# Notify when we are connecting to a server
def connecting_cb(word, word_eol, userdata): 
    header = 'Connecting to...' 
    body = word[1]
    os.system (replace_header_body (header, body))
    return xchat.EAT_NONE #

# Notify when the channel topic changes
def topic_changed_cb(word, word_eol, userdata):
    header = 'Topic changed'
    body = word[1]
    os.system (replace_header_body (header, body))
    return xchat.EAT_NONE #

def private_message_cb(word, word_eol, userdata): 
    header = 'Private message'
    body = word[1]
    os.system (replace_header_body (header, body))
    return xchat.EAT_NONE #

# Notify when someone says your name in the channel
def onnick_cb(word, word_eol, userdata):
    # replace the nick
    nick = xchat.get_info ('nick')
    if word[1].startswith (nick):
	text = word[1].replace (nick, '')
    else:
	text = word[1]
    
    # string the , simbol
    text.replace (',','')

    header = word[0] + ' says:'
    body = text
    os.system (replace_header_body (header, body))
    return xchat.EAT_NONE

# Hook here to the events
xchat.hook_print("Channel Msg Hilight", onnick_cb)
xchat.hook_print("Topic Change", topic_changed_cb)
xchat.hook_print("Private Message", private_message_cb)
xchat.hook_print("Connecting", connecting_cb)

# some users find this anoying so it is optional
xchat.hook_print("Join", check_join_notifies)
xchat.hook_print("Part", check_leave_notifies)

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
# Sergio Rubio
# Luis Bosque
#

__module_name__ = "Popup"
__module_version__ = "0.2" 
__module_description__ = "Shows a popup when something happens"

#
#   You don't need to modify this
#
import os
import xchat

ruta = os.getenv("HOME") + "/.xchat2/xchat_popup.conf"


# Si no existe un fichero de configuracion xchat_popup.conf generara
# las variables necesarias con valor 1.
# En cambio si existe, cargara del fichero de configuracion los valores
# de todas las variables necesarias.

desired_users = []	
if not os.path.exists(ruta):
	valores = {"enable_join_leave_notifications":1,
			"enable_connecting_notification":1,
			"enable_topic_changed_notification":1,
			"enable_private_message_notification":1,
			"enable_nick_said_notification":1}
else:		
	file = open(ruta)
	valores = {}
	while 1:
		line = file.readline()
		if not line:
			break
		else:
			if line.startswith("desired_users"):
				subcadena = line.split(" ")[1]
				nombres = subcadena.split(",")
				for i in nombres:
					desired_users.append(i) 
			else:	
				valores[line.split(" ")[0]] = int(line.split(" ")[1])
	file.close()		


#
# enable_join_leave_notifications = 1
# set this to 0 if you don't want a popup
# when any user joins or leaves a channel
#

#
# enable_connecting_notification = 1
# set this to 0 if you don't want a popup
# when you are connecting to any server
#

#
# enable_topic_changed_notification = 1
# set this to 0 if you don't want a popup
# when the topic changes in any of the channels
# you are joined
#

#
# enable_nick_said_notification = 1
# enable_private_message_notification = 1
# set this to 0 if you don't want a popup
# when any user send you a private message
#

#
# set this to 0 if you don't want a popup
# when any user says your nick in the channel
#



#
# This is the popup timeout in milliseconds
# If you want the popup to stay longer, change this value
#
timeout = 3500 


#
# XChat will notify of joins only for this users, add
# as many as you want.
#

#
# TamaÃ±o maximo de cadena que vamos a enviar al popup
#
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
	if valores["enable_join_leave_notifications"] == 1 or word[0] in desired_users:
		join_cb(word)
	else:
		return xchat.EAT_NONE

def check_leave_notifies(word, word_eol, userdata):
	if valores["enable_join_leave_notifications"] == 1 or word[0] in desired_users:
		leave_cb(word)
	else:
		return xchat.EAT_NONE

# Notify when we are connecting to a server
def connecting_cb(word, word_eol, userdata): 
	if valores["enable_connecting_notification"] == 1:
		header = 'Connecting to...' 
		body = word[1]
		os.system (replace_header_body (header, body))
	return xchat.EAT_NONE #

# Notify when the channel topic changes
def topic_changed_cb(word, word_eol, userdata):
	if valores["enable_topic_changed_notification"] == 1:
		header = 'Topic changed'
		body = word[1]
		os.system (replace_header_body (header, body))
	return xchat.EAT_NONE #

def private_message_cb(word, word_eol, userdata): 
	if valores["enable_private_message_notification"] == 1:
		header = 'Private message'
		body = word[1]
		os.system (replace_header_body (header, body))
	return xchat.EAT_NONE #

# Notify when someone says your name in the channel
def onnick_cb(word, word_eol, userdata):
    # replace the nick
    if valores["enable_nick_said_notification"] == 1:
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

# Cambios en los valores de notificacion
def change_notify(word, word_eol, userdata):
	global valores
	if word[0] == "setnicknotify":
		valores["enable_nick_said_notification"] = int(word[1])
	elif "setjoinnotify":
		valores["enable_join_leave_notifications"] = int(word[1])
	elif "setprivatenotify":
		valores["enable_private_message_notification"] = int(word[1])
	elif "setconnectnotify":
		valores["enable_connecting_notification"] = int(word[1])
	elif "settopicnotify":
		valores["enable_topic_changed_notification"] = int(word[1])
	else:
		return xchat.EAT_NONE
	
	save_conf()
		

def adduser_notify(word, word_eol, userdata):
	global desired_users
	if word[1] in desired_users:
		print "El usuario ya esta en tu lista de seguimiento"
		xchat.EAT_NONE
	else:
		desired_users += [word[1]]
		save_conf()

def remove_user_notify(word, word_eol, userdata):
	global desired_users
	if word[1] not in desired_users:
		print "Actualmente no estas llevando el seguimiento de el usuario " + word[1]
		xchat.EAT_NONE
	else:
		desired_users.remove(word[1])
		save_conf()

def save_conf():
	global valores
	global desired_users
	file = open(ruta,'w')
	for i in valores:
		file.write(i +" " + str(valores[i])+"\n")	

	file.write('desired_users ')
	cont = 0
	for i in desired_users:
		if cont is not 0:
			file.write(",")
		file.write(i)
		cont += 1		
	file.close()			
	return xchat.EAT_NONE
	

# Hook here to the events
xchat.hook_print("Connecting", connecting_cb)
xchat.hook_print("Channel Msg Hilight", onnick_cb)
xchat.hook_print("Topic Change", topic_changed_cb)
xchat.hook_print("Private Message", private_message_cb)

# some users find this anoying so it is optional
xchat.hook_print("Join", check_join_notifies)
xchat.hook_print("Part", check_leave_notifies)

# Creacion de los comandos que vamos a utilizar en xchat
# para modificar en tiempo real los valores de las variables utilizadas
xchat.hook_command("setnicknotify",change_notify, help="Activa o desactiva la notificacion producida cuando alguien te nombra en un canal")
xchat.hook_command("setjoinnotify",change_notify, help="Activa o desactiva la notificacion producida al unirse alguien al canal")
xchat.hook_command("setprivatenotify",change_notify, help="Activa o desactiva la notificacion producida al enviarte alguien un mensaje privado")
xchat.hook_command("settopicnotify",change_notify, help="Activa o desactiva la notificacion producida cuando se cambia el topic de cualquier canal donde estes")
xchat.hook_command("setconnectnotify",change_notify, help="Activa o desactiva la notificacion producida cuado te estas conectando a un servidor")
xchat.hook_command("addusernotify", adduser_notify, help="Añade notify de un usuario")
xchat.hook_command("removeusernotify", remove_user_notify, help="Elimina el notify de un usuario")


print "Script POPUP succesfully loaded!"

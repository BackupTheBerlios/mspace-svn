#!/bin/bash
#variables que vamos a utilizar
GROUP=dropbox
USER=
IP=
GUEST=
GUEST_MAIL=

############################
## Comprobaciones previas ##
############################
#Comprobamos si zenity está instalado
echo comprobando zenity
if test -z `which zenity`; then
    printf "\n\033[31;1mDebes tener \033[32;1mzenity \033[31;1minstalado en tu sistema para poder utilizar este script\033[0m\n\n"
    exit 1
fi

#Comprobamos si el script se esa ejecutando como root
if test `whoami` != "root"; then
    zenity --info --text="Debes ejecutar el script como usuario root." --title="Usuario sin privilegios"
    exit 1
fi


#Comprobamos si scponly está instalado
if test -z `which scponly`; then
    zenity --info --text="Debes tener scponly instalado para poder continuar." --title="scponly"
    exit 1
fi
    
#Captura el nombre del usuario que ejecuta el script
USER=`zenity --title "Nombre de usuario" --entry --text="Escribe tu nombre de usuario.\n"`
if test -z `cat /etc/passwd | grep $USER`; then
    zenity --info --title "No puedo continuar" --text="El usuario $USER no existe en el sistema.\nElija otro nombre por favor.\n"
    exit 1
fi

#captura el nombre del invitado
GUEST=`zenity --title "Nombre del invitado" --entry --text="Escribe el nombre de usuario que quieres para el invitado.\nEn minúsculas y sin espacios.\n"`
if test `cat /etc/passwd | grep $GUEST`; then
    zenity --info --title "No puedo continuar" --text="El usuario $GUEST ya existe en el sistema.\nElija otro nombre por favor.\n"
    exit 1
fi

#captura del correo del invitado
GUEST_MAIL=`zenity --title "e-mail del invitado" --entry --text="Escribe el e-mail de tu invitado.\n"`


#captura la ip del usuario
IP=`zenity --title "IP del mi máquina" --entry --text="Escribe la ip de tu máquina o el nobre de dominio.\n"`

#Añade el grupo dropbox al sistema si no existe
if test -z `cat /etc/group|grep $GROUP`; then
    #el grupo no existe, lo creamos
    addgroup $GROUP
fi

#Creamos el buzón si no existe y le damos los permisos adecuados
DROP_FOLDER="/home/$USER/Dropbox"
if test ! -d $DROP_FOLDER; then
    mkdir $DROP_FOLDER
fi
chown $USER $DROP_FOLDER
chgrp $GROUP $DROP_FOLDER
chmod g+srw $DROP_FOLDER

#Crea el usuario invitado en el grupo dropbox
useradd -d /home/$USER/Dropbox -g $GROUP -s /usr/bin/scponly $GUEST
echo $GUEST:$GUEST | chpasswd

#reemplaza @USER@ e @IP@ en el fichero Dropbox.desktop
#sed -i "s/@USER@/$USER/" $USER-$GUEST-Dropbox.desktop
#sed -i "s/@GUEST@/$GUEST/" $USER-$GUEST-Dropbox.desktop
#sed -i "s/@IP@/$IP/" $USER-$GUEST-Dropbox.desktop

zenity --info --title="Proceso finalizado" --text="$GUEST es ahora tu invitado.\nMándale el archivo $USER-$GUEST-Dropbox.desktop creado por\ncorreo y podrá empezar a usar tu buzón."


echo "[Desktop Entry]
Version=1.0
Encoding=UTF-8
Name=$USER DropBox
Type=Link
URL=sftp://$GUEST@$IP:/home/$USER/Dropbox
Terminal=false
Icon=gnome-fs-share.png" > $USER-$GUEST-Dropbox.desktop

if test `which sudo`; then
    sudo -u $USER gnome-open mailto:$GUEST_MAIL $USER-$GUEST-Dropbox.desktop
fi

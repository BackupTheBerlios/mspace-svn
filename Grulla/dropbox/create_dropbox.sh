#!/bin/bash
#variables que vamos a utilizar
IP=
GROUP=dropbox
USER=
GUEST=
DROP_FOLDER=
if test `whoami` != "root"; then
    zenity --info --text="Debes ejecutar el script como usuario root." --title="Usuario sin privilegios"
    exit 1
fi

#A�ade el grupo dropbox al sistema si no existe
if test -z `cat /etc/group|grep $GROUP`; then
    #el grupo no existe, lo creamos
    addgroup $GROUP
fi

#Captura el nombre del usuario que ejecuta el script
USER=`zenity --title "Nombre de usuario" --entry --text="Escribe tu nombre de usuario.\n"`

#Creamos el buz�n si no existe y le damos los permisos adecuados
DROP_FOLDER="/home/$USER/Dropbox"
if !test -d $DROP_FOLDER; then
    mkdir $DROP_FOLDER
fi
chown $USER $DROP_FOLDER
chgrp $GROUP $DROP_FOLDER
chmod g+srw $DROP_FOLDER


#captura el nombre del invitado
GUEST=`zenity --title "Nombre del invitado" --entry --text="Escribe el nombre de usuario que quieres para el invitado.\nEn min�sculas y sin espacios.\n"`

#captura la ip del usuario
IP=`zenity --title "IP del mi m�quina" --entry --text="Escribe la ip de tu m�quina o el nobre de dominio.\n"`

#Crea el usuario invitado en el grupo dropbox
useradd -d /home/$USER/Dropbox -g $GROUP -s /usr/bin/scponly $GUEST

#Renombra el fichero .desktop a $USER-Dropbox.desktop
cp Dropbox.desktop $USER-Dropbox.desktop

#reemplaza @USER@ e @IP@ en el fichero Dropbox.desktop
sed -i "s/@USER@/$USER/" $USER-Dropbox.desktop
sed -i "s/@IP@/$IP/" $USER-Dropbox.desktop

zenity --info --title="Proceso finalizado" --text="$GUEST es ahora tu invitado.\n M�ndale el archivo $USER-Dropbox.desktop por\n correo y podr� empezar a usar tu buz�n."

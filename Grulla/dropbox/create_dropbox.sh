#!/bin/bash
#variables que vamos a utilizar
IP=
GROUP=dropbox
USER=
GUEST=
DROP_FOLDER=


############################
## Comprobaciones previas ##
############################
if test -z `which zenity`; then
    printf "\n\033[31;1mDebes tener zenity instalado en tu sistema para poder utilizar este script\033[0m\n\n"
    exit 1
fi

if test ! -f Dropbox.desktop; then
    printf "\n\033[31;1mDebes ejecutar el script desde el directorio donde lo has descomprimido\033[0m\n\n"
    exit 1
fi
    
if test `whoami` != "root"; then
    zenity --info --text="Debes ejecutar el script como usuario root." --title="Usuario sin privilegios"
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
if test -n `cat /etc/passwd | grep $GUEST`; then
    zenity --info --title "No puedo continuar" --text="El usuario $GUEST ya existe en el sistema.\nElija otro nombre por favor.\n"
    exit 1

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
else
    useradd -d /home/$USER/Dropbox -g $GROUP -s /usr/bin/scponly $GUEST
fi

#Renombra el fichero .desktop a $USER-Dropbox.desktop
cp Dropbox.desktop $USER-Dropbox.desktop

#reemplaza @USER@ e @IP@ en el fichero Dropbox.desktop
sed -i "s/@USER@/$USER/" $USER-Dropbox.desktop
sed -i "s/@IP@/$IP/" $USER-Dropbox.desktop

zenity --info --title="Proceso finalizado" --text="$GUEST es ahora tu invitado.\n Mándale el archivo $USER-Dropbox.desktop por\n correo y podrá empezar a usar tu buzón."

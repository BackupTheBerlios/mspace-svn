
 This script uses MonkeyPop and the Chicken Framework
 to notify the user about XChat events.
 
 Get MonkeyPop at:
 http://mspace.berlios.de/gunther-user/view.php/page/MonkeyPop
 
 Get the Chicken Framework at:
 http://mspace.berlios.de/gunther-user/view.php/page/Chicken-Framework

 AUTHORS
 --------
 Luis Bosque
 Sergio Rubio


 Modo de empleo:
	Copiar el script y la imagen XChat.svg en la carpeta .xchat2 de tu directorio de usuario.

 Este script muetra un popup de monkeypop cada vez que ocurre en xchat cualquiera de los siguienes eventos:

 * Si la variable "enable_private_message_notification" esta puesta a 1 cada vez que alguien nos envie un mensaje privado 
 se notificara mediante popup.

 * Si la variable "enable_connecting_notification" esta puesta a 1, se notificara mediante el popup la informacion
 de intento de conexion a un servidor

 * Si la variable "enable_join_leave_notification" esta puesta a 1, se hara una notificacion mediante popup cada
 vez que un usuario entre o salga del canal.

 * Si la variable "enable_topic_changed_notification" esta puesta a 1, cada vez que se modifique el topic del canal,
 se recibira una notificacion

 * Si la variable "enable_nick_said_notification" esta puesta a 1, cada vez que alguien mencione nuestro nick en un canal
 recibiremos una notificacion

A parte de estas variables, vamos a poder tener una lista de usuarios para aplicarles un seguimiento simple. Esto es que, si la variable "enable_join_leave_notification" estuviese puesta a 0, automaticamente se miraria esa lista de usuarios y solo se harian las notificaciones de entrada y salida del canal de los usuarios puestos en esa lista.

Todas estas variables se encontraran almacenadas en un fichero llamado xchat_popup.conf situado en la carpeta .xchat2 de nuestro home. Se ha de respetar totalmente la sintaxis que muestra. Si el fichero no existiese se generaria uno nuevo con unos valores por defecto al arrancar el script.

 Todos estos valores, tambien se pueden modificar en tiempo real desde xchat escribiendo los siguientes comandos:

 Para modificar "enable_private_message_notification":
	/setprivatenotify valor		"siendo el valor 0 o 1"

 Para modificar "enable_connecting_notification":
	/setconnectnotify valor		"siendo el valor 0 o 1"

 Para modificar "enable_join_leave_notification":
	/setjoinnotify valor		"siendo el valor 0 o 1"

 Para modificar "enable_topic_changed_notification":
	/settopicnotify	valor		"siendo el valor 0 o 1"

 Para modificar "enable_nick_said_notification":
	/setnicknotify valor		"siendo el valor 0 o 1"

 Para añadir un usuario a la lista de seguimiento:
	/addusernotify nick

 Para quitar un usuario de la lista de seguimiento:
	/removeusernotify


#!/bin/bash

#############################################
# Script principal de gestion de la HA.
# No es usable directamente. Para utilizarlo,
# se ha de hacer desde /etc/init.d/ha
#
# AUTHOR: Luis Bosque
# DATE:   20060309.
# NAME:   ha.sh
#

BASE_PATH=/etc/HA/
CONF_FILE=${BASE_PATH}ha.cf
HA_PIDFILE=/var/run/ha.pid

sleep 2
. ${CONF_FILE}

echo ${BODY} | ${MAIL} -s ${SUBJECT} ${MAILTO}

### Bucle principal del sistema de HA
while [ true ]
do
	. ${IP_CHECK}
	. ${ROUTES_CHECK}
	. ${FW_CHECK}
	. ${SERVICE_CHECK}
	sleep ${SLEEP_TIME}
done

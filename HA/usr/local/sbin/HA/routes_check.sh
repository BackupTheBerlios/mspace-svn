#!/bin/bash

################################################
#
#  SCRIPT PARA COMPROBAR SI LAS IPS VIRTUALES
#  ESTAN LEVANTADAS. SI NO LO ESTAN SE LEVANTAN
#


CONF_FILE=${BASE_PATH}ha.cf
. ${CONF_FILE}
ROUTES_STATUS="CHECK"
RETRY_COUNT=2
COUNTER=0

while [[ ${ROUTES_STATUS} != "OK" && ${COUNTER} -lt ${RETRY_COUNT} ]]
do
	if [[ `/etc/init.d/routes status` = "stopped" ]]
	then
		/etc/init.d/routes start
		if [ ${?} != 0 ]
		then
			ROUTES_STATUS="FAIL"
			break;
		else
			ROUTES_STATUS="OK"
		fi
	else
		ROUTES_STATUS="OK"
	fi
	let "COUNTER+=1"
done
echo "`date "+%x %X"` ESTADO DE LAS RUTAS: ${ROUTES_STATUS}" >> ${PATH_LOG}

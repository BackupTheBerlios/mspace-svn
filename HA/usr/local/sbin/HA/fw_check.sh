#!/bin/bash

################################################
#
#  SCRIPT PARA COMPROBAR SI LAS IPS VIRTUALES
#  ESTAN LEVANTADAS. SI NO LO ESTAN SE LEVANTAN
#


CONF_FILE=${BASE_PATH}ha.cf
. ${CONF_FILE}
FW_STATUS="CHECK"
RETRY_COUNT=2
COUNTER=0

while [[ ${FW_STATUS} != "OK" && ${COUNTER} -lt ${RETRY_COUNT} ]]
do
	if [[ `/etc/init.d/firewall status` = "stopped" ]]
	then
		/etc/init.d/firewall start
		if [ ${?} != 0 ]
		then
			FW_STATUS="FAIL"
			break;
		else
			FW_STATUS="OK"
		fi
	else
		FW_STATUS="OK"
	fi
	let "COUNTER+=1"
done
echo "`date "+%x %X"` ESTADO DEL FIREWALL: ${IP_STATUS}" >> ${PATH_LOG}

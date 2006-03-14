#!/bin/bash

################################################
#
#  SCRIPT PARA COMPROBAR SI LOS SERVICIOS DE RED
#  ESTAN FUNCIONANDO. SINO, LOS INTENTA ARRANCAR.
#


CONF_FILE=${BASE_PATH}ha.cf
. ${CONF_FILE}
SERVICE_STATUS="CHECK"
RETRY_COUNT=2
COUNTER=0

while [[ ${SERVICE_STATUS} != "OK" && ${COUNTER} -lt ${RETRY_COUNT} ]]
do
	for i in ${SERVICES}
	do
		if [[ -n `/etc/init.d/${i} status |grep stopped` ]]
		then
			/etc/init.d/${i} start
			if [ ${?} != 0 ]
			then
				SERVICE_STATUS="FAIL"
				break;
			else
				SERVICE_STATUS="OK"
			fi
		else
                                SERVICE_STATUS="OK"
		fi 
	done
	let "COUNTER+=1"
done
echo "`date "+%x %X"` ESTADO DE LOS SERVICIOS: ${SERVICE_STATUS}" >> ${PATH_LOG}

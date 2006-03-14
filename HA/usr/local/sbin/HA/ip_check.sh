#!/bin/bash

################################################
#
#  SCRIPT PARA COMPROBAR SI LAS IPS VIRTUALES
#  ESTAN LEVANTADAS. SI NO LO ESTAN SE LEVANTAN
#


CONF_FILE=${BASE_PATH}ha.cf
. ${CONF_FILE}
IP_STATUS="CHECK"
RETRY_COUNT=2
COUNTER=0

while [[ ${IP_STATUS} != "OK" && ${COUNTER} -lt ${RETRY_COUNT} ]]
do
	for i in ${IFACES}
	do
		IFACE=`echo ${i} |cut -d_ -f1`
		HOST=`echo ${i} |cut -d_ -f2 |cut -d/ -f1`
		NETMASK=`echo ${i} |cut -d_ -f2 |cut -d/ -f2`
		if [[ -z `${IFCONFIG} ${IFACE} |grep inet` ]]
		then
			${IFCONFIG} ${IFACE} ${HOST} netmask ${NETMASK} up
			if [ ${?} != 0 ]
			then
				IP_STATUS="FAIL"
				break;
			else
				IP_STATUS="OK"
				arping -q -w 5 -U ${HOST}
								
			fi
		else
			IP_STATUS="OK"
		fi 
	done
	let "COUNTER+=1"
done
echo "`date "+%x %X"` ESTADO DE LA RED: ${IP_STATUS}" >> ${PATH_LOG}

#!/bin/bash

############################################
# Script demonio para HA que monitoriza
# el estado del nodoA y en caso de perdida
# de comunicacion con el mismo, arranca
# todos los recursos sustituyendo por
# completo al nodoA. El script solo funciona
# ejecutandose desde /etc/init.d/remota_ha.
# No tiene funcionamiento independiente.
# 
# AUTHOR: Luis Bosque
# DATE:   20060309.
# NAME:   remote_ha
#

HA_INIT_SCRIPT=/etc/init.d/ha

PING=/bin/ping

REPEAT=1
TIMEOUT=10
TIMESLEEP=5
TTL=1

CONTINUE=YES
CRITICAL_STATUS=NO

CHECK_IPS="ip_fija1
ip_fija2"

NODE_IPS="virtual_ip1
virtual_ip2
virtual_ip3"


IP_STATUS ()
{
	${PING} -c ${REPEAT} -w ${TIMEOUT} -t ${TTL} ${1} > /dev/null 2>&1
}


while [ ${CONTINUE} = "YES" ]
do
	echo "**** COMPROBANDO COMUNICACION LOCAL ****"
	LOCAL=CHECK
	while [[ ${LOCAL} != "OK" ]]
	do
		LOCAL=CHECK
		for i in ${CHECK_IPS}
		do
			IP_STATUS ${i}
			if [[ ${?} = 0 && ${LOCAL} != "FAIL" ]]
			then
				LOCAL=OK
			else
				LOCAL=FAIL
			fi
		done
	done
	echo "**** COMUNICACION LOCAL CORRECTA ****"
	
	echo "**** COMPROBANDO COMUNICACION REMOTA ****"
	REMOTE=CHECK
	for i in ${NODE_IPS}
	do
		IP_STATUS ${i}
		if [[ ${?} = 0 && ${REMOTE} != "FAIL" ]]
		then
			REMOTE=OK
		else
			REMOTE=FAIL
			break;
		fi
	done
	if [ ${REMOTE} = "OK" ]
	then
		echo "**** COMUNICACION REMOTA CORRECTA ****"
		sleep 10
	else
		echo "**** COMUNICACION REMOTA CRITICA ****"
		CONTINUE=NO
		CRITICAL_STATUS=YES
	fi	
done

if [[ ${CRITICAL_STATUS} = "YES" ]]
then
	${HA_INIT_SCRIPT} start
	/etc/init.d/remote_ha stop
fi

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

TOTAL_SERVICE_IPS=`echo ${IFACES} |wc -w`


IP_STATUS ()
{
        HOST=`echo ${1} |cut -d_ -f2 |cut -d/ -f1`
	${PING} -c ${REPEAT} -w ${TIMEOUT} -t ${TTL} ${HOST} > /dev/null 2>&1
}

failover ()
{
	${HA_INIT_SCRIPT} start
	/etc/init.d/remote_ha stop
}

stonith ()
{
	ssh -i ${STONITH_PRIVATE_KEY} ${STONITH_USER}@${1} ${STONITH_COMMAND}
}

while [ ${CONTINUE} = "YES" ]
do
	echo "**** COMPROBANDO COMUNICACION LOCAL ****"
	LOCAL=CHECK
	while [[ ${LOCAL} != "OK" ]]
	do
		LOCAL=CHECK
		for i in ${SECURE_IPS}
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
	FAIL_IPS=0
	for i in ${IFACES}
	do
		IP_STATUS ${i}
		if [[ ${?} != 0 ]]
		then
			IP_STATUS ${i}
			if [[ ${?} != 0 ]]
			then
				let "FAIL_IPS+=1"
			else
				SURVIVOR_IP=${i}
			fi
		else
			SURVIVOR_IP=${i}
		fi
	done
	
	if [[ ${FAIL_IPS} = 0 ]]
	then
		REMOTE="UP"
		sleep 10
	elif [[ ${FAIL_IPS} = ${TOTAL_SERVICE_IPS} ]]
	then
		REMOTE="DOWN"
		CONTINUE=NO
		failover
	else
		REMOTE="KILL"
		stonith ${SURVIVOR_IP}
		sleep 5
		failover
	fi
}

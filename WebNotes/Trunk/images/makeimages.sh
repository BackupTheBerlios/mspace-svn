#!/bin/bash 

echo 'IMAGERS =' > imagers.list
for i in `ls *.png`; do echo IMAGERS += /resource:../../images/$i,$i >> imagers.list ;done

#!/bin/bash 

echo 'GLADERS =' > gladers.list
for i in `ls *.glade`; do echo GLADERS += /resource:../../glade/$i,$i >> gladers.list ;done

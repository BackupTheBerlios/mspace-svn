#!/bin/bash 

echo 'IMAGES =' > images.list
for i in `ls *.png`; do echo IMAGES += /resource:images/$i,$i >> images.list ;done

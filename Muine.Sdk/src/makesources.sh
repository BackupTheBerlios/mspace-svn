#!/bin/bash 
UNWANTED="tests"
echo CSSOURCES= > sources.list
for i in `find ./ -iname '*.cs'`; do echo CSSOURCES += $i >> sources.list ;done
grep -v -E "($UNWANTED)" sources.list > sources.list.fixed
mv sources.list.fixed sources.list

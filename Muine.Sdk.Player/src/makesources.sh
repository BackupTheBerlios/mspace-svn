#!/bin/bash 

echo 'SOURCES = \' > sources.list
for i in `find ./ -iname '*.cs'`; do echo $i\\ >> sources.list ;done
grep -v -e 'tests' sources.list > sources.list.fixed
mv sources.list.fixed sources.list

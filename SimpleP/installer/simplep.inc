#!/bin/bash
pushd @@installdir@@/lib/simplep
SLIBS=@@installdir@@/lib/simplep
LD_LIBRARY_PATH=$SLIBS:$LD_LIBRARY_PATH MONO_PATH=$SLIBS:$MONO_PATH mono SimpleP.exe
popd

#!/bin/sh

rsync -av --delete ~/unitybuild/Server/* pota@gacrux.uberspace.de:/home/pota/apps/2ds
ssh pota@gacrux.uberspace.de -C "supervisorctl restart 2ds"
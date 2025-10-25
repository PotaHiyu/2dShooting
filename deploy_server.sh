#!/bin/sh

rsync -av --delete ./server_build/* shostakn@horologium.uberspace.de:/home/shostakn/apps/2ds
ssh shostakn@horologium.uberspace.de -C "supervisorctl restart 2ds"

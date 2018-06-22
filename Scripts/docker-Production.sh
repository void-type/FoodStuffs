#! /bin/bash

cd ../
docker build --build-arg env=Production -t foodstuffs-production .
docker run -it --rm -p 3333:80 --name foodstuffs-production foodstuffs-production
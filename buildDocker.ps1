#! /usr/bin/pwsh

docker build -t foodstuffs-prod .
docker run -it --rm -p 3333:80 --name foodstuffs-prod foodstuffs-prod
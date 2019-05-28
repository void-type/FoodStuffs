#! /bin/bash

RUN curl -sL https://deb.nodesource.com/setup_10.x | bash - \
  && apt update \
  && apt install -y nodejs npm \
  && npm install -g npm@latest

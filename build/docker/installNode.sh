NODE_VERSION=8.12.0
NODE_DOWNLOAD_SHA=3df19b748ee2b6dfe3a03448ebc6186a3a86aeab557018d77a0f7f3314594ef6
NODE_DOWNLOAD_URL=https://nodejs.org/dist/v$NODE_VERSION/node-v$NODE_VERSION-linux-x64.tar.gz

curl -SL "$NODE_DOWNLOAD_URL" --output nodejs.tar.gz
echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c -
tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1
rm nodejs.tar.gz
ln -f -s /usr/local/bin/node /usr/local/bin/nodejs

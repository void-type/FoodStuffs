const nbgv = require('nerdbank-gitversioning');
const fs = require('fs');

nbgv.getVersion()
  .then((r) => {
    console.log('\x1b[32m%s\x1b[0m', `\nMaking app version file: ${r.semVer2}\n`);
    fs.writeFileSync('../wwwroot/js/version.json', JSON.stringify(r, null, 2), 'utf8');
  })
  .catch(e => console.error(e));

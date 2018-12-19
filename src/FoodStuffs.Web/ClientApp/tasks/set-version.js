const nbgv = require('nerdbank-gitversioning');
const fs = require('fs');

nbgv.getVersion()
  .then((r) => {
    fs.writeFileSync('../wwwroot/js/version.json', JSON.stringify(r, null, 2), 'utf8');
  })
  .catch(e => console.error(e));

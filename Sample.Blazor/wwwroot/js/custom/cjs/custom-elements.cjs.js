'use strict';

const core = require('./core-8909b889.js');

core.patchBrowser().then(options => {
  return core.bootstrapLazy([["custom-elements-shell_2.cjs",[[1,"custom-elements-shell"],[1,"custom-elements-table",{"items":[16]}]]]], options);
});

'use strict';

Object.defineProperty(exports, '__esModule', { value: true });

const core = require('./core-8909b889.js');

const defineCustomElements = (win, options) => {
  return core.patchEsm().then(() => {
    core.bootstrapLazy([["custom-elements-shell_2.cjs",[[1,"custom-elements-shell"],[1,"custom-elements-table",{"items":[16]}]]]], options);
  });
};

exports.defineCustomElements = defineCustomElements;

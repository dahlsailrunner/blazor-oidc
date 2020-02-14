import { a as patchEsm, b as bootstrapLazy } from './core-0a29fab8.js';
var defineCustomElements = function (win, options) {
    return patchEsm().then(function () {
        bootstrapLazy([["custom-elements-shell_2", [[1, "custom-elements-shell"], [1, "custom-elements-table", { "items": [16] }]]]], options);
    });
};
export { defineCustomElements };

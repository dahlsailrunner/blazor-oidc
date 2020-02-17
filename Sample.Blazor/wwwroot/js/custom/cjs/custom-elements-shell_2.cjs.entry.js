'use strict';

Object.defineProperty(exports, '__esModule', { value: true });

const core = require('./core-8909b889.js');

const ElementsShell = class {
    constructor(hostRef) {
        core.registerInstance(this, hostRef);
        this.tableItems = [
            {
                "item": "Home",
                "icon": "fa fa-cog",
                "url": "/"
            },
            {
                "item": "Someplace Else",
                "icon": "fa fa-bang",
                "url": "/elsewhere"
            }
        ];
    }
    render() {
        return (core.h("div", null, core.h("h1", null, "Hello World"), core.h("custom-elements-table", { items: this.tableItems })));
    }
};

const ElementsTable = class {
    constructor(hostRef) {
        core.registerInstance(this, hostRef);
    }
    render() {
        return (core.h("table", { class: "table" }, core.h("thead", null, core.h("tr", null, core.h("th", null, "Title"), core.h("th", null, "Icon"), core.h("th", null, "Url"))), core.h("tbody", null, this.items.map((item) => core.h("tr", null, core.h("td", null, item['title']), core.h("td", null, item['icon']), core.h("td", null, item['url']))))));
    }
    static get style() { return ":host{display:block}"; }
};

exports.custom_elements_shell = ElementsShell;
exports.custom_elements_table = ElementsTable;

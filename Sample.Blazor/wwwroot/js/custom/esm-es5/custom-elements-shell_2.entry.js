import { r as registerInstance, h } from './core-0a29fab8.js';
var ElementsShell = /** @class */ (function () {
    function ElementsShell(hostRef) {
        registerInstance(this, hostRef);
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
    ElementsShell.prototype.render = function () {
        return (h("div", null, h("h1", null, "Hello World"), h("custom-elements-table", { items: this.tableItems })));
    };
    return ElementsShell;
}());
var ElementsTable = /** @class */ (function () {
    function ElementsTable(hostRef) {
        registerInstance(this, hostRef);
    }
    ElementsTable.prototype.render = function () {
        return (h("table", { class: "table" }, h("thead", null, h("tr", null, h("th", null, "Title"), h("th", null, "Icon"), h("th", null, "Url"))), h("tbody", null, this.items.map(function (item) { return h("tr", null, h("td", null, item['item']), h("td", null, item['icon']), h("td", null, item['url'])); }))));
    };
    Object.defineProperty(ElementsTable, "style", {
        get: function () { return ":host{display:block}"; },
        enumerable: true,
        configurable: true
    });
    return ElementsTable;
}());
export { ElementsShell as custom_elements_shell, ElementsTable as custom_elements_table };

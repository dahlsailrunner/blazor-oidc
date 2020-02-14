import { r as registerInstance, h } from './core-0a29fab8.js';

const ElementsShell = class {
    constructor(hostRef) {
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
    render() {
        return (h("div", null, h("h1", null, "Hello World"), h("custom-elements-table", { items: this.tableItems })));
    }
};

const ElementsTable = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
    }
    render() {
        return (h("table", { class: "table" }, h("thead", null, h("tr", null, h("th", null, "Title"), h("th", null, "Icon"), h("th", null, "Url"))), h("tbody", null, this.items.map((item) => h("tr", null, h("td", null, item['item']), h("td", null, item['icon']), h("td", null, item['url']))))));
    }
    static get style() { return ":host{display:block}"; }
};

export { ElementsShell as custom_elements_shell, ElementsTable as custom_elements_table };

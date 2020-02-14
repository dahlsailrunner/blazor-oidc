import { h } from "@stencil/core";
export class ElementsShell {
    constructor() {
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
        return (h("div", null,
            h("h1", null, "Hello World"),
            h("custom-elements-table", { items: this.tableItems })));
    }
    static get is() { return "custom-elements-shell"; }
    static get encapsulation() { return "shadow"; }
}

import { h } from "@stencil/core";
export class ElementsTable {
    render() {
        return (h("table", { class: "table" },
            h("thead", null,
                h("tr", null,
                    h("th", null, "Title"),
                    h("th", null, "Icon"),
                    h("th", null, "Url"))),
            h("tbody", null, this.items.map((item) => h("tr", null,
                h("td", null, item['title']),
                h("td", null, item['icon']),
                h("td", null, item['url']))))));
    }
    static get is() { return "custom-elements-table"; }
    static get encapsulation() { return "shadow"; }
    static get originalStyleUrls() { return {
        "$": ["elements-table.css"]
    }; }
    static get styleUrls() { return {
        "$": ["elements-table.css"]
    }; }
    static get properties() { return {
        "items": {
            "type": "unknown",
            "mutable": false,
            "complexType": {
                "original": "Array<Object>",
                "resolved": "Object[]",
                "references": {
                    "Array": {
                        "location": "global"
                    },
                    "Object": {
                        "location": "global"
                    }
                }
            },
            "required": false,
            "optional": false,
            "docs": {
                "tags": [],
                "text": ""
            }
        }
    }; }
}

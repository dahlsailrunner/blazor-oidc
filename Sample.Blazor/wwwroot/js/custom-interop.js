var jsInterop = jsInterop || {};

jsInterop.setLeftNavItems = function (leftNavElement, navItems) {
    leftNavElement.items = navItems;
};

jsInterop.setLeftNavItemsForInline = function () {
    var inlineTable = document.getElementById('inline-table');
    inlineTable.items = [
        { "title": "Home2", "icon": "fa fa-cog", "url": "/" },
        { "title": "SomeplaceElse2", "icon": "fa fa-bang", "url": "/elsewhere" }
    ];
};
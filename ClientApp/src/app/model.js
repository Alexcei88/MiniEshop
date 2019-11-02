"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Good = /** @class */ (function () {
    function Good(id, name, price, count) {
        this.id = id;
        this.name = name;
        this.price = price;
        this.count = count;
    }
    return Good;
}());
exports.Good = Good;
var Category = /** @class */ (function () {
    function Category(id, name, childs) {
        this.id = id;
        this.name = name;
        this.childs = childs;
    }
    return Category;
}());
exports.Category = Category;
//# sourceMappingURL=model.js.map
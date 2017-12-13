// Recipe class structure
module.exports = function () {
    this.categories = [];
    this.cookTimeMinutes = 0;
    this.directions = "";
    this.ingredients = "";
    this.name = "";
    this.prepTimeMinutes = 0;

    // TODO: remove these from the created view model
    this.createdByUserId = 0;
    this.createdOn = null;
    this.modifiedByUserId = 0;
    this.modifiedOn = null;
    this.id = 0;
};
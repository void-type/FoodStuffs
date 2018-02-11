export default class {
  constructor() {
    this.id = 0;
    this.categories = [];
    this.cookTimeMinutes = null;
    this.directions = "";
    this.ingredients = "";
    this.name = "";
    this.prepTimeMinutes = null;
    this.createdBy = "";
    this.createdOn = new Date();
    this.modifiedBy = "";
    this.modifiedOn = new Date();
  }
}
export default class GetRecipeResponse {
  constructor() {
    this.id = 0;
    this.name = '';
    this.ingredients = '';
    this.directions = '';
    this.cookTimeMinutes = null;
    this.prepTimeMinutes = null;
    this.pinnedImageId = null;
    this.categories = [];
    this.images = [];
    this.createdOn = new Date();
    this.createdBy = '';
    this.modifiedOn = new Date();
    this.modifiedBy = '';
  }
}

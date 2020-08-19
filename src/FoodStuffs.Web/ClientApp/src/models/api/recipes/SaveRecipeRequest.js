export default class SaveRecipeRequest {
  constructor() {
    this.id = 0;
    this.name = '';
    this.ingredients = '';
    this.directions = '';
    this.cookTimeMinutes = 0;
    this.prepTimeMinutes = 0;
    this.categories = [];
  }
}

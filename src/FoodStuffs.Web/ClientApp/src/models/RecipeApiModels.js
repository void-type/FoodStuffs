import ItemSetPage from './ItemSetPage';
import sort from '../util/sort';

class RecipeGetRequest {
  constructor() {
    this.categorySearch = '';
    this.nameSearch = '';
    this.page = 1;
    this.take = 15;
    this.nameSort = sort.types[0].name;
  }
}

class RecipeSaveRequest {
  constructor() {
    this.categories = [];
    this.cookTimeMinutes = null;
    this.createdBy = '';
    this.createdOn = new Date();
    this.directions = '';
    this.id = 0;
    this.ingredients = '';
    this.modifiedBy = '';
    this.modifiedOn = new Date();
    this.name = '';
    this.prepTimeMinutes = null;
  }
}

export default {
  GetRequest: RecipeGetRequest,
  GetResponse: ItemSetPage,
  SaveRequest: RecipeSaveRequest,
};

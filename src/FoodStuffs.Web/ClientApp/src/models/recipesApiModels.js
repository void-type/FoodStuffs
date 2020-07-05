import ItemSet from './itemSet';
import options from '../util/options';

export class GetRecipeResponse {
  constructor() {
    this.id = 0;
    this.name = '';
    this.ingredients = '';
    this.directions = '';
    this.cookTimeMinutes = null;
    this.prepTimeMinutes = null;
    this.categories = [];
    this.images = [];
    this.createdOn = new Date();
    this.createdBy = '';
    this.modifiedOn = new Date();
    this.modifiedBy = '';
  }
}

export class ListRecipesRequest {
  constructor() {
    this.category = '';
    this.name = '';
    this.sortBy = 'name';
    this.sortDesc = false;
    this.isPagingEnabled = true;
    this.page = 1;
    this.take = options.paginationTakeOptions[0].value;
  }
}

export class ListRecipesResponse extends ItemSet { }

export class SaveRecipeRequest {
  constructor() {
    this.id = 0;
    this.name = '';
    this.ingredients = '';
    this.directions = '';
    this.cookTimeMinutes = null;
    this.prepTimeMinutes = null;
    this.categories = [];
  }
}

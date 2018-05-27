import recipeSearchSortTypes from './recipeSearchSortTypes';

export default class {
  constructor() {
    this.nameSearch = '';
    this.categorySearch = '';
    this.sort = recipeSearchSortTypes[0].name;
    this.page = 1;
    this.take = 15;
  }
}

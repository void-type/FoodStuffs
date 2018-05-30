import recipeSearchSortTypes from './recipeSearchSortTypes';

export default class {
  constructor() {
    this.categorySearch = '';
    this.nameSearch = '';
    this.page = 1;
    this.sort = recipeSearchSortTypes[0].name;
    this.take = 15;
  }
}

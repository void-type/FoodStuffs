import ItemSetPage from './ItemSetPage';

const RecipeGetRequestSortTypes = [
  {
    name: 'ascending',
    symbol: '&#9660;',
  },
  {
    name: 'descending',
    symbol: '&#9650;',
  },
  {
    name: 'chronological',
    symbol: '&#x1F552;',
  },
];

class RecipeGetRequest {
  constructor() {
    this.categorySearch = '';
    this.nameSearch = '';
    this.page = 1;
    this.nameSort = RecipeGetRequestSortTypes[0].name;
    this.take = 15;
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
  GetRequestSortTypes: RecipeGetRequestSortTypes,
  GetResponse: ItemSetPage,
  SaveRequest: RecipeSaveRequest,
};

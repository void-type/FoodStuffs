import ItemSetPage from './ItemSetPage';
import sort from '../util/sort';
import options from '../util/options';

class ListRequest {
  constructor() {
    this.page = 1;
    this.take = options.paginationTakeOptions[0].value;
    this.categorySearch = '';
    this.nameSearch = '';
    this.nameSort = sort.types[0].name;
  }
}

class SaveRequest {
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

class GetResponse {
  constructor() {
    this.id = 0;
    this.name = '';
    this.ingredients = '';
    this.directions = '';
    this.cookTimeMinutes = null;
    this.prepTimeMinutes = null;
    this.categories = [];
    this.createdOn = new Date();
    this.createdBy = '';
    this.modifiedOn = new Date();
    this.modifiedBy = '';
  }
}

export default {
  ListRequest,
  ListResponse: ItemSetPage,
  GetResponse,
  SaveRequest,
};

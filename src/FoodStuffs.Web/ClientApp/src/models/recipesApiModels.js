import ItemSet from './itemSet';
import options from '../util/options';

const listSortOptions = {
  types: [
    {
      id: 0,
      name: 'name',
      symbol: '▼',
    },
    {
      id: 1,
      name: 'nameDesc',
      symbol: '▲',
    },
    {
      id: 2,
      name: null,
      symbol: '',
    },
  ],
  getTypeByName(name) {
    return this.types.filter(type => type.name === name)[0] || this.types[0];
  },
  nextSort(name) {
    const nextId = (this.getTypeByName(name).id + 1) % this.types.length;
    return this.types[nextId];
  },
};

class ListRequest {
  constructor() {
    this.categorySearch = '';
    this.nameSearch = '';
    this.sort = listSortOptions.types[0].name;
    this.isPagingEnabled = true;
    this.page = 1;
    this.take = options.paginationTakeOptions[0].value;
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
  listSortOptions,
  ListRequest,
  ListResponse: ItemSet,
  GetResponse,
  SaveRequest,
};

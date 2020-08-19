import { paginationTakeOptions } from '../../options';

export default class ListRecipesRequest {
  constructor() {
    this.category = '';
    this.name = '';
    this.sortBy = '';
    this.sortDesc = false;
    this.isPagingEnabled = true;
    this.page = 1;
    this.take = paginationTakeOptions[0].value;
  }
}

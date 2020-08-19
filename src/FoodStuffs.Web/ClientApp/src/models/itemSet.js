import { paginationTakeOptions } from './options';

export default class ItemSet {
  constructor() {
    this.items = [];
    this.count = 0;
    this.isPagingEnabled = true;
    this.page = 1;
    this.take = paginationTakeOptions[0].value;
    this.totalCount = 0;
  }
}

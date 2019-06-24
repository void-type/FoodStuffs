import defaults from '../util/options';

export default class {
  constructor() {
    this.items = [];
    this.count = 0;
    this.isPagingEnabled = true;
    this.page = 1;
    this.take = defaults.paginationTakeOptions[0].value;
    this.totalCount = 0;
  }
}

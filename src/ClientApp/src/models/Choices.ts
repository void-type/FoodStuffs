export default class Choices {
  static get paginationTake() {
    return [
      {
        text: '12',
        value: 12,
      },
      {
        text: '24',
        value: 24,
      },
      {
        text: '96',
        value: 96,
      },
      {
        text: 'All',
        value: -1,
      },
    ];
  }

  static get defaultPaginationTake() {
    return this.paginationTake[1]!;
  }

  static get boolean() {
    return [
      {
        text: 'All',
        value: null,
      },
      {
        text: 'Yes',
        value: true,
      },
      {
        text: 'No',
        value: false,
      },
    ];
  }

  static get sortOptions() {
    return [
      { text: 'Relevance', value: '' },
      { text: 'Newest', value: 'newest' },
      { text: 'Oldest', value: 'oldest' },
      { text: 'A-Z', value: 'a-z' },
      { text: 'Z-A', value: 'z-a' },
      { text: 'Random', value: 'random' },
    ];
  }
}

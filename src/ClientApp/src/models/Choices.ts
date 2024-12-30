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
    return this.paginationTake[1];
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
}

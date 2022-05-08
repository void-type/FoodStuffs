export default class Choices {
  static get paginationTake() {
    return [
      {
        text: '10',
        value: 10,
      },
      {
        text: '30',
        value: 30,
      },
      {
        text: '100',
        value: 100,
      },
      {
        text: 'All',
        value: -1,
      },
    ];
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

export default {
  types: [
    {
      id: 0,
      name: 'ascending',
      symbol: '&#9660;',
    },
    {
      id: 1,
      name: 'descending',
      symbol: '&#9650;',
    },
    {
      id: 2,
      name: 'chronological',
      symbol: '&#x1F552;',
    },
  ],
  getTypeByName(name) {
    return this.types.filter(type => type.name === name)[0] || this.types[0];
  },
  nextType(name) {
    const nextId = (this.getTypeByName(name).id + 1) % this.types.length;
    return this.types[nextId];
  },
};

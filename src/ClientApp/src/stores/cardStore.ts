import { defineStore } from 'pinia';

export interface CardIngredient {
  name: string;
  quantity: number;
}

export interface Card {
  id: number;
  name: string;
  ingredients: CardIngredient[];
  active: boolean;
}

interface CardStoreState {
  cards: Card[];
  pantry: Record<string, number>;
}

export interface CardQuery {
  active: boolean;
}

function countIngredients(acc: Record<string, number>, curr: CardIngredient) {
  const { name } = curr;

  if (!acc[name]) {
    acc[name] = 0;
  }

  acc[name] += curr.quantity;
  return acc;
}

function addCount(dict: Record<string, number>, key: string, count = 1) {
  if (!dict[key]) {
    // eslint-disable-next-line no-param-reassign
    dict[key] = 0;
  }

  // eslint-disable-next-line no-param-reassign
  dict[key] += count;
}

function subtractCount(dict: Record<string, number>, key: string, count = 1) {
  if (!dict[key]) {
    return;
  }

  // eslint-disable-next-line no-param-reassign
  dict[key] -= count;

  if (dict[key] < 1) {
    // eslint-disable-next-line no-param-reassign
    delete dict[key];
  }
}

const testCards = [
  {
    id: 1,
    name: 'Burgers',
    ingredients: [
      { name: 'Beef', quantity: 2 },
      { name: 'Buns', quantity: 2 },
      { name: 'Ketchup', quantity: 1 },
    ],
    active: false,
  },
  {
    id: 2,
    name: 'Brats',
    ingredients: [
      { name: 'Sausage', quantity: 1 },
      { name: 'Buns', quantity: 1 },
      { name: 'Mustard', quantity: 3 },
    ],
    active: false,
  },
  {
    id: 3,
    name: 'Mac N Cheese',
    ingredients: [
      { name: 'Mac', quantity: 1 },
      { name: 'Cheese', quantity: 1 },
    ],
    active: false,
  },
  {
    id: 4,
    name: 'Curry',
    ingredients: [
      { name: 'Chicken', quantity: 1 },
      { name: 'Curry sauce', quantity: 1 },
      { name: 'Rice', quantity: 1 },
    ],
    active: false,
  },
  {
    id: 5,
    name: 'Meatloaf',
    ingredients: [
      { name: 'Beef', quantity: 3 },
      { name: 'Crackers', quantity: 1 },
      { name: 'Ketchup', quantity: 1 },
      { name: 'Egg', quantity: 1 },
    ],
    active: false,
  },
  {
    id: 6,
    name: 'Sandwich',
    ingredients: [
      { name: 'Lunch meat', quantity: 1 },
      { name: 'Buns', quantity: 2 },
      { name: 'Mustard', quantity: 1 },
      { name: 'Cheese', quantity: 1 },
      { name: 'Lettuce', quantity: 1 },
    ],
    active: false,
  },
];

export const useCardStore = defineStore('cards', {
  state: (): CardStoreState => ({
    cards: testCards,
    pantry: {},
  }),

  getters: {
    getCards: (state) => (query: CardQuery) =>
      state.cards
        .sort((a, b) => a.name.localeCompare(b.name))
        .filter((c) => (query.active !== null ? c.active === query.active : true)),

    getPantry: (state) => Object.entries(state.pantry),

    getShoppingList: (state) => {
      const ingredientCounts = state.cards
        .filter((card) => card.active === true)
        .flatMap((c) => c.ingredients)
        .reduce(countIngredients, {});

      Object.entries(state.pantry).forEach(([ingredient, quantity]) => {
        subtractCount(ingredientCounts, ingredient, quantity);
      });

      return Object.entries(ingredientCounts);
    },
  },

  actions: {
    toggleCard(id: number) {
      const card = this.cards.find((c) => c.id === id);
      if (typeof card === 'undefined') {
        return;
      }
      card.active = !card.active;
    },

    clearPantry() {
      this.pantry = {};
    },

    addToPantry(ingredient: string) {
      addCount(this.pantry, ingredient);
    },

    removeFromPantry(ingredient: string) {
      subtractCount(this.pantry, ingredient);
    },

    clearShoppingList() {
      this.cards.forEach((c) => {
        // eslint-disable-next-line no-param-reassign
        c.active = false;
      });
    },
  },
});

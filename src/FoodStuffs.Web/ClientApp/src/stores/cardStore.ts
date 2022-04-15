import { defineStore } from 'pinia';

export interface Card {
  id: number,
  name: string,
  ingredients: string[]
  active: boolean
}

export interface Pantry {
  [key: string]: number
}

export interface CardStoreState {
  cards: Card[],
  pantry: Pantry
}

export const useCardStore = defineStore('cards', {
  state: (): CardStoreState => {
    return {
      cards: [
        {
          id: 1,
          name: 'Burgers',
          ingredients: ['Beef', 'Buns', 'Ketchup'],
          active: false,
        },
        {
          id: 2,
          name: 'Brats',
          ingredients: ['Sausage', 'Buns', 'Mustard'],
          active: false,
        },
        {
          id: 3,
          name: 'Mac N Cheese',
          ingredients: ['Mac', 'Cheese'],
          active: false,
        },
        {
          id: 4,
          name: 'Curry',
          ingredients: ['Chicken', 'Curry sauce', 'Rice'],
          active: false,
        },
        {
          id: 5,
          name: 'Meatloaf',
          ingredients: ['Beef', 'Crackers', 'Ketchup', 'Egg'],
          active: false,
        },
        {
          id: 6,
          name: 'Sandwich',
          ingredients: ['Lunch meat', 'Buns', 'Mustard', 'Cheese', 'Lettuce'],
          active: false,
        },
      ],
      pantry: {},
    }
  },

  getters: {
    getCards: (state) => state.cards
      .sort((a, b) => a.name.localeCompare(b.name)),

    getPantry: (state) => Object.entries(state.pantry),

    getShoppingList: (state) => {
      const ingredientCounts = state.cards
        .filter(card => card.active == true)
        .flatMap((c: { ingredients: string[]; }) => c.ingredients)
        .sort()
        .reduce(countInstances, {});

      Object.entries(state.pantry)
        .forEach(([ingredient, quantity]) => {
          subtractCount(ingredientCounts, ingredient, quantity);
        });

      return Object.entries(ingredientCounts);
    },
  },

  actions: {
    toggleCard(id: number) {
      const card = this.cards.find(c => c.id === id);
      if (card == undefined) {
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
      this.cards.forEach(c => c.active = false);
    },
  },
});

function countInstances(acc: { [x: string]: number; }, curr: string | number) {
  if (!acc[curr]) {
    acc[curr] = 0;
  }

  acc[curr]++;
  return acc;
}

function addCount(dict: Record<string, number>, key: string | number, count = 1) {
  if (!dict[key]) {
    dict[key] = 0;
  }

  dict[key] += count;
}

function subtractCount(dict: Record<string, number>, key: string | number, count = 1) {
  if (!dict[key]) {
    return;
  }

  dict[key] -= count;

  if (dict[key] < 1) {
    delete (dict[key]);
  }
}

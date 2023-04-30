import type {
  ListRecipesResponse,
  ListRecipesResponseIItemSet,
  ListRecipesResponseIngredient,
  RecipesListParams,
} from '@/api/data-contracts';
import Choices from '@/models/Choices';
import { isNil } from '@/models/FormatHelpers';
import ListRecipesRequest from '@/models/ListRecipesRequest';
import { defineStore } from 'pinia';

interface CardStoreState {
  listResponse: ListRecipesResponseIItemSet;
  listRequest: RecipesListParams;
  selectedCards: ListRecipesResponse[];
  pantry: Record<string, number>;
}

function countIngredients(acc: Record<string, number>, curr: ListRecipesResponseIngredient) {
  const { name, quantity } = curr;

  if (isNil(name)) {
    return acc;
  }

  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  if (!acc[name!]) {
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    acc[name!] = 0;
  }

  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  acc[name!] += quantity || 0;
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

export default defineStore('cards', {
  state: (): CardStoreState => ({
    listResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    listRequest: {
      ...new ListRecipesRequest(),
      isForMealPlanning: true,
    },
    selectedCards: [],
    pantry: {},
  }),

  getters: {
    getPantry: (state) => Object.entries(state.pantry),

    getShoppingList: (state) => {
      const ingredientCounts = state.selectedCards
        .flatMap((c) => c.ingredients || [])
        .filter((c) => !c.isCategory)
        .reduce(countIngredients, {});

      Object.entries(state.pantry).forEach(([ingredient, quantity]) => {
        subtractCount(ingredientCounts, ingredient, quantity);
      });

      return Object.entries(ingredientCounts);
    },

    isCardSelected: (state) => (id: number | null | undefined) =>
      state.selectedCards.findIndex((x) => x.id === id) > -1,
  },

  actions: {
    setListResponse(data: ListRecipesResponseIItemSet) {
      this.listResponse = data;
    },

    setListRequest(data: RecipesListParams) {
      this.listRequest = data;
    },

    toggleCard(id: number) {
      const cardIndex = this.selectedCards.findIndex((x) => x.id === id);

      if (cardIndex > -1) {
        this.selectedCards.splice(cardIndex, 1);
        return;
      }

      const recipe = this.listResponse.items?.find((c) => c.id === id);

      if (typeof recipe === 'undefined' || recipe === null) {
        return;
      }

      this.selectedCards.push(recipe);
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
      this.selectedCards = [];
    },
  },
});

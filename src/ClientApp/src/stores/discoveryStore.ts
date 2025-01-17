import { defineStore } from 'pinia';
import type { SearchRecipesResultItem, SearchRecipesResponse } from '@/api/data-contracts';
import RecipesListRequest from '@/models/RecipesListRequest';
import ApiHelper from '@/models/ApiHelper';
import type { HttpResponse } from '@/api/http-client';
import useMessageStore from './messageStore';

interface DiscoveryStoreState {
  list: SearchRecipesResultItem[];
  page: number;
  take: number;
  isFetchingRecipes: boolean;
  randomSortSeed: string;
}

const api = ApiHelper.client;

export default defineStore('discovery', {
  state: (): DiscoveryStoreState => ({
    list: [],
    page: 0,
    take: 12,
    isFetchingRecipes: false,
    randomSortSeed: crypto.randomUUID(),
  }),

  actions: {
    setList(data: SearchRecipesResponse) {
      const { results } = data;

      if (!results?.items) {
        this.page = 0;
        this.list = [];
        return;
      }

      if (results.page === 1) {
        this.list = [];
      }

      this.page = results.page || 0;

      // If it wasn't a full page, make the next request load this page again.
      if ((results.count || 0) < (results.take || 1) && this.page > 1) {
        this.page -= 1;
      }

      const newItems = results.items.filter(
        (newItem) => !this.list.some((existingItem) => existingItem.id === newItem.id)
      );

      if (newItems.length > 0) {
        this.list = [...this.list, ...newItems];
      }
    },

    removeFromList(id: number) {
      const list = this.list.slice();

      const indexOfCurrentInRecents = list.findIndex((recentRecipe) => recentRecipe.id === id);

      if (indexOfCurrentInRecents > -1) {
        list.splice(indexOfCurrentInRecents, 1);
      }

      this.list = list;
    },

    async fetchNext() {
      if (this.isFetchingRecipes) {
        return;
      }

      this.isFetchingRecipes = true;

      try {
        const response = await api().recipesSearch({
          ...new RecipesListRequest(),
          page: this.page + 1,
          take: 12,
          sortBy: 'random',
          randomSortSeed: this.randomSortSeed,
        });

        if ((response.data?.results?.count || 0) > 0) {
          this.setList(response.data);
        }
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      } finally {
        this.isFetchingRecipes = false;
      }
    },
  },
});

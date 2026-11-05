import type {
  GetMealPlanResponse,
  GetMealPlanResponseExcludedGroceryItem,
  GetMealPlanResponseRecipe,
  IItemSetOfListMealPlansResponse,
  MealPlansListParams,
} from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import { defineStore } from 'pinia';
import ApiHelper from '@/models/ApiHelper';
import Choices from '@/models/Choices';
import { isNil } from '@/models/FormatHelper';
import ListMealPlansRequest from '@/models/MealPlansListRequest';
import {
  addGroceryItem,
  countGroceryItems,
  listRequestToQueryParams,
  storeCurrentMealPlanInStorage,
  subtractGroceryItem,
} from '@/models/MealPlanStoreHelper';
import MealPlanWorking from '@/models/MealPlanWorking';
import useMessageStore from './messageStore';

interface MealPlanStoreState {
  listResponse: IItemSetOfListMealPlansResponse;
  listRequest: MealPlansListParams;
  currentMealPlan: GetMealPlanResponse;
  editingMealPlan: GetMealPlanResponse;
}

const api = ApiHelper.client;

export default defineStore('mealPlan', {
  state: (): MealPlanStoreState => ({
    listResponse: {
      count: 0,
      items: [],
      isPagingEnabled: true,
      page: 1,
      take: Choices.defaultPaginationTake.value,
      totalCount: 0,
    },
    listRequest: {
      ...new ListMealPlansRequest(),
    },
    currentMealPlan: MealPlanWorking.createForStore(),
    editingMealPlan: MealPlanWorking.createForStore(),
  }),

  getters: {
    currentRecipes: state => state.currentMealPlan.recipes || [],

    currentPantry: state => state.currentMealPlan.excludedGroceryItems || [],

    currentRecipesContains: state => (recipeId: number | null | undefined) => {
      if (isNil(recipeId)) {
        return false;
      }

      return (state.currentMealPlan.recipes || []).map(x => x.id).includes(recipeId!);
    },

    currentShoppingList(state): GetMealPlanResponseExcludedGroceryItem[] {
      const groceryItemCounts = this.currentRecipes
        .flatMap(c => c.groceryItems || [])
        .reduce(countGroceryItems, []);

      (state.currentMealPlan.excludedGroceryItems || []).forEach((x) => {
        if (!x.id) {
          return;
        }

        subtractGroceryItem(groceryItemCounts, x.id, x.quantity);
      });

      return groceryItemCounts;
    },

    currentQueryParams(state) {
      const { listRequest } = state;

      return listRequestToQueryParams(listRequest);
    },
  },

  actions: {
    async addToCurrentPantry(groceryItemId: number | undefined, count = 1) {
      if (this.currentMealPlan === null || isNil(groceryItemId)) {
        return;
      }

      addGroceryItem(this.currentMealPlan.excludedGroceryItems || [], groceryItemId, count);
      await this.saveCurrentMealPlan([], true, true);
    },

    async removeFromCurrentPantry(groceryItemId: number | undefined, count = 1) {
      if (this.currentMealPlan === null || isNil(groceryItemId)) {
        return;
      }

      subtractGroceryItem(this.currentMealPlan.excludedGroceryItems || [], groceryItemId, count);
      await this.saveCurrentMealPlan([], true, true);
    },

    async clearCurrentPantry() {
      if (this.currentMealPlan === null) {
        return;
      }

      this.currentMealPlan.excludedGroceryItems = [];
      await this.saveCurrentMealPlan([], true, true);
    },

    async addCurrentRecipe(recipeId: number | null | undefined) {
      if (this.currentMealPlan === null) {
        return;
      }

      if (isNil(recipeId)) {
        return;
      }

      if (!this.currentMealPlan.id) {
        await this.saveCurrentMealPlan([recipeId]);
        return;
      }

      try {
        const response = await api().mealPlansAddRecipe({ id: this.currentMealPlan.id, recipeId });

        if (response.data.entity) {
          this.currentMealPlan = response.data.entity;
        }

        if (response.data.message) {
          useMessageStore().setSuccessMessage(response.data.message);
        }
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },

    async removeCurrentRecipe(recipeId: number | null | undefined) {
      if (this.currentMealPlan === null) {
        return;
      }

      if (isNil(recipeId)) {
        return;
      }

      if (!this.currentMealPlan.id) {
        return;
      }

      const mealPlanId = this.currentMealPlan.id;
      const index = (this.currentMealPlan.recipes ?? []).findIndex(r => r.id === recipeId);
      const removed = index >= 0
        ? { ...this.currentMealPlan.recipes![index] }
        : null;

      try {
        const response = await api().mealPlansRemoveRecipe({ id: mealPlanId, recipeId });

        if (response.data.entity) {
          this.currentMealPlan = response.data.entity;
        }

        if (removed) {
          // Replaces the server's message so the only toast is the one with Undo.
          useMessageStore().setSuccessMessage(`Removed ${removed.name} from the current meal plan.`, {
            autoClear: 10,
            action: {
              label: 'Undo',
              handler: () => this.undoRemoveCurrentRecipe(mealPlanId, removed, index),
            },
          });
        } else if (response.data.message) {
          useMessageStore().setSuccessMessage(response.data.message);
        }
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },

    async undoRemoveCurrentRecipe(mealPlanId: number, recipe: GetMealPlanResponseRecipe, index: number) {
      if (this.currentMealPlan.id !== mealPlanId) {
        useMessageStore().setErrorMessage('Can\'t undo because the current meal plan has changed.');
        return;
      }

      const recipes = this.currentMealPlan.recipes ?? [];

      if (recipes.some(r => r.id === recipe.id)) {
        return;
      }

      // Restores the original position and completion state, which adding by id wouldn't.
      recipes.splice(Math.min(index, recipes.length), 0, recipe);
      recipes.forEach((r, i) => {
        r.order = i + 1;
      });
      this.currentMealPlan.recipes = recipes;

      await this.saveCurrentMealPlan();
    },

    async clearCurrentRecipes() {
      if (this.currentMealPlan === null) {
        return;
      }

      this.currentMealPlan.recipes = [];
      await this.saveCurrentMealPlan();
    },

    async newCurrentMealPlan() {
      this.currentMealPlan = MealPlanWorking.createForStore();
      storeCurrentMealPlanInStorage(null);
    },

    /**
     * Reloads the current meal plan so it reflects changes made elsewhere, like a recipe deleted or restored in
     * another tab. Other failures are ignored so a network blip doesn't clear the meal plan.
     */
    async refreshCurrentMealPlan() {
      const id = this.currentMealPlan.id;

      if (!id) {
        return;
      }

      try {
        const response = await api().mealPlansGet({ id });
        this.currentMealPlan = response.data;
      } catch (error) {
        // The meal plan was deleted elsewhere.
        if ((error as HttpResponse<unknown, unknown>).status === 400) {
          this.newCurrentMealPlan();
        }
      }
    },

    async setCurrentMealPlan(id: number | null | undefined) {
      if (isNil(id)) {
        return;
      }

      try {
        const response = await api().mealPlansGet({ id });
        this.currentMealPlan = response.data;
        if (response.data.id) {
          storeCurrentMealPlanInStorage(response.data.id);
        }
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
        this.newCurrentMealPlan();
      }
    },

    async saveCurrentMealPlan(additionalRecipeIds: number[] = [], quickSave: boolean = false, pantryOnly: boolean = false, silent: boolean = false): Promise<boolean> {
      const current = this.currentMealPlan;

      if (current === null) {
        return false;
      }

      const request = { ...current };

      if (additionalRecipeIds && additionalRecipeIds.length > 0) {
        request.recipes = request.recipes ?? [];

        additionalRecipeIds.forEach((additionalId) => {
          const highestOrder = Math.max(...request.recipes!.map(x => x.order || 0), 0);

          request.recipes!.push({
            id: additionalId,
            order: highestOrder + 1,
          });
        });
      }

      if (pantryOnly) {
        request.recipes = undefined;
      }

      try {
        const response = await api().mealPlansSave(request);

        if (response.data.message && !silent) {
          useMessageStore().setSuccessMessage(response.data.message);
        }

        // Quick save can be used for rapid changes that don't need refreshed data returned like updating pantry groceryItems.
        if (!quickSave) {
          await this.setCurrentMealPlan(response.data.id);
          await this.fetchMealPlanList();
        }

        return true;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
        return false;
      }
    },

    async deleteMealPlan(id: number | null | undefined) {
      if (!id) {
        return;
      }

      try {
        const response = await api().mealPlansDelete({ id });

        if (response.data.message) {
          useMessageStore().setSuccessMessage(response.data.message);
        }

        await this.fetchMealPlanList();

        if (this.currentMealPlan.id === id) {
          this.newCurrentMealPlan();
        }
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },

    async fetchMealPlanList() {
      try {
        const response = await api().mealPlansList(this.listRequest);
        this.listResponse = response.data;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
      }
    },

    async fetchMealPlan(id: number | null | undefined) {
      if (isNil(id)) {
        this.editingMealPlan = MealPlanWorking.createForStore();
        return;
      }

      try {
        const response = await api().mealPlansGet({ id });
        this.editingMealPlan = response.data;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
        this.editingMealPlan = MealPlanWorking.createForStore();
      }
    },

    async saveMealPlan(
      mealPlan: GetMealPlanResponse,
      additionalRecipeIds: number[] = [],
      silent: boolean = false,
    ): Promise<number | null> {
      if (mealPlan === null) {
        return null;
      }

      const request = mealPlan;

      if (additionalRecipeIds && additionalRecipeIds.length > 0) {
        request.recipes = request.recipes ?? [];

        additionalRecipeIds.forEach((additionalId) => {
          const highestOrder = Math.max(...request.recipes!.map(x => x.order || 0), 0);

          request.recipes!.push({
            id: additionalId,
            order: highestOrder + 1,
          });
        });
      }

      try {
        const response = await api().mealPlansSave(request);

        // Silent saves let the caller show its own message, like one with an Undo action.
        if (response.data.message && !silent) {
          useMessageStore().setSuccessMessage(response.data.message);
        }

        // Refresh the editing meal plan with the saved data
        await this.fetchMealPlan(response.data.id);
        await this.fetchMealPlanList();

        return response.data.id || null;
      } catch (error) {
        useMessageStore().setApiFailureMessages(error as HttpResponse<unknown, unknown>);
        return null;
      }
    },
  },
});

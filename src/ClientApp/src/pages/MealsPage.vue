<script lang="ts" setup>
import { computed, onMounted, ref, watch } from 'vue';
import { storeToRefs } from 'pinia';
import useMealStore from '@/stores/mealStore';
import { clamp, isNil, toInt } from '@/models/FormatHelpers';
import ApiHelpers from '@/models/ApiHelpers';
import SearchRecipesRequest from '@/models/SearchRecipesRequest';
import MealsIngredientList from '@/components/MealsIngredientList.vue';
import MealsCard from '@/components/MealsCard.vue';
import EntityTableControls from '@/components/EntityTableControls.vue';
import EntityTablePager from '@/components/EntityTablePager.vue';
import useMessageStore from '@/stores/messageStore';
import type { SaveMealSetRequest } from '@/api/data-contracts';
import RecipeStoreHelpers from '@/models/RecipeStoreHelpers';
import type { HttpResponse } from '@/api/http-client';
import type { ModalParameters } from '@/models/ModalParameters';
import useAppStore from '@/stores/appStore';
import RecipeSearchCategoriesFilter from '@/components/RecipeSearchCategoriesFilter.vue';
import Choices from '@/models/Choices';

const appStore = useAppStore();
const mealStore = useMealStore();
const messageStore = useMessageStore();
const api = ApiHelpers.client;

const { recipeListRequest, recipeListResponse, mealSetListRequest, currentMealSet } =
  storeToRefs(mealStore);
const availableRecipes = computed(() => mealStore.recipeListResponse.items);
const selectedRecipes = computed(() => mealStore.getSelectedRecipes);
const { sortOptions } = RecipeStoreHelpers;

const selectedCategories = ref([] as Array<number>);

function fetchRecipeList() {
  api()
    .recipesList({
      ...recipeListRequest.value,
    })
    .then((response) => mealStore.setListResponse(response.data))
    .catch((response) => messageStore.setApiFailureMessages(response));
}

function clearRecipeSearch() {
  mealStore.recipeListRequest = {
    ...new SearchRecipesRequest(),
    take: recipeListRequest.value.take,
    isPagingEnabled: recipeListRequest.value.isPagingEnabled,
    isForMealPlanning: true,
  };

  // This should trigger a fetch.
  selectedCategories.value = [];
}

function startRecipeSearch() {
  mealStore.recipeListRequest = {
    ...recipeListRequest.value,
    page: 1,
  };

  fetchRecipeList();
}

function changeRecipePage(page: number) {
  mealStore.recipeListRequest = { ...recipeListRequest.value, page };
  fetchRecipeList();
}

function changeRecipeTake(take: number) {
  mealStore.recipeListRequest = {
    ...mealStore.recipeListRequest,
    isPagingEnabled: toInt(take) > 1,
    take,
    page: 1,
  };

  fetchRecipeList();
}

async function changeMealSetIndex(suggestedIndex: number) {
  const sets = mealStore.mealSetListResponse.items || [];

  if (sets.length < 1) {
    mealStore.newMealSet();
    return;
  }

  const newIndex = clamp(suggestedIndex, 0, sets.length - 1);
  const mealSet = sets[newIndex];

  try {
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    const response = await api().mealSetsDetail(mealSet.id!);
    mealStore.currentMealSet = response.data;
    mealStore.mealSetListIndex = newIndex;
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

async function changeMealSetIndexBack() {
  await changeMealSetIndex(mealStore.mealSetListIndex + 1);
}

async function changeMealSetIndexForward() {
  await changeMealSetIndex(mealStore.mealSetListIndex - 1);
}

async function changeMealSetId(id: number) {
  const sets = mealStore.mealSetListResponse.items || [];
  const index = sets.findIndex((x) => x.id === id);
  await changeMealSetIndex(index);
}

async function fetchMealSetList() {
  try {
    const response = await api().mealSetsList(mealSetListRequest.value);
    mealStore.mealSetListResponse = response.data;
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

async function saveMealSet() {
  const current = currentMealSet.value;

  const request: SaveMealSetRequest = {
    id: current.id,
    name: current.name,
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    recipeIds: selectedRecipes.value.map((x) => x.id!).filter((x) => !isNil(x)),
    pantryIngredients: current.pantryIngredients,
  };

  try {
    const response = await api().mealSetsCreate(request);

    if (response.data.message) {
      messageStore.setSuccessMessage(response.data.message);
    }

    await fetchMealSetList();

    if (response.data.id) {
      await changeMealSetId(response.data.id);
    }
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

async function onDeleteMealSet() {
  async function deleteMealSet() {
    const { id } = currentMealSet.value;

    if (!id) {
      return;
    }

    try {
      const response = await api().mealSetsDelete(id);

      if (response.data.message) {
        messageStore.setSuccessMessage(response.data.message);
      }

      await fetchMealSetList();
      await changeMealSetIndex(mealStore.mealSetListIndex - 1);
    } catch (error) {
      messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    }
  }

  const parameters: ModalParameters = {
    title: 'Delete meal set',
    description: 'Do you really want to delete this meal set?',
    okAction: deleteMealSet,
  };

  appStore.showModal(parameters);
}

async function changeRecipeSort(event: Event) {
  const { value } = event.target as HTMLSelectElement;
  recipeListRequest.value.sortBy = value;
  fetchRecipeList();
}

onMounted(async () => {
  fetchRecipeList();
  await fetchMealSetList();
  await changeMealSetIndex(0);
});

watch(selectedCategories, () => {
  recipeListRequest.value.categories = selectedCategories.value;
  fetchRecipeList();
});
</script>

<template>
  <div class="container-xxl">
    <h1 class="mt-4">Meals</h1>
    <div class="grid">
      <div class="g-col-12">
        <div class="grid">
          <div class="g-col-12 g-col-lg-6">
            <h2 class="mt-4 mb-3">Search recipes</h2>
            <EntityTableControls :clear-search="clearRecipeSearch" :init-search="startRecipeSearch">
              <template #searchForm>
                <div class="grid mb-3 gap-sm">
                  <div class="g-col-12 g-col-lg-6">
                    <label for="nameSearch" class="form-label">Name</label>
                    <input
                      id="nameSearch"
                      v-model="recipeListRequest.name"
                      class="form-control"
                      @keydown.stop.prevent.enter="startRecipeSearch"
                    />
                  </div>
                  <div class="g-col-6 g-col-lg-3">
                    <label class="form-label" for="isForMealPlanning">Meals</label>
                    <select
                      id="isForMealPlanning"
                      v-model="recipeListRequest.isForMealPlanning"
                      class="form-select"
                      @change="startRecipeSearch"
                    >
                      <option
                        v-for="option in Choices.boolean"
                        :key="option.value?.toString()"
                        :value="option.value"
                      >
                        {{ option.text }}
                      </option>
                    </select>
                  </div>
                  <div class="g-col-6 g-col-lg-3">
                    <label for="recipeSort" class="form-label">Sort</label>
                    <select
                      id="recipeSort"
                      :value="recipeListRequest.sortBy"
                      name="recipeSort"
                      class="form-select"
                      aria-label="Page size"
                      @change="changeRecipeSort"
                    >
                      <option
                        v-for="sortOption in sortOptions"
                        :key="sortOption.value"
                        :value="sortOption.value"
                      >
                        {{ sortOption.text }}
                      </option>
                    </select>
                  </div>
                  <RecipeSearchCategoriesFilter v-model="selectedCategories" class="g-col-12" />
                </div>
              </template>
            </EntityTableControls>
            <div class="grid mt-4">
              <div v-if="(availableRecipes?.length || 0) < 1" class="g-col-12 p-4 text-center">
                No results
              </div>
              <MealsCard
                v-for="recipe in availableRecipes"
                :key="recipe.id"
                :recipe="recipe"
                :on-card-click="mealStore.toggleRecipe"
                :selected="mealStore.isRecipeSelected(recipe.id)"
                card-type="inactive"
                class="g-col-12"
              />
            </div>
            <EntityTablePager
              :list-request="recipeListRequest"
              :total-count="toInt(recipeListResponse.totalCount)"
              :on-change-page="changeRecipePage"
              :on-change-take="changeRecipeTake"
              class="mt-4"
            />
          </div>
          <div class="g-col-12 g-col-lg-6">
            <h2 class="mt-4 mb-3">{{ (currentMealSet.id || 0) < 1 ? 'New' : 'Edit' }} plan</h2>
            <label for="nameSearch" class="form-label">Meal set name</label>
            <input
              id="mealSetName"
              v-model="currentMealSet.name"
              class="form-control"
              @keydown.stop.prevent.enter="saveMealSet"
            />
            <div class="btn-toolbar mt-3">
              <button
                class="btn btn-outline-light me-2"
                @click.stop.prevent="changeMealSetIndexBack"
              >
                Back
              </button>
              <button
                class="btn btn-outline-light me-2"
                @click.stop.prevent="changeMealSetIndexForward"
              >
                Forward
              </button>
              <button
                class="btn btn-outline-light ms-auto"
                @click.stop.prevent="mealStore.newMealSet"
              >
                New
              </button>
            </div>
            <div class="btn-toolbar mt-3">
              <button class="btn btn-primary me-2" @click.stop.prevent="saveMealSet">Save</button>
              <button
                class="btn btn-outline-light me-2"
                @click.stop.prevent="mealStore.clearSelectedRecipes"
              >
                Empty
              </button>
              <button
                class="btn btn-danger ms-auto"
                :disabled="(currentMealSet.id || 0) < 1"
                @click.stop.prevent="onDeleteMealSet"
              >
                Delete
              </button>
            </div>
            <div class="grid mt-4">
              <div v-if="(selectedRecipes?.length || 0) < 1" class="g-col-12 p-4 text-center">
                None selected
              </div>
              <MealsCard
                v-for="recipe in selectedRecipes"
                :key="recipe.id"
                :recipe="recipe"
                :on-card-click="mealStore.toggleRecipe"
                card-type="active"
                class="g-col-12"
              />
            </div>
          </div>
        </div>
      </div>
      <div class="g-col-12 mt-5">
        <div class="grid">
          <div class="g-col-12 g-col-md-6">
            <MealsIngredientList
              class="mt-4"
              title="Shopping list"
              :ingredients="mealStore.getShoppingList"
              :on-ingredient-click="mealStore.addToPantry"
              :show-copy-list="true"
            />
          </div>
          <div class="g-col-12 g-col-md-6">
            <MealsIngredientList
              class="mt-4"
              title="Pantry"
              :ingredients="mealStore.getPantry"
              :on-clear="mealStore.clearPantry"
              :on-ingredient-click="mealStore.removeFromPantry"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>

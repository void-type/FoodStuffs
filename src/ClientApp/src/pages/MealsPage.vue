<script lang="ts" setup>
import { computed, onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import useMealStore from '@/stores/mealStore';
import { clamp, isNil, toInt } from '@/models/FormatHelpers';
import ApiHelpers from '@/models/ApiHelpers';
import ListRecipesRequest from '@/models/ListRecipesRequest';
import MealsIngredientList from '@/components/MealsIngredientList.vue';
import MealsCard from '@/components/MealsCard.vue';
import EntityTableControls from '@/components/EntityTableControls.vue';
import EntityTablePager from '@/components/EntityTablePager.vue';
import useMessageStore from '@/stores/messageStore';
import type { SaveMealSetRequest } from '@/api/data-contracts';
import RecipeStoreHelpers from '@/models/RecipeStoreHelpers';

const mealStore = useMealStore();
const messageStore = useMessageStore();
const api = ApiHelpers.client;

const { recipeListRequest, mealSetListRequest, currentMealSet } = storeToRefs(mealStore);
const availableRecipes = computed(() => mealStore.recipeListResponse.items);
const mealSets = computed(() => mealStore.mealSetListResponse.items || []);
const selectedRecipes = computed(() => mealStore.getSelectedRecipes);
const { sortOptions } = RecipeStoreHelpers;

function fetchRecipeList() {
  api()
    .recipesList({
      ...recipeListRequest.value,
    })
    .then((response) => {
      mealStore.recipeListResponse = response.data;
    })
    .catch((response) => messageStore.setApiFailureMessages(response));
}

function clearRecipeSearch() {
  mealStore.recipeListRequest = {
    ...new ListRecipesRequest(),
    take: recipeListRequest.value.take,
    isPagingEnabled: recipeListRequest.value.isPagingEnabled,
    isForMealPlanning: true,
  };

  fetchRecipeList();
}

function startRecipeSearch() {
  mealStore.recipeListRequest = {
    ...recipeListRequest.value,
    page: 1,
  };

  fetchRecipeList();
}

function changeRecipePage(page: number) {
  mealStore.recipeListRequest = { ...mealStore.recipeListRequest, page };

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

function changeMealSetIndex(suggestedIndex: number) {
  if (mealSets.value.length < 1) {
    mealStore.newMealSet();
    return;
  }

  const newIndex = clamp(suggestedIndex, 0, mealSets.value.length - 1);
  const mealSet = mealSets.value[newIndex];

  ApiHelpers.client()
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    .mealSetsDetail(mealSet.id!)
    .then((response) => {
      mealStore.currentMealSet = response.data;
      mealStore.mealSetListIndex = newIndex;
    })
    .catch((response) => messageStore.setApiFailureMessages(response));
}

function changeMealSetIndexBack() {
  changeMealSetIndex(mealStore.mealSetListIndex + 1);
}

function changeMealSetIndexForward() {
  changeMealSetIndex(mealStore.mealSetListIndex - 1);
}

function fetchMealSetList() {
  api()
    .mealSetsList(mealSetListRequest.value)
    .then((response) => {
      mealStore.mealSetListResponse = response.data;
      changeMealSetIndex(0);
    })
    .catch((response) => messageStore.setApiFailureMessages(response));
}

function saveMealSet() {
  const current = currentMealSet.value;

  const request: SaveMealSetRequest = {
    id: current.id,
    name: current.name,
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    recipeIds: selectedRecipes.value.map((x) => x.id!).filter((x) => !isNil(x)),
  };

  api()
    .mealSetsCreate(request)
    .then((response) => {
      if (response.data.message) {
        messageStore.setSuccessMessage(response.data.message);
      }

      fetchMealSetList();
    })
    .catch((response) => {
      messageStore.setApiFailureMessages(response);
    });
}

function deleteMealSet() {
  const current = currentMealSet.value;

  if (isNil(current.id)) {
    return;
  }

  api()
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    .mealSetsDelete(current.id!)
    .then((response) => {
      if (response.data.message) {
        messageStore.setSuccessMessage(response.data.message);
      }

      fetchMealSetList();
    })
    .catch((response) => {
      messageStore.setApiFailureMessages(response);
    });
}

function changeRecipeSort(event: Event) {
  const { value } = event.target as HTMLSelectElement;
  recipeListRequest.value.sortBy = value;
  fetchRecipeList();
}

onMounted(() => {
  fetchRecipeList();
  fetchMealSetList();
  changeMealSetIndex(0);
});
</script>

<template>
  <div class="container-xxl">
    <h1 class="mt-4 mb-4">Meals</h1>
    <div class="grid">
      <div class="g-col-12 g-col-lg-4 d-print-none">
        <h2>Recipes</h2>
        <EntityTableControls
          class="mt-3"
          :clear-search="clearRecipeSearch"
          :init-search="startRecipeSearch"
        >
          <template #searchForm>
            <div class="grid mb-3" style="--bs-gap: 1em">
              <div class="g-col-12">
                <label for="nameSearch" class="form-label">Name contains</label>
                <input
                  id="nameSearch"
                  v-model="recipeListRequest.name"
                  class="form-control"
                  @keydown.stop.prevent.enter="startRecipeSearch"
                />
              </div>
              <div class="g-col-12">
                <label for="categorySearch" class="form-label">Categories contain</label>
                <input
                  id="categorySearch"
                  v-model="recipeListRequest.category"
                  class="form-control"
                  @keydown.stop.prevent.enter="startRecipeSearch"
                />
              </div>
              <div class="g-col-12 g-col-xs-6 g-col-sm-4 g-col-md-3 g-col-lg-12">
                <label for="recipeSort" class="form-label">Sort</label>
                <select
                  id="recipeSort"
                  name="recipeSort"
                  class="form-select"
                  aria-label="Page size"
                  @change="changeRecipeSort"
                >
                  <option
                    v-for="sortOption in sortOptions"
                    :key="String(sortOption.value)"
                    :value="String(sortOption.value)"
                  >
                    {{ sortOption.text }}
                  </option>
                </select>
              </div>
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
          :list-request="mealStore.recipeListRequest"
          :total-count="toInt(mealStore.recipeListResponse.totalCount)"
          :on-change-page="changeRecipePage"
          :on-change-take="changeRecipeTake"
          class="mt-4"
        />
      </div>
      <div class="g-col-12 g-col-lg-8">
        <h2>{{ (currentMealSet.id || 0) < 1 ? 'New' : 'Edit' }} plan</h2>
        <div class="grid">
          <div class="g-col-12 g-col-lg-6">
            <label for="nameSearch" class="form-label">Meal set name</label>
            <input
              id="mealSetName"
              v-model="currentMealSet.name"
              class="form-control"
              @keydown.stop.prevent.enter="saveMealSet"
            />
            <div class="btn-toolbar mt-3">
              <button class="btn btn-secondary me-2" @click.stop.prevent="changeMealSetIndexBack">
                Back
              </button>
              <button
                class="btn btn-secondary me-2"
                @click.stop.prevent="changeMealSetIndexForward"
              >
                Forward
              </button>
              <button class="btn btn-secondary ms-auto" @click.stop.prevent="mealStore.newMealSet">
                New
              </button>
            </div>
            <div class="btn-toolbar mt-3">
              <button class="btn btn-primary me-2" @click.stop.prevent="saveMealSet">Save</button>
              <button
                class="btn btn-secondary me-2"
                @click.stop.prevent="mealStore.clearSelectedRecipes"
              >
                Empty
              </button>
              <button
                class="btn btn-danger ms-auto"
                :disabled="(currentMealSet.id || 0) < 1"
                @click.stop.prevent="deleteMealSet"
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
          <div class="g-col-12 g-col-lg-6">
            <MealsIngredientList
              class="mt-4"
              title="Shopping list"
              :ingredients="mealStore.getShoppingList"
              :on-ingredient-click="mealStore.addToPantry"
              :show-copy-list="true"
            />
            <MealsIngredientList
              class="mt-4 mb-4"
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

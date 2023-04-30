<script lang="ts" setup>
import { computed, onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import useCardStore from '@/stores/cardStore';
import { toInt } from '@/models/FormatHelpers';
import ApiHelpers from '@/models/ApiHelpers';
import ListRecipesRequest from '@/models/ListRecipesRequest';
import useAppStore from '@/stores/appStore';
import MealsIngredientList from '@/components/MealsIngredientList.vue';
import MealsCard from '@/components/MealsCard.vue';
import EntityTableControls from '@/components/EntityTableControls.vue';
import EntityTablePager from '@/components/EntityTablePager.vue';

const appStore = useAppStore();
const cardStore = useCardStore();
const api = ApiHelpers.client;

const { listRequest } = storeToRefs(cardStore);
const activeCards = computed(() => cardStore.selectedCards);
const inactiveCards = computed(() => cardStore.listResponse.items);

function fetchList() {
  api()
    .recipesList({
      ...listRequest.value,
    })
    .then((response) => cardStore.setListResponse(response.data))
    .catch((response) => appStore.setApiFailureMessages(response));
}

function clearSearch() {
  cardStore.setListRequest({
    ...new ListRecipesRequest(),
    take: listRequest.value.take,
    isPagingEnabled: listRequest.value.isPagingEnabled,
    isForMealPlanning: true,
  });

  fetchList();
}

function startSearch() {
  cardStore.setListRequest({
    ...listRequest.value,
    page: 1,
  });

  fetchList();
}

function changePage(page: number) {
  cardStore.setListRequest({ ...cardStore.listRequest, page });

  fetchList();
}

function changeTake(take: number) {
  cardStore.setListRequest({
    ...cardStore.listRequest,
    isPagingEnabled: toInt(take) > 1,
    take,
    page: 1,
  });

  fetchList();
}

onMounted(() => {
  fetchList();
});
</script>

<template>
  <div class="container-xxl">
    <h1 class="mt-4 mb-4">Meals</h1>
    <div class="grid">
      <div class="g-col-12 g-col-lg-6">
        <h2>Available</h2>
        <EntityTableControls class="mt-3" :clear-search="clearSearch" :init-search="startSearch">
          <template #searchForm>
            <div class="grid mb-3" style="--bs-gap: 1em">
              <div class="g-col-12 g-col-md-6">
                <label for="nameSearch" class="form-label">Name contains</label>
                <input
                  id="nameSearch"
                  v-model="listRequest.name"
                  class="form-control"
                  @keydown.stop.prevent.enter="startSearch"
                />
              </div>
              <div class="g-col-12 g-col-md-6">
                <label for="categorySearch" class="form-label">Categories contain</label>
                <input
                  id="categorySearch"
                  v-model="listRequest.category"
                  class="form-control"
                  @keydown.stop.prevent.enter="startSearch"
                />
              </div>
            </div>
          </template>
        </EntityTableControls>
        <div class="grid mt-4">
          <div v-if="(inactiveCards?.length || 0) < 1" class="g-col-12 p-4 text-center">
            No results
          </div>
          <MealsCard
            v-for="card in inactiveCards"
            :key="card.id"
            :card="card"
            :on-card-click="cardStore.toggleCard"
            :selected="cardStore.isCardSelected(card.id)"
            card-type="inactive"
            class="g-col-12 g-col-md-6"
          />
        </div>
        <EntityTablePager
          :list-request="cardStore.listRequest"
          :total-count="toInt(cardStore.listResponse.totalCount)"
          :on-change-page="changePage"
          :on-change-take="changeTake"
          class="mt-4"
        />
      </div>
      <div class="g-col-12 g-col-lg-6">
        <h2>Selected</h2>
        <div class="btn-toolbar mt-3">
          <button class="btn btn-primary me-2" disabled @click.stop.prevent="() => {}">Save</button>
          <button class="btn btn-secondary" @click.stop.prevent="cardStore.clearShoppingList">
            Clear
          </button>
        </div>
        <div class="grid mt-4">
          <div v-if="(activeCards?.length || 0) < 1" class="g-col-12 p-4 text-center">
            None selected
          </div>
          <MealsCard
            v-for="card in activeCards"
            :key="card.id"
            :card="card"
            :on-card-click="cardStore.toggleCard"
            card-type="active"
            class="g-col-12 g-col-md-6"
          />
        </div>
        <MealsIngredientList
          class="mt-4"
          title="Shopping list"
          :ingredients="cardStore.getShoppingList"
          :on-ingredient-click="cardStore.addToPantry"
        />
        <MealsIngredientList
          class="mt-4 mb-4"
          title="Pantry"
          :ingredients="cardStore.getPantry"
          :on-clear="cardStore.clearPantry"
          :on-ingredient-click="cardStore.removeFromPantry"
        />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>

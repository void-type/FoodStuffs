<script lang="ts" setup>
import { computed, onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import useCardStore from '@/stores/cardStore';
import { toInt } from '@/models/FormatHelpers';
import ApiHelpers from '@/models/ApiHelpers';
import ListRecipesRequest from '@/models/ListRecipesRequest';
import useAppStore from '@/stores/appStore';
import MealCardsIngredientList from '@/components/MealCardsIngredientList.vue';
import MealCardsCard from '@/components/MealCardsCard.vue';
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
  <div class="container-fluid">
    <h1 class="mt-4 mb-4">Meal Cards</h1>
    <div class="grid">
      <div class="g-col-12 g-col-lg-3 d-print-none">
        <EntityTableControls :clear-search="clearSearch" :init-search="startSearch">
          <template #searchForm>
            <div class="grid mb-3" style="--bs-gap: 1em">
              <div class="g-col-12">
                <label for="nameSearch" class="form-label">Name contains</label>
                <input
                  id="nameSearch"
                  v-model="listRequest.name"
                  class="form-control"
                  @keydown.stop.prevent.enter="startSearch"
                />
              </div>
              <div class="g-col-12">
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
        <MealCardsIngredientList
          class="mt-5"
          title="Shopping list"
          :ingredients="cardStore.getShoppingList"
          :on-clear="cardStore.clearShoppingList"
          :on-ingredient-click="cardStore.addToPantry"
        />
        <MealCardsIngredientList
          class="mt-5 mb-4"
          title="Pantry"
          :ingredients="cardStore.getPantry"
          :on-clear="cardStore.clearPantry"
          :on-ingredient-click="cardStore.removeFromPantry"
        />
      </div>
      <div class="g-col-12 g-col-lg-9">
        <h2>Selected cards</h2>
        <div class="grid mt-3">
          <MealCardsCard
            v-for="card in activeCards"
            :key="card.id"
            :card="card"
            :on-card-click="cardStore.toggleCard"
            card-type="active"
            class="g-col-12 g-col-md-6 g-col-xxl-4"
          />
        </div>
        <h2 class="mt-5">Available cards</h2>
        <div class="grid mt-3">
          <MealCardsCard
            v-for="card in inactiveCards"
            :key="card.id"
            :card="card"
            :on-card-click="cardStore.toggleCard"
            :selected="cardStore.isCardSelected(card.id)"
            card-type="inactive"
            class="g-col-12 g-col-md-6 g-col-xxl-4"
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
    </div>
  </div>
</template>

<style lang="scss" scoped></style>

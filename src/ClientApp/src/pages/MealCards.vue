<script lang="ts" setup>
import { computed } from 'vue';
import { useCardStore } from '@/stores/cardStore';
import IngredientList from '@/components/MealCardIngredientList.vue';
import Card from '@/components/MealCard.vue';

const cardStore = useCardStore();

const activeCards = computed(() => cardStore.getCards({ active: true }));
const inactiveCards = computed(() => cardStore.getCards({ active: false }));
</script>

<template>
  <div class="container-xxl">
    <h1 class="mt-4 mb-4">Meal Cards</h1>
    <div class="grid mt-4">
      <IngredientList
        class="g-col-12 g-col-md-6"
        title="Shopping list"
        :ingredients="cardStore.getShoppingList"
        :on-clear="cardStore.clearShoppingList"
        :on-ingredient-click="cardStore.addToPantry"
      />
      <IngredientList
        class="g-col-12 g-col-md-6"
        title="Pantry"
        :ingredients="cardStore.getPantry"
        :on-clear="cardStore.clearPantry"
        :on-ingredient-click="cardStore.removeFromPantry"
      />
    </div>
    <div class="grid mt-4">
      <Card
        v-for="card in activeCards"
        :key="card.id"
        :card="card"
        :on-card-click="cardStore.toggleCard"
        :show-ingredients="true"
        class="g-col-12 g-col-md-6 g-col-lg-4"
      />
    </div>
    <div class="grid mt-3">
      <Card
        v-for="card in inactiveCards"
        :key="card.id"
        :card="card"
        :on-card-click="cardStore.toggleCard"
        :show-ingredients="false"
        class="g-col-12 g-col-md-6 g-col-lg-4"
      />
    </div>
  </div>
</template>

<style lang="scss" scoped>
.area {
  display: flex;
  flex-wrap: wrap;
}
</style>

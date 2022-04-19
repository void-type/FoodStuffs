<script setup lang="ts">
import { computed } from 'vue';
import { useCardStore } from '@/stores/cards';
import IngredientList from '@/components/CardIngredientList.vue';
import Card from '@/components/Card.vue';

const cardStore = useCardStore();

const getActiveCards = computed(() => cardStore.getCards.filter((c) => c.active));
const getInactiveCards = computed(() => cardStore.getCards.filter((c) => !c.active));
</script>

<template>
  <div class="area">
    <IngredientList
      title="Shopping list"
      :ingredients="cardStore.getShoppingList"
      :on-clear="cardStore.clearShoppingList"
      :on-ingredient-click="cardStore.addToPantry"
    />
    <IngredientList
      title="Pantry"
      :ingredients="cardStore.getPantry"
      :on-clear="cardStore.clearPantry"
      :on-ingredient-click="cardStore.removeFromPantry"
    />
  </div>

  <div class="area">
    <Card
      v-for="card in getActiveCards"
      :key="card.id"
      :card="card"
      :on-card-click="cardStore.toggleCard"
      :show-ingredients="true"
    />
  </div>

  <div class="area">
    <Card
      v-for="card in getInactiveCards"
      :key="card.id"
      :card="card"
      :on-card-click="cardStore.toggleCard"
      :show-ingredients="false"
    />
  </div>
</template>

<style scoped lang="scss">
.area {
  display: flex;
  flex-wrap: wrap;
  padding: 0.5rem;
}
</style>

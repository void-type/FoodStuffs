<script setup lang="ts">
import { computed } from 'vue';
import IngredientList from '@/components/IngredientList.vue';
import Card from '@/components/Card.vue';
import { useCardStore } from '@/stores/cardStore';

const cardStore = useCardStore();

const getActiveCards = computed(() => cardStore.getCards
  .filter(c => c.active));

const getInactiveCards = computed(() => cardStore.getCards
  .filter(c => !c.active));

</script>

<template>
  <div class="area">
    <IngredientList
                    title="Shopping list"
                    :ingredients="cardStore.getShoppingList"
                    :onClear="cardStore.clearShoppingList"
                    :onIngredientClick="cardStore.addToPantry" />
    <IngredientList
                    title="Pantry"
                    :ingredients="cardStore.getPantry"
                    :onClear="cardStore.clearPantry"
                    :onIngredientClick="cardStore.removeFromPantry" />
  </div>

  <div class="area">
    <Card
          v-for="card in getActiveCards"
          :key="card.id"
          :card="card"
          :onCardClick="cardStore.toggleCard"
          :showIngredients="true" />
  </div>

  <div class="area">
    <Card
          v-for="card in getInactiveCards"
          :key="card.id"
          :card="card"
          :onCardClick="cardStore.toggleCard"
          :showIngredients="false" />
  </div>
</template>

<style scoped lang="scss">
@import "../App";

.area {
  display: flex;
  flex-wrap: wrap;
  padding: 0.5rem;
}
</style>

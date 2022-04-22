<script setup lang="ts">
import { computed } from 'vue';
import { useCardStore } from '@/stores/cardStore';
import IngredientList from '@/components/CardIngredientList.vue';
import Card from '@/components/Card.vue';
import useAppStore from '@/stores/appStore';

const cardStore = useCardStore();

const activeCards = computed(() => cardStore.getCards({ active: true }));
const inactiveCards = computed(() => cardStore.getCards({ active: false }));

const addMessage = () => useAppStore().setSuccessMessage('my message');
const addEMessage = () => useAppStore().setErrorMessage('my message');
</script>

<template>
  <button @click="addMessage">g</button>
  <button @click="addEMessage">b</button>
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
      v-for="card in activeCards"
      :key="card.id"
      :card="card"
      :on-card-click="cardStore.toggleCard"
      :show-ingredients="true"
    />
  </div>

  <div class="area">
    <Card
      v-for="card in inactiveCards"
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

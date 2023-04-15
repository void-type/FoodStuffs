<script lang="ts" setup>
import type { ListRecipesResponse } from '@/api/data-contracts';
import type { PropType } from 'vue';

const props = defineProps({
  card: { type: Object as PropType<ListRecipesResponse>, required: true },
  onCardClick: { type: Function, required: true },
  selected: { type: Boolean, required: false, default: false },
  cardType: { type: String, required: true },
});

function cardClickInternal() {
  props.onCardClick(props.card.id);
}
</script>

<template>
  <div
    :class="['card', selected ? 'card--selected' : '', 'card-hover', 'meal-card']"
    tabindex="0"
    :role="selected ? '' : 'button'"
    @keydown.stop.prevent.enter="cardClickInternal()"
    @click.stop.prevent="cardClickInternal()"
  >
    <div class="card-header-custom d-flex align-items-center">
      <span class="h5">{{ card.name }}</span>
      <div class="d-flex ms-auto">
        <button
          type="button"
          data-bs-toggle="collapse"
          :data-bs-target="`#card-${cardType}-${card.id}-ingredient-collapse`"
          aria-expanded="false"
          :aria-controls="`card-${cardType}-${card.id}-ingredient-collapse`"
          class="btn-card-collapse collapsed"
          @click.stop.prevent="() => {}"
        ></button>
      </div>
    </div>
    <div :id="`card-${cardType}-${card.id}-ingredient-collapse`" class="collapse">
      <div class="card-body">
        <ul>
          <li v-for="ingredient in card.ingredients?.filter(x => x.isCategory === false)" :key="ingredient.name || ''">
            {{ ingredient.quantity }}x {{ ingredient.name }}
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
body.bg-dark .card.card--selected {
  color: var(--bs-gray-700);
}

body .card.card--selected {
  color: var(--bs-gray-500);
}

.card-header-custom {
  padding: 0;

  .h5 {
    padding: var(--bs-accordion-btn-padding-y) var(--bs-accordion-btn-padding-x);
    margin: 0;
  }
}

.btn-card-collapse::after {
  flex-shrink: 0;
  width: var(--bs-accordion-btn-icon-width);
  height: var(--bs-accordion-btn-icon-width);
  margin-left: auto;
  content: '';
  background-image: var(--bs-accordion-btn-icon);
  background-repeat: no-repeat;
  background-size: var(--bs-accordion-btn-icon-width);
  transition: var(--bs-accordion-btn-icon-transition);
}

.btn-card-collapse:not(.collapsed)::after {
  background-image: var(--bs-accordion-btn-active-icon);
  transform: var(--bs-accordion-btn-icon-transform);
}

.btn-card-collapse {
  position: relative;
  display: flex;
  align-items: center;
  width: 100%;
  padding: var(--bs-accordion-btn-padding-y) var(--bs-accordion-btn-padding-x);
  font-size: 1rem;
  color: var(--bs-accordion-btn-color);
  text-align: left;
  background-color: var(--bs-accordion-btn-bg);
  border: 0;
  border-radius: 0;
  overflow-anchor: none;
  transition: var(--bs-accordion-transition);
}
</style>

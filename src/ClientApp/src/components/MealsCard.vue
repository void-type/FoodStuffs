<script lang="ts" setup>
import type { ListRecipesResponse } from '@/api/data-contracts';
import type { PropType } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

const props = defineProps({
  recipe: { type: Object as PropType<ListRecipesResponse>, required: true },
  onCardClick: { type: Function, required: true },
  selected: { type: Boolean, required: false, default: false },
  cardType: { type: String, required: true },
});

function cardClickInternal() {
  props.onCardClick(props.recipe.id);
}
</script>

<template>
  <div>
    <div
      :class="['card', selected ? 'card--selected' : '', 'card-hover', 'meal-card']"
      tabindex="0"
      role="button"
      @keydown.stop.prevent.enter="cardClickInternal()"
      @click.stop.prevent="cardClickInternal()"
    >
      <div class="card-header">
        <div class="h5">{{ recipe.name }}</div>
      </div>
      <div class="d-flex align-items-center">
        <button
          type="button"
          data-bs-toggle="collapse"
          :data-bs-target="`#card-${cardType}-${recipe.id}-ingredient-collapse`"
          aria-expanded="false"
          :aria-controls="`card-${cardType}-${recipe.id}-ingredient-collapse`"
          class="btn-card-collapse collapsed"
          @click.stop.prevent="() => {}"
        ></button>
        <router-link
          class="btn-card-control ms-auto"
          :to="{ name: 'view', params: { id: recipe.id } }"
          @click.stop.prevent
        >
          <font-awesome-icon icon="fa-eye" aria-label="view recipe" />
        </router-link>
        <router-link
          class="btn-card-control"
          :to="{ name: 'edit', params: { id: recipe.id } }"
          @click.stop.prevent
        >
          <font-awesome-icon icon="fa-pencil" aria-label="edit recipe" />
        </router-link>
      </div>
      <div :id="`card-${cardType}-${recipe.id}-ingredient-collapse`" class="collapse">
        <div class="card-body">
          <ul>
            <li
              v-for="ingredient in recipe.ingredients?.filter((x) => x.isCategory === false)"
              :key="ingredient.name || ''"
            >
              {{ ingredient.quantity }}x {{ ingredient.name }}
            </li>
          </ul>
        </div>
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

.btn-card-collapse {
  padding: 0.5rem var(--bs-card-cap-padding-x);
}

.btn-card-collapse::after {
  flex-shrink: 0;
  width: var(--bs-accordion-btn-icon-width);
  height: var(--bs-accordion-btn-icon-width);
  margin-right: auto;
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

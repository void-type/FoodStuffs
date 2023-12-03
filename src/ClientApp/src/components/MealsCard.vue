<script lang="ts" setup>
import type { ListRecipesResponse } from '@/api/data-contracts';
import type { PropType } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import ApiHelpers from '@/models/ApiHelpers';
import ImagePlaceholder from './ImagePlaceholder.vue';

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
      :class="['card', 'card-hover', selected ? 'card--selected' : '']"
      tabindex="0"
      role="button"
      @keydown.stop.prevent.enter="cardClickInternal()"
      @click.stop.prevent="cardClickInternal()"
    >
      <div class="grid gap-0">
        <div class="g-col-4">
          <img
            v-if="recipe.image != null"
            class="img-fluid rounded-start"
            :src="ApiHelpers.imageUrl(recipe.image)"
            :alt="`Image of ${recipe.name}`"
            loading="lazy"
          />
          <ImagePlaceholder v-else class="img-fluid rounded-start" width="100%" height="100%" />
        </div>
        <div class="g-col-8">
          <div class="card-body">
            <h5 class="card-title me-5">{{ recipe.name }}</h5>
            <div class="card-floating-toolbar">
              <div class="d-flex align-items-center">
                <router-link
                  class="btn-card-control ms-auto"
                  :to="{ name: 'view', params: { id: recipe.id } }"
                  @keydown.stop.enter
                  @click.stop
                >
                  <font-awesome-icon icon="fa-eye" aria-label="view recipe" />
                </router-link>
                <router-link
                  class="btn-card-control"
                  :to="{ name: 'edit', params: { id: recipe.id } }"
                  @keydown.stop.enter
                  @click.stop
                >
                  <font-awesome-icon icon="fa-pencil" aria-label="edit recipe" />
                </router-link>
              </div>
            </div>
            <button
              type="button"
              data-bs-toggle="collapse"
              :data-bs-target="`#card-${cardType}-${recipe.id}-ingredient-collapse`"
              aria-expanded="false"
              :aria-controls="`card-${cardType}-${recipe.id}-ingredient-collapse`"
              class="btn-card-collapse mb-2 collapsed"
              @click.stop.prevent="() => {}"
            ></button>
            <div
              :id="`card-${cardType}-${recipe.id}-ingredient-collapse`"
              class="card-text collapse"
            >
              <ul class="text-muted">
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
    </div>
  </div>
</template>

<style lang="scss" scoped>
html[data-bs-theme='dark'] .card.card--selected {
  color: var(--bs-gray-700);
}

body .card.card--selected {
  color: var(--bs-gray-500);
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
  padding: 0;
}

ul.text-muted {
  list-style: none;
  padding: 0;
  margin-bottom: 0;
}
</style>

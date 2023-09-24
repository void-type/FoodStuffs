<script lang="ts" setup>
import type { ListRecipesResponse } from '@/api/data-contracts';
import type { PropType } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import ApiHelpers from '@/models/ApiHelpers';
import ImagePlaceholder from './ImagePlaceholder.vue';

const props = defineProps({
  recipe: { type: Object as PropType<ListRecipesResponse>, required: true },
  onCardClick: { type: Function, required: true },
});

function cardClickInternal() {
  props.onCardClick(props.recipe.id);
}
</script>

<template>
  <div>
    <div
      class="card card-hover meal-card"
      tabindex="0"
      role="button"
      @keydown.stop.prevent.enter="cardClickInternal()"
      @click.stop.prevent="cardClickInternal()"
    >
      <router-link
        class="card-link"
        :to="{ name: 'view', params: { id: recipe.id } }"
        @click.stop.prevent
      >
        <div class="grid">
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
              <h5 class="card-title">{{ recipe.name }}</h5>
              <div class="card-text">
                <small class="text-muted">{{ (recipe?.categories || []).join(', ') }}</small>
              </div>
              <div class="btn-card-toolbar">
                <router-link
                  class="btn-card-control"
                  :to="{ name: 'edit', params: { id: recipe.id } }"
                  @click.stop.prevent
                >
                  <font-awesome-icon icon="fa-pencil" aria-label="edit recipe" />
                </router-link>
              </div>
            </div>
          </div>
        </div>
      </router-link>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.btn-card-toolbar {
  position: absolute;
  bottom: 0;
  right: 0;
}
</style>

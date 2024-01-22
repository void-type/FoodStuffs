<script lang="ts" setup>
import type { SearchRecipesResponse } from '@/api/data-contracts';
import type { PropType } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import ApiHelpers from '@/models/ApiHelpers';
import ImagePlaceholder from './ImagePlaceholder.vue';

defineProps({
  recipe: { type: Object as PropType<SearchRecipesResponse>, required: true },
});
</script>

<template>
  <div>
    <div class="card card-hover">
      <router-link
        class="card-link grid gap-0"
        :to="{ name: 'view', params: { id: recipe.id } }"
        @click.stop.prevent
      >
        <div class="g-col-4">
          <img
            v-if="recipe.image != null"
            class="img-fluid rounded-start"
            :src="ApiHelpers.imageUrl(recipe.image)"
            :alt="`Image of ${recipe.name}`"
            loading="lazy"
            width="1600"
            height="1200"
          />
          <ImagePlaceholder v-else class="img-fluid rounded-start" />
        </div>
        <div class="g-col-8">
          <div class="card-body">
            <h5 class="card-title me-4">{{ recipe.name }}</h5>
            <div class="card-floating-toolbar">
              <router-link
                class="btn-card-control"
                :to="{ name: 'edit', params: { id: recipe.id } }"
                @click.stop.prevent
              >
                <font-awesome-icon icon="fa-pencil" aria-label="edit recipe" />
              </router-link>
            </div>
            <div class="card-subtitle mb-2 text-muted">
              {{ (recipe?.categories || []).join(', ') }}
            </div>
          </div>
        </div>
      </router-link>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>

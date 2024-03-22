<script lang="ts" setup>
import type { RecipeSearchResultItem } from '@/api/data-contracts';
import type { PropType } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import ApiHelpers from '@/models/ApiHelpers';
import RouterHelpers from '@/models/RouterHelpers';
import ImagePlaceholder from './ImagePlaceholder.vue';

defineProps({
  recipe: { type: Object as PropType<RecipeSearchResultItem>, required: true },
  imgLazy: { type: Boolean, required: false, default: false },
});
</script>

<template>
  <div class="card card-hover">
    <router-link class="card-link" :to="RouterHelpers.viewRecipe(recipe)">
      <div class="card-title m-2 me-5">{{ recipe.name }}</div>
      <div class="card-floating-toolbar">
        <router-link
          class="btn-card-control ms-auto"
          :to="RouterHelpers.editRecipe(recipe)"
          @click.stop.prevent
        >
          <font-awesome-icon icon="fa-pencil" aria-label="edit recipe" />
        </router-link>
      </div>
      <div class="image-container">
        <img
          v-if="recipe.image != null"
          class="img-fluid rounded-bottom"
          :src="ApiHelpers.imageUrl(recipe.image)"
          :alt="`Image of ${recipe.name}`"
          :loading="imgLazy ? 'lazy' : 'eager'"
          width="1600"
          height="1200"
        />
        <ImagePlaceholder v-else class="img-fluid rounded-bottom position-absolute" />
      </div>
    </router-link>
  </div>
</template>

<style lang="scss" scoped>
.image-container {
  position: relative;
  width: 100%;
  height: 0;
  padding-top: calc(1200 / 1600 * 100%);
  overflow: hidden;
}

.image-container img {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.card-title {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
</style>

<script lang="ts" setup>
import { ref, computed, defineProps, type PropType } from 'vue';
import type { GetRecipeResponse } from '@/api/data-contracts';
import ApiHelpers from '@/models/ApiHelpers';
import RecipeTimeSpan from '@/models/RecipeTimeSpan';
import EntityAuditInfo from './EntityAuditInfo.vue';
import RecipeViewerIngredients from './RecipeViewerIngredients.vue';

const props = defineProps({
  recipe: {
    type: Object as PropType<GetRecipeResponse>,
    required: true,
  },
});

const showImage = ref(true);

const carouselIndex = ref(0);

const images = computed(() => {
  const { pinnedImageId } = props.recipe;
  const recipeImages = props.recipe.images || [];

  if (pinnedImageId != null && recipeImages.includes(pinnedImageId)) {
    const sortedRecipeImages = recipeImages.filter((i) => i !== pinnedImageId);

    sortedRecipeImages.unshift(pinnedImageId);

    return sortedRecipeImages;
  }

  return recipeImages;
});

function imageUrl(id: number) {
  return ApiHelpers.imageUrl(id);
}

function timeSpanFormat(minutes: number | null | undefined) {
  if (minutes === null || minutes === undefined) {
    return '';ssssssssssssss
  }

  return new RecipeTimeSpan(minutes).toString();
}
</script>

<template>
  <div v-if="recipe.name" class="viewer">
    <b-row class="no-print">
      <b-col cols="12">
        <b-button-toolbar>
          <b-button
            :to="{ name: 'edit', params: { id: recipe.id } }"
            class="mr-2"
            variant="primary"
          >
            Edit
          </b-button>
          <b-form-checkbox id="showImage" v-model="showImage" name="showImage" class="mt-2" switch>
            Image
          </b-form-checkbox>
        </b-button-toolbar>
      </b-col>
    </b-row>
    <b-row>
      <b-col v-if="showImage" cols="12" class="text-center mt-3">
        <b-carousel
          v-if="images.length > 0"
          id="image-carousel"
          v-model="carouselIndex"
          :interval="0"
          no-animation
          controls
          indicators
        >
          <b-carousel-slide v-for="image in images" :key="image">
            <template #img>
              <b-img fluid rounded :src="imageUrl(image)" img />
            </template>
          </b-carousel-slide>
        </b-carousel>
      </b-col>
    </b-row>
    <h3 class="mt-3">Ingredients</h3>
    <RecipeViewerIngredients :ingredients="recipe.ingredients || []" />
    <h3 class="mt-3">Directions</h3>
    <b-form-textarea
      plaintext
      rows="1"
      :max-rows="Number.MAX_SAFE_INTEGER"
      :value="recipe.directions"
    />
    <h3 class="mt-3">Stats</h3>
    <div v-if="recipe.prepTimeMinutes || 0 > 0">
      Prep Time: {{ timeSpanFormat(recipe.prepTimeMinutes) }}
    </div>
    <div v-if="recipe.cookTimeMinutes || 0 > 0">
      Cook Time: {{ timeSpanFormat(recipe.cookTimeMinutes) }}
    </div>
    <div v-if="(recipe.categories || []).length > 0">
      Categories: {{ (recipe.categories || []).join(', ') }}
    </div>
    <div>
      <EntityAuditInfo class="mt-3" :entity="recipe" />
    </div>
  </div>
</template>

<style lang="scss" scoped>
textarea {
  overflow: hidden !important;
  resize: none;
}

// Don't print carousel controls.
@media print {
  #image-carousel ::v-deep {
    & a,
    ol {
      display: none;
    }
  }
}
</style>

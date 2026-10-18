<script lang="ts" setup>
import type bootstrap from 'bootstrap';
import type { PropType } from 'vue';
import type { GetRecipeResponse, GetRecipeResponseGroceryItem } from '@/api/data-contracts';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { computed, onBeforeUnmount, ref, useTemplateRef, watch } from 'vue';
import EntityAuditInfo from '@/components/EntityAuditInfo.vue';
import ApiHelper from '@/models/ApiHelper';
import { isNil } from '@/models/FormatHelper';
import RouterHelper from '@/models/RouterHelper';
import { toTimeSpanString } from '@/models/TimeSpanHelper';
import useImageLightboxStore from '@/stores/imageLightboxStore';
import RecipeCurrentMealPlanButton from './RecipeCurrentMealPlanButton.vue';
import TagBadge from './TagBadge.vue';

const props = defineProps({
  recipe: {
    type: Object as PropType<GetRecipeResponse>,
    required: true,
  },
});

const carouselIndex = ref(0);
const carouselElement = useTemplateRef<HTMLElement>('carouselElement');
const uniqueId = crypto.randomUUID();
const imageLightboxStore = useImageLightboxStore();

const images = computed(() => {
  const { pinnedImage } = props.recipe;
  const recipeImages = props.recipe.images || [];

  if (pinnedImage != null && recipeImages.includes(pinnedImage)) {
    const sortedRecipeImages = recipeImages.filter(i => i !== pinnedImage);

    sortedRecipeImages.unshift(pinnedImage);

    return sortedRecipeImages;
  }

  return recipeImages;
});

const categories = computed(() => props.recipe.categories || []);
const groceryItems = computed(() => props.recipe.groceryItems || []);
const sidesCount = computed(() => props.recipe.mealPlanningSidesCount || 0);
const hasSides = computed(() => !isNil(props.recipe.sides));
const hasDirections = computed(() => !isNil(props.recipe.directions));
const hasSideColumn = computed(() => groceryItems.value.length > 0 || hasSides.value);

const timings = computed(() => {
  const prep = props.recipe.prepTimeMinutes || 0;
  const cook = props.recipe.cookTimeMinutes || 0;

  return {
    prep: prep > 0 ? toTimeSpanString(prep) : null,
    cook: cook > 0 ? toTimeSpanString(cook) : null,
    // Only useful when it differs from the single value shown.
    total: prep > 0 && cook > 0 ? toTimeSpanString(prep + cook) : null,
  };
});

const hasMeta = computed(() => !!(timings.value.prep || timings.value.cook || sidesCount.value > 0));

function stockStatus(item: GetRecipeResponseGroceryItem) {
  const onHand = item.inventoryQuantity || 0;

  if (onHand >= (item.quantity || 0)) {
    return { text: 'In stock', badgeClass: 'text-bg-success' };
  }

  if (onHand > 0) {
    return { text: `${onHand} on hand`, badgeClass: 'text-bg-warning' };
  }

  return { text: 'Out', badgeClass: 'text-bg-danger' };
}

watch(
  () => props.recipe.id,
  () => {
    carouselIndex.value = 0;
  },
);

// Keep carouselIndex in step with the slide the user moved to, so re-renders don't jump back.
function onSlid(event: Event) {
  carouselIndex.value = (event as unknown as bootstrap.Carousel.Event).to;
}

// The carousel only exists while there are images, so follow the element as it comes and goes.
watch(
  carouselElement,
  (element, previousElement) => {
    previousElement?.removeEventListener('slid.bs.carousel', onSlid);
    element?.addEventListener('slid.bs.carousel', onSlid);
  },
  { flush: 'post', immediate: true },
);

onBeforeUnmount(() => {
  carouselElement.value?.removeEventListener('slid.bs.carousel', onSlid);
});
</script>

<template>
  <div>
    <div class="grid align-items-center">
      <!-- Image first on mobile, to the right on md+. Not printed. -->
      <div v-if="images.length > 0" class="g-col-12 g-col-md-6 order-md-2 d-print-none">
        <div
          :id="`image-carousel-${uniqueId}`"
          ref="carouselElement"
          class="carousel slide recipe-carousel"
          data-bs-interval="false"
        >
          <div v-if="images.length > 1" class="carousel-indicators">
            <button
              v-for="(imageName, i) in images"
              :key="imageName"
              type="button"
              :data-bs-target="`#image-carousel-${uniqueId}`"
              :data-bs-slide-to="i"
              :class="{ active: i === carouselIndex }"
              :aria-current="i === carouselIndex"
              :aria-label="`Show image ${i + 1}`"
            />
          </div>
          <div class="carousel-inner rounded">
            <div
              v-for="(imageName, i) in images"
              :key="imageName"
              class="carousel-item" :class="{ active: i === carouselIndex }"
            >
              <img
                class="d-block w-100 h-auto object-fit-cover"
                role="button"
                :src="ApiHelper.imageUrl(imageName)"
                :alt="`image ${i + 1} of ${recipe.name}`"
                :loading="i > 0 ? 'lazy' : 'eager'"
                width="1600"
                height="1200"
                style="aspect-ratio: 4 / 3; cursor: pointer"
                @click="imageLightboxStore.open(images, i)"
              >
            </div>
          </div>
          <template v-if="images.length > 1">
            <button
              type="button"
              class="carousel-control-prev"
              :data-bs-target="`#image-carousel-${uniqueId}`"
              data-bs-slide="prev"
            >
              <span class="carousel-control-prev-icon" aria-hidden="true" />
              <span class="visually-hidden">Previous image</span>
            </button>
            <button
              type="button"
              class="carousel-control-next"
              :data-bs-target="`#image-carousel-${uniqueId}`"
              data-bs-slide="next"
            >
              <span class="carousel-control-next-icon" aria-hidden="true" />
              <span class="visually-hidden">Next image</span>
            </button>
          </template>
        </div>
      </div>
      <div class="recipe-summary g-col-12 order-md-1" :class="{ 'g-col-md-6': images.length > 0 }">
        <h1 class="mb-3">
          {{ recipe.name }}
        </h1>
        <div v-if="categories.length > 0" class="d-flex flex-wrap gap-1 mb-3">
          <TagBadge
            v-for="category in categories"
            :key="category.id"
            :tag="category"
            :to="RouterHelper.searchRecipesByCategory(category)"
          />
        </div>
        <ul v-if="hasMeta" class="list-inline text-muted mb-0">
          <li v-if="timings.prep" class="list-inline-item me-3">
            <FontAwesomeIcon icon="fa-kitchen-set" class="me-1" aria-hidden="true" />Prep {{ timings.prep }}
          </li>
          <li v-if="timings.cook" class="list-inline-item me-3">
            <FontAwesomeIcon icon="fa-fire" class="me-1" aria-hidden="true" />Cook {{ timings.cook }}
          </li>
          <li v-if="timings.total" class="list-inline-item me-3">
            <FontAwesomeIcon icon="fa-clock" class="me-1" aria-hidden="true" />Total {{ timings.total }}
          </li>
          <li v-if="sidesCount > 0" class="list-inline-item me-3 d-print-none">
            <FontAwesomeIcon icon="fa-utensils" class="me-1" aria-hidden="true" />{{ sidesCount }} side{{ sidesCount !== 1 ? 's' : '' }}
          </li>
        </ul>
        <div class="btn-toolbar mt-3 d-print-none">
          <router-link :to="RouterHelper.editRecipe(recipe)" class="btn btn-primary me-2">
            Edit
          </router-link>
          <RecipeCurrentMealPlanButton :recipe-id="recipe.id" />
        </div>
      </div>
    </div>

    <div class="grid mt-4">
      <div v-if="hasSideColumn" class="g-col-12 g-col-lg-4">
        <div class="card">
          <div class="card-body">
            <section v-if="groceryItems.length > 0">
              <h2 class="h4">
                Ingredients
              </h2>
              <ul class="list-unstyled mb-0">
                <li
                  v-for="item in groceryItems"
                  :key="item.id"
                  class="d-flex align-items-baseline justify-content-between gap-2 py-1"
                >
                  <span>
                    {{ item.quantity }}x
                    <router-link :to="RouterHelper.editGroceryItem(item)">{{ item.name }}</router-link>
                  </span>
                  <span class="badge flex-shrink-0 d-print-none" :class="stockStatus(item).badgeClass">
                    {{ stockStatus(item).text }}
                  </span>
                </li>
              </ul>
            </section>
            <section v-if="hasSides" :class="{ 'mt-4': groceryItems.length > 0 }">
              <h2 class="h4">
                Sides
              </h2>
              <div class="sides-text">
                {{ recipe.sides }}
              </div>
            </section>
          </div>
        </div>
      </div>
      <div v-if="hasDirections" class="g-col-12" :class="{ 'g-col-lg-8': hasSideColumn }">
        <section>
          <h2 class="h4">
            Directions
          </h2>
          <!-- eslint-disable-next-line vue/no-v-html -->
          <div class="rich-text" v-html="recipe.directions" />
        </section>
      </div>
    </div>
    <EntityAuditInfo class="mt-4" :entity="recipe" />
  </div>
</template>

<style lang="scss" scoped>
// The 4:3 images get very tall when the column is wide, so cap the width and center it.
.recipe-carousel {
  max-width: 28rem;
  margin-inline: auto;
}

.sides-text {
  white-space: pre-wrap;
}

@media print {
  // The image isn't printed, so let the summary use the full width.
  .recipe-summary {
    grid-column: 1 / -1;
  }

  .card {
    border: 0;
  }
}
</style>

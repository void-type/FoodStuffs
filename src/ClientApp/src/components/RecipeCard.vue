<script lang="ts" setup>
import type { SearchRecipesResultItem } from '@/api/data-contracts';
import { computed, type PropType } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import ApiHelper from '@/models/ApiHelper';
import RouterHelper from '@/models/RouterHelper';
import ImagePlaceholder from './ImagePlaceholder.vue';
import RecipeMealButton from './RecipeMealButton.vue';
import AppSortHandle from './AppSortHandle.vue';

const props = defineProps({
  recipe: { type: Object as PropType<SearchRecipesResultItem>, required: true },
  imgLazy: { type: Boolean, required: false, default: false },
  showSortHandle: { type: Boolean, required: false, default: false },
  showCompactView: { type: Boolean, required: false, default: false },
});

const recipeCardId = computed(() => `recipe-card-${props.recipe.id}`);

function flipCard() {
  const card = document.getElementById(recipeCardId.value);

  const front = card?.querySelector('.card-flip-front');
  front?.classList.toggle('invisible');

  const back = card?.querySelector('.card-flip-back');
  back?.classList.toggle('d-none');
}
</script>

<template>
  <div :id="recipeCardId" class="card card-hover">
    <AppSortHandle v-if="props.showSortHandle" class="card-header-button left" />
    <div
      :class="{
        'card-header': true,
        sortable: props.showSortHandle,
        flippable: !props.showCompactView,
      }"
    >
      <router-link class="card-link" :to="RouterHelper.viewRecipe(recipe)">
        {{ recipe.name }}
      </router-link>
    </div>
    <div v-if="!props.showCompactView" class="card-header-button right">
      <a href="#" aria-label="flip card" @click.stop.prevent="flipCard">
        <font-awesome-icon icon="fa-rotate" />
      </a>
    </div>
    <router-link class="card-link" :to="RouterHelper.viewRecipe(recipe)">
      <div v-if="!props.showCompactView" class="card-flip-container">
        <div class="card-flip-front">
          <div class="image-container">
            <img
              v-if="recipe.image != null"
              class="img-fluid rounded-bottom"
              :src="ApiHelper.imageUrl(recipe.image)"
              :alt="`Image of ${recipe.name}`"
              :loading="imgLazy ? 'lazy' : 'eager'"
              width="1600"
              height="1200"
            />
            <ImagePlaceholder
              v-else
              class="img-fluid rounded-bottom position-absolute top-0 left-0"
            />
          </div>
        </div>
        <div class="card-flip-back p-3 d-none">
          <div class="card-flip-back-inner slim-scroll">
            <div class="btn-toolbar">
              <router-link
                type="button"
                class="btn btn-sm btn-secondary me-2"
                aria-label="edit recipe"
                :to="RouterHelper.editRecipe(recipe)"
                @click.stop
                >Edit</router-link
              >
              <RecipeMealButton class="btn-sm" :recipe-id="recipe.id" />
            </div>
            <div v-if="(recipe.shoppingItems?.length || 0) > 0" class="mt-3">
              <div>Shopping Items</div>
              <ul>
                <li v-for="ingredient in recipe.shoppingItems" :key="ingredient.name || ''">
                  {{ ingredient.quantity }}x {{ ingredient.name }}
                </li>
              </ul>
            </div>
            <div v-if="(recipe.categories?.length || 0) > 0" class="mt-3">
              <span
                v-for="category in recipe.categories"
                :key="category || ''"
                class="badge text-bg-secondary me-2 mt-2"
              >
                {{ category }}</span
              >
            </div>
          </div>
        </div>
      </div>
      <div v-else class="card-body">
        <div class="btn-toolbar">
          <router-link
            type="button"
            class="btn btn-sm btn-secondary me-2 mb-2"
            aria-label="edit recipe"
            :to="RouterHelper.editRecipe(recipe)"
            @click.stop
            >Edit</router-link
          >
          <RecipeMealButton class="btn-sm mb-2" :recipe-id="recipe.id" />
        </div>
        <div v-if="(recipe.categories?.length || 0) > 0">
          <span
            v-for="category in recipe.categories"
            :key="category || ''"
            class="badge text-bg-secondary me-2 mt-2"
          >
            {{ category }}</span
          >
        </div>
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

  img {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    object-fit: cover;
  }
}

.card-header {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;

  &.sortable {
    padding-left: 2.4rem;
  }

  &.flippable {
    padding-right: 2.4rem;
  }
}

.card-header-button {
  padding: 0.5rem var(--bs-card-cap-padding-x);
  display: inline-block;
  z-index: 2;

  position: absolute;
  top: 0;

  &.right {
    right: 0;
  }

  &.left {
    left: 0;
  }
}

.card-flip-container {
  position: relative;
}

.card-flip-back {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  overflow: hidden;
}

.card-flip-back-inner {
  height: 100%;
  width: 100%;
}
</style>

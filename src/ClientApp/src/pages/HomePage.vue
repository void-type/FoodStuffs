<script lang="ts" setup>
import ImagePlaceholder from '@/components/ImagePlaceholder.vue';
import ApiHelpers from '@/models/ApiHelpers';
import ListRecipesRequest from '@/models/ListRecipesRequest';
import useMessageStore from '@/stores/messageStore';
import useRecipeStore from '@/stores/recipeStore';
import { storeToRefs } from 'pinia';
import { onMounted } from 'vue';

const recipeStore = useRecipeStore();
const messageStore = useMessageStore();
const api = ApiHelpers.client;

const { discoverListResponse } = storeToRefs(recipeStore);

const imageUrl = (id: number | string) => ApiHelpers.imageUrl(id);

onMounted(() => {
  if (discoverListResponse.value.count === 0) {
    api()
      .recipesList({
        ...new ListRecipesRequest(),
        sortBy: 'random',
      })
      .then((response) => recipeStore.setDiscoverListResponse(response.data))
      .catch((response) => messageStore.setApiFailureMessages(response));
  }
});
</script>

<template>
  <div class="container-xxl">
    <div class="grid mt-4">
      <div
        v-for="(recipe, i) in discoverListResponse.items"
        :key="recipe.id"
        class="g-col-12 g-col-md-6 g-col-lg-4"
      >
        <div class="card card-hover overflow-hidden">
          <router-link
            class="card-link text-center p-3"
            :to="{ name: 'view', params: { id: recipe.id } }"
          >
            <img
              v-if="recipe.imageId != null"
              class="img-fluid rounded"
              :src="imageUrl(recipe.imageId)"
              :alt="`image of ${recipe.name}`"
              :loading="i > 5 ? 'lazy' : 'eager'"
            />
            <ImagePlaceholder v-else />
            <div class="mt-3">
              <h4 class="card-title">
                {{ recipe.name }}
              </h4>
              <p class="card-text">
                {{ (recipe?.categories || []).join(', ') }}
              </p>
            </div>
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>

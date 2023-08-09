<script lang="ts" setup>
import ImagePlaceholder from '@/components/ImagePlaceholder.vue';
import ApiHelpers from '@/models/ApiHelpers';
import ListRecipesRequest from '@/models/ListRecipesRequest';
import useMessageStore from '@/stores/messageStore';
import useRecipeStore from '@/stores/recipeStore';
import { storeToRefs } from 'pinia';
import { onMounted } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

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
          <router-link class="card-link" :to="{ name: 'view', params: { id: recipe.id } }">
            <div>
              <div class="card-title h5 m-3">{{ recipe.name }}</div>
              <div class="btn-card-toolbar">
                <router-link
                  class="btn-card-control ms-auto"
                  :to="{ name: 'edit', params: { id: recipe.id } }"
                  @click.stop.prevent
                >
                  <font-awesome-icon icon="fa-pencil" aria-label="edit recipe" />
                </router-link>
              </div>
              <img
                v-if="recipe.imageId != null"
                class="img-fluid"
                :src="imageUrl(recipe.imageId)"
                :alt="`Image of ${recipe.name}`"
                :loading="i > 5 ? 'lazy' : 'eager'"
              />
              <ImagePlaceholder v-else class="img-fluid rounded-bottom" width="100%" />
            </div>
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.btn-card-toolbar {
  position: absolute;
  top: 0;
  right: 0;
}
</style>

<script lang="ts" setup>
import ImagePlaceholder from '@/components/ImagePlaceholder.vue';
import ApiHelpers from '@/models/ApiHelpers';
import SearchRecipesRequest from '@/models/SearchRecipesRequest';
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
        ...new SearchRecipesRequest(),
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
        <div class="card card-hover">
          <router-link class="card-link" :to="{ name: 'view', params: { id: recipe.id } }">
            <div class="card-title h5 m-3 me-5">{{ recipe.name }}</div>
            <div class="card-floating-toolbar">
              <router-link
                class="btn-card-control ms-auto"
                :to="{ name: 'edit', params: { id: recipe.id } }"
                @click.stop.prevent
              >
                <font-awesome-icon icon="fa-pencil" aria-label="edit recipe" />
              </router-link>
            </div>
            <div class="image-container">
              <img
                v-if="recipe.image != null"
                class="img-fluid rounded-bottom"
                :src="imageUrl(recipe.image)"
                :alt="`Image of ${recipe.name}`"
                :loading="i > 5 ? 'lazy' : 'eager'"
                width="1600"
                height="1200"
              />
              <ImagePlaceholder v-else class="img-fluid rounded-bottom position-absolute" />
            </div>
          </router-link>
        </div>
      </div>
    </div>
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

.position-absolute {
  position: absolute;
  top: 0;
  left: 0;
}
</style>

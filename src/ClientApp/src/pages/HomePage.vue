<script lang="ts" setup>
import ImagePlaceholder from '@/components/ImagePlaceholder.vue';
import ApiHelpers from '@/models/ApiHelpers';
import SearchRecipesRequest from '@/models/SearchRecipesRequest';
import useMessageStore from '@/stores/messageStore';
import useRecipeStore from '@/stores/recipeStore';
import { storeToRefs } from 'pinia';
import { onMounted, ref, type Ref } from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

const recipeStore = useRecipeStore();
const messageStore = useMessageStore();
const api = ApiHelpers.client;

const { discoverList, discoverPage } = storeToRefs(recipeStore);

const imageUrl = (id: number | string) => ApiHelpers.imageUrl(id);

const isFetchingRecipes = ref(false);

function fetchRecipes() {
  if (isFetchingRecipes.value) {
    return;
  }

  isFetchingRecipes.value = true;

  api()
    .recipesList({
      ...new SearchRecipesRequest(),
      page: discoverPage.value + 1,
      take: 8,
      sortBy: 'random',
    })
    .then((response) => {
      recipeStore.setDiscoverListResponse(response.data);
      isFetchingRecipes.value = false;
    })
    .catch((response) => {
      messageStore.setApiFailureMessages(response);
      isFetchingRecipes.value = false;
    });
}

const loadMoreTrigger: Ref<Element | undefined> = ref();

function observeLoadMoreTrigger() {
  if (!loadMoreTrigger.value) {
    return;
  }

  const observer = new IntersectionObserver(
    (entries) => {
      if (entries[0].isIntersecting) {
        fetchRecipes();
      }
    },
    {
      threshold: 1,
    }
  );

  observer.observe(loadMoreTrigger.value);
}

onMounted(() => {
  observeLoadMoreTrigger();
});
</script>

<template>
  <div class="container-xxl">
    <div class="grid mt-4">
      <div
        v-for="(recipe, i) in discoverList"
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
    <p v-if="discoverList.length > 0" class="mt-3 mb-4">
      <a href="#main">Back to top</a>
    </p>
    <div ref="loadMoreTrigger" class="m-0"></div>
    <div v-if="isFetchingRecipes" class="g-col-12 text-center">
      <div class="spinner-border" role="status">
        <span class="visually-hidden">Loading...</span>
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
</style>

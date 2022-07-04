<script lang="ts" setup>
import { Api } from '@/api/Api';
import ApiHelpers from '@/models/ApiHelpers';
import useAppStore from '@/stores/appStore';
import useRecipeStore from '@/stores/recipeStore';
import { storeToRefs } from 'pinia';
import { onMounted } from 'vue';

const appStore = useAppStore();
const recipeStore = useRecipeStore();

const { listResponse, listRequest } = storeToRefs(recipeStore);

const imageUrl = (id: number | string) => ApiHelpers.imageUrl(id);

onMounted(() => {
  if (listResponse.value.count === 0) {
    new Api()
      .recipesList(listRequest.value)
      .then((response) => recipeStore.setListResponse(response.data))
      .catch((response) => appStore.setApiFailureMessages(response));
  }
});
</script>

<template>
  <div class="container-xxl">
    <div class="row">
      <div v-for="(recipe, i) in listResponse.items" :key="recipe.id" class="col-lg-4 mt-3">
        <div no-body class="card overflow-hidden">
          <router-link
            class="card-link text-center p-3"
            :to="{ name: 'view', params: { id: recipe.id } }"
          >
            <img
              v-if="recipe.imageId != null"
              class="img-fluid rounded"
              :src="imageUrl(recipe.imageId)"
              :alt="`image of ${recipe.name}`"
              :loading="i > 3 ? 'lazy' : 'eager'"
            />
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

<style lang="scss" scoped>
@import '@/styles/theme.scss';
@import 'bootstrap/scss/bootstrap';

.card {
  outline: $gray-500 1px solid;

  &:hover {
    background-color: $gray-200;
  }
}

.card-link {
  text-decoration: none;
  color: unset;

  & > img {
    max-height: 350px;
  }
}
</style>

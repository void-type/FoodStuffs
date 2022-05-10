<script setup lang="ts">
import { Api } from '@/api/Api';
import ApiHelpers from '@/models/ApiHelpers';
import useAppStore from '@/stores/appStore';
import useRecipeStore from '@/stores/recipeStore';
import { storeToRefs } from 'pinia';
import { computed, onMounted } from 'vue';

const appStore = useAppStore();
const recipeStore = useRecipeStore();

const { listResponse, listRequest } = storeToRefs(recipeStore);

const recipes = computed(() => listResponse.value.items?.slice(0, 12));

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
  <div class="container-lg">
    <div class="row mt-3">
      <div v-for="recipe in recipes" :key="recipe.id" class="col-lg-4 mt-3">
        <div no-body class="card overflow-hidden">
          <router-link class="card-link" :to="{ name: 'view', params: { id: recipe.id } }">
            <div v-if="recipe.imageId != null">
              <img
                class="card-img"
                fluid
                :src="imageUrl(recipe.imageId)"
                :alt="`image of ${recipe.name}`"
              />
            </div>
            <div class="p-3">
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
.card-link {
  text-decoration: none;
  color: unset;
}
</style>

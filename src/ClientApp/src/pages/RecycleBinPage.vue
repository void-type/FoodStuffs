<script lang="ts" setup>
import type { ListDeletedRecipesResponseRecipe } from '@/api/data-contracts';
import type { HttpResponse } from '@/api/http-client';
import type { ModalParameters } from '@/models/ModalParameters';
import { storeToRefs } from 'pinia';
import { computed, ref } from 'vue';
import AppPageHeading from '@/components/AppPageHeading.vue';
import ApiHelper from '@/models/ApiHelper';
import DateHelper from '@/models/DateHelper';
import useAppStore from '@/stores/appStore';
import useMessageStore from '@/stores/messageStore';

const appStore = useAppStore();
const messageStore = useMessageStore();
const api = ApiHelper.client;

const { useDarkMode } = storeToRefs(appStore);

const recipes = ref<ListDeletedRecipesResponseRecipe[]>([]);
const retentionDays = ref<number | null>(null);

const resultCountText = computed(() => {
  const count = recipes.value.length;

  return count > 0
    ? `Found ${count} deleted ${count === 1 ? 'recipe' : 'recipes'}.`
    : 'The recycle bin is empty.';
});

async function fetchDeletedRecipes() {
  try {
    const response = await api().recipesListDeleted({ isPagingEnabled: false });
    recipes.value = response.data.recipes?.items || [];
    retentionDays.value = response.data.retentionDays ?? null;
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

async function onRestoreRecipe(id: number | null | undefined) {
  if (!id) {
    return;
  }

  try {
    const response = await api().recipesRestore({ id });
    await fetchDeletedRecipes();

    if (response.data.message) {
      messageStore.setSuccessMessage(response.data.message);
    }
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

function onPermanentlyDeleteRecipe(id: number | null | undefined) {
  async function permanentlyDeleteRecipe() {
    if (!id) {
      return;
    }

    try {
      const response = await api().recipesPermanentlyDelete({ id });
      await fetchDeletedRecipes();

      if (response.data.message) {
        messageStore.setSuccessMessage(response.data.message);
      }
    } catch (error) {
      messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    }
  }

  const parameters: ModalParameters = {
    title: 'Permanently delete recipe',
    description: 'Do you really want to permanently delete this recipe? This can\'t be undone.',
    okAction: () => permanentlyDeleteRecipe(),
  };

  appStore.showModal(parameters);
}

function onEmptyRecycleBin() {
  async function emptyRecycleBin() {
    try {
      const response = await api().recipesEmptyRecycleBin();
      await fetchDeletedRecipes();

      if (response.data.message) {
        messageStore.setSuccessMessage(response.data.message);
      }
    } catch (error) {
      messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    }
  }

  const parameters: ModalParameters = {
    title: 'Empty recycle bin',
    description: 'Do you really want to permanently delete every recipe in the recycle bin? This can\'t be undone.',
    okAction: () => emptyRecycleBin(),
  };

  appStore.showModal(parameters);
}

fetchDeletedRecipes();
</script>

<template>
  <div class="container-xxl">
    <AppPageHeading />
    <p class="mt-3">
      Deleted recipes are hidden until you restore them.
      <template v-if="retentionDays">
        Recipes are permanently deleted automatically {{ retentionDays }} days after they are deleted.
      </template>
    </p>
    <div class="btn-toolbar">
      <button
        class="btn btn-danger me-2"
        type="button"
        :disabled="recipes.length === 0"
        @click.stop.prevent="onEmptyRecycleBin()"
      >
        Empty recycle bin
      </button>
    </div>
    <div class="mt-3">
      {{ resultCountText }}
    </div>
    <table
      v-if="recipes.length > 0"
      class="table mt-4" :class="{ 'table-dark': useDarkMode }"
    >
      <thead>
        <tr>
          <th>Name</th>
          <th>Deleted</th>
          <th v-if="retentionDays">
            Permanently deleted on
          </th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="recipe in recipes" :key="recipe.id">
          <td>{{ recipe.name }}</td>
          <td>{{ DateHelper.dateTimeForView(recipe.deletedOn) }} by {{ recipe.deletedBy }}</td>
          <td v-if="retentionDays">
            {{ DateHelper.dateTimeForView(recipe.purgeOn) }}
          </td>
          <td>
            <button class="btn btn-sm btn-primary me-2" @click="() => onRestoreRecipe(recipe.id)">
              Restore
            </button>
            <button class="btn btn-sm btn-danger" @click="() => onPermanentlyDeleteRecipe(recipe.id)">
              Delete permanently
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style lang="scss" scoped></style>

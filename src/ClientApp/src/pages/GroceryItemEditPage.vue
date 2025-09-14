<script lang="ts" setup>
import { computed, reactive, watch, onMounted, onBeforeUnmount, ref } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import useAppStore from '@/stores/appStore';
import useMessageStore from '@/stores/messageStore';
import useGroceryItemStore from '@/stores/groceryItemStore';
import { trimAndTitleCase, isNil } from '@/models/FormatHelper';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';
import AppPageHeading from '@/components/AppPageHeading.vue';
import GroceryItemGetResponse from '@/models/GroceryItemGetResponse';
import GroceryItemWorking from '@/models/GroceryItemWorking';
import type { HttpResponse } from '@/api/http-client';
import type { GetGroceryItemResponse } from '@/api/data-contracts';
import type { ModalParameters } from '@/models/ModalParameters';
import router from '@/router';
import {
  type RouteLocationNormalized,
  type NavigationGuardNext,
  onBeforeRouteUpdate,
  onBeforeRouteLeave,
} from 'vue-router';
import EntityAuditInfo from '@/components/EntityAuditInfo.vue';
import TagEditor from '@/components/TagEditor.vue';
import RouterHelper from '@/models/RouterHelper';
import GroceryItemGroceryAisleSelect from '@/components/GroceryItemGroceryAisleSelect.vue';

const props = defineProps({
  id: {
    type: Number,
    required: false,
    default: 0,
  },
});

const data = reactive({
  source: new GroceryItemGetResponse() as GetGroceryItemResponse,
  working: new GroceryItemWorking(),
  // This is a snapshot of our source right after it became working so we can check if working is dirty.
  workingInitial: '',
});

const appStore = useAppStore();
const messageStore = useMessageStore();
const groceryItemStore = useGroceryItemStore();
const api = ApiHelper.client;

// Editing existing
const isEditMode = computed(() => (props.id || 0) > 0);
// Create new
const isCreateNewMode = computed(() => !isEditMode.value);

const isDirty = computed(() => JSON.stringify(data.working) !== data.workingInitial);

const storageLocationOptions = ref([] as Array<string>);

async function fetchStorageLocations() {
  try {
    const response = await api().storageLocationsList({ isPagingEnabled: false });
    storageLocationOptions.value =
      response.data.items?.map((x) => x.name || '').filter((x) => !isNil(x)) || [];
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

async function fetch() {
  if (isCreateNewMode.value) {
    data.source = new GroceryItemGetResponse();
    return;
  }

  const { id } = props;

  try {
    const response = await api().groceryItemsGet(id);
    data.source = response.data;
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    data.source = new GroceryItemGetResponse();
  }
}

let forceImmediateRouteChange = false;

async function onSaveClick() {
  try {
    const response = await api().groceryItemsSave(data.working);

    if (!isEditMode.value) {
      forceImmediateRouteChange = true;
      await router.push({ name: 'groceryItemEdit', params: { id: response.data.id } });
    } else {
      await fetch();
    }

    if (response.data.message) {
      messageStore.setSuccessMessage(response.data.message);
    }

    fetchStorageLocations();

    await groceryItemStore.fetchGroceryItemsList();
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

function onDeleteClick(id: number) {
  function deleteNow() {
    api()
      .groceryItemsDelete(id)
      .then(async (response) => {
        data.source = new GroceryItemGetResponse();
        await groceryItemStore.fetchGroceryItemsList();
        router
          .push({ name: 'groceryItemList', query: groceryItemStore.currentQueryParams })
          .then(() => {
            if (response.data.message) {
              messageStore.setSuccessMessage(response.data.message);
            }
          });
      })
      .catch((response) => {
        messageStore.setApiFailureMessages(response);
      });
  }

  const parameters: ModalParameters = {
    title: 'Delete grocery item',
    description: 'Do you really want to delete this grocery item?',
    okAction: deleteNow,
  };

  appStore.showModal(parameters);
}

function reset() {
  const sourceCopy: Record<string, unknown> = JSON.parse(JSON.stringify(data.source));

  const newWorkingClass = new GroceryItemWorking();

  const validProperties = Object.keys(newWorkingClass);

  // Remove properties that aren't in the request class.
  Object.keys(sourceCopy).forEach((key) => {
    if (!validProperties.includes(key)) {
      delete sourceCopy[key];
    }
  });

  const newWorking: GroceryItemWorking = {
    ...newWorkingClass,
    ...sourceCopy,
    groceryAisleId: data.source.groceryAisle?.id || null,
  };

  data.workingInitial = JSON.stringify(newWorking);

  data.working = newWorking;
}

function addStorageLocation(tag: string) {
  const storageLocationName = trimAndTitleCase(tag);

  const storageLocations = data.working.storageLocations?.slice() || [];

  const storageLocationDoesNotExist =
    storageLocations
      .map((value) => value.toUpperCase())
      .indexOf(storageLocationName.toUpperCase()) < 0;

  if (storageLocationDoesNotExist && storageLocationName.length > 0) {
    storageLocations.push(storageLocationName);
    data.working.storageLocations = storageLocations;
  }
}

function removeStorageLocation(storageLocationName: string) {
  const storageLocations = data.working.storageLocations?.slice() || [];
  const storageLocationIndex = storageLocations.indexOf(storageLocationName);

  if (storageLocationIndex > -1) {
    storageLocations.splice(storageLocationIndex, 1);
    data.working.storageLocations = storageLocations;
  }
}

function changeRouteFromModal(t: RouteLocationNormalized) {
  forceImmediateRouteChange = true;
  router.push(t).then(() => {
    forceImmediateRouteChange = false;
  });
}

function beforeRouteChange(
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
) {
  if (forceImmediateRouteChange) {
    next();
    return;
  }

  if (isDirty.value) {
    const parameters: ModalParameters = {
      title: 'Unsaved changes',
      description: 'You have unsaved changes. Do you really want to leave?',
      okAction: () => changeRouteFromModal(to),
    };

    next(false);
    appStore.showModal(parameters);
    return;
  }

  changeRouteFromModal(to);
  next();
}

watch(
  () => props.id,
  () => {
    fetch();
  },
  { immediate: true }
);

watch(
  () => data.source,
  () => {
    reset();
  },
  { immediate: true }
);

onBeforeRouteUpdate(async (to, from, next) => {
  beforeRouteChange(to, from, next);
});

onBeforeRouteLeave(async (to, from, next) => {
  beforeRouteChange(to, from, next);
});

function handleBeforeUnload(event: BeforeUnloadEvent) {
  if (isDirty.value) {
    event.preventDefault();
    // Required for Chrome to show the confirmation dialog
    // eslint-disable-next-line no-param-reassign
    event.returnValue = '';
    // Required for Firefox to show the confirmation dialog
    return '';
  }

  return null;
}

onMounted(() => {
  window.addEventListener('beforeunload', handleBeforeUnload);
});

onBeforeUnmount(() => {
  window.removeEventListener('beforeunload', handleBeforeUnload);
});
</script>

<template>
  <div class="container-xxl">
    <AppBreadcrumbs />
    <AppPageHeading />
    <div class="mt-3">
      <div class="btn-toolbar sticky-top pt-1">
        <button type="button" class="btn btn-primary me-2" @click.stop.prevent="onSaveClick()">
          Save
        </button>
        <div class="dropdown">
          <button
            v-if="isEditMode"
            id="overflowMenuButton"
            class="btn btn-secondary dropdown-toggle"
            type="button"
            data-bs-toggle="dropdown"
            aria-expanded="false"
          >
            More
          </button>
          <ul class="dropdown-menu" aria-labelledby="overflowMenuButton">
            <li>
              <button
                class="dropdown-item text-danger"
                @click.stop.prevent="onDeleteClick(data.working.id)"
              >
                Delete
              </button>
            </li>
          </ul>
        </div>
      </div>
      <div class="grid mt-3">
        <div class="g-col-12 g-col-md-6">
          <label for="name" class="form-label">Name</label>
          <input
            id="name"
            v-model="data.working.name"
            required
            type="text"
            :class="{
              'form-control': true,
              'is-invalid': messageStore.isFieldInError('name'),
            }"
          />
        </div>
        <div class="g-col-12 g-col-md-6">
          <label for="inventoryQuantity" class="form-label">Inventory</label>
          <input
            id="inventoryQuantity"
            v-model="data.working.inventoryQuantity"
            required
            type="number"
            min="0"
            :class="{
              'form-control': true,
              'is-invalid': messageStore.isFieldInError('inventoryQuantity'),
            }"
          />
        </div>
        <div class="g-col-12 g-col-md-6">
          <label for="groceryAisleId" class="form-label">Grocery aisle</label>
          <GroceryItemGroceryAisleSelect v-model="data.working.groceryAisleId" />
        </div>
        <TagEditor
          :class="{
            'g-col-12 g-col-md-6': true,
            danger: messageStore.isFieldInError('storageLocations'),
          }"
          :tags="(data.working.storageLocations || []).map((name) => ({ name }))"
          :on-add-tag="addStorageLocation"
          :on-remove-tag="removeStorageLocation"
          :suggestions="storageLocationOptions.map((name) => ({ name }))"
          field-name="storageLocations"
          label="Storage Locations"
        />
        <EntityAuditInfo v-if="data.source.id" class="g-col-12" :entity="data.source" />
        <div v-if="data.source.recipes?.length" class="g-col-12 g-col-md-6">
          Used in recipes
          <ul>
            <li v-for="recipe in data.source.recipes" :key="recipe.id">
              <router-link :to="RouterHelper.viewRecipe(recipe)">
                {{ recipe.name }}
              </router-link>
              |
              <router-link :to="RouterHelper.editRecipe(recipe)">Edit</router-link>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>

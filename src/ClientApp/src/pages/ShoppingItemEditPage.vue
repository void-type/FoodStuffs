<script lang="ts" setup>
import { computed, reactive, watch, onMounted, onBeforeUnmount, ref } from 'vue';
import ApiHelper from '@/models/ApiHelper';
import useAppStore from '@/stores/appStore';
import useMessageStore from '@/stores/messageStore';
import useShoppingItemStore from '@/stores/shoppingItemStore';
import { trimAndTitleCase, isNil } from '@/models/FormatHelper';
import AppBreadcrumbs from '@/components/AppBreadcrumbs.vue';
import AppPageHeading from '@/components/AppPageHeading.vue';
import ShoppingItemGetResponse from '@/models/ShoppingItemGetResponse';
import ShoppingItemWorking from '@/models/ShoppingItemWorking';
import type { HttpResponse } from '@/api/http-client';
import type { GetShoppingItemResponse } from '@/api/data-contracts';
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
import ShoppingItemGroceryDepartmentSelect from '@/components/ShoppingItemGroceryDepartmentSelect.vue';

const props = defineProps({
  id: {
    type: Number,
    required: false,
    default: 0,
  },
});

const data = reactive({
  source: new ShoppingItemGetResponse() as GetShoppingItemResponse,
  working: new ShoppingItemWorking(),
  // This is a snapshot of our source right after it became working so we can check if working is dirty.
  workingInitial: '',
});

const appStore = useAppStore();
const messageStore = useMessageStore();
const shoppingItemStore = useShoppingItemStore();
const api = ApiHelper.client;

// Editing existing
const isEditMode = computed(() => (props.id || 0) > 0);
// Create new
const isCreateNewMode = computed(() => !isEditMode.value);

const isDirty = computed(() => JSON.stringify(data.working) !== data.workingInitial);

const pantryLocationOptions = ref([] as Array<string>);

async function fetchPantryLocations() {
  try {
    const response = await api().pantryLocationsList({ isPagingEnabled: false });
    pantryLocationOptions.value =
      response.data.items?.map((x) => x.name || '').filter((x) => !isNil(x)) || [];
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

async function fetch() {
  if (isCreateNewMode.value) {
    data.source = new ShoppingItemGetResponse();
    return;
  }

  const { id } = props;

  try {
    const response = await api().shoppingItemsGet(id);
    data.source = response.data;
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
    data.source = new ShoppingItemGetResponse();
  }
}

let forceImmediateRouteChange = false;

async function onSaveClick() {
  try {
    const response = await api().shoppingItemsSave(data.working);

    if (!isEditMode.value) {
      forceImmediateRouteChange = true;
      await router.push({ name: 'shoppingItemEdit', params: { id: response.data.id } });
    } else {
      await fetch();
    }

    if (response.data.message) {
      messageStore.setSuccessMessage(response.data.message);
    }

    fetchPantryLocations();

    await shoppingItemStore.fetchShoppingItemsList();
  } catch (error) {
    messageStore.setApiFailureMessages(error as HttpResponse<unknown, unknown>);
  }
}

function onDeleteClick(id: number) {
  function deleteNow() {
    api()
      .shoppingItemsDelete(id)
      .then(async (response) => {
        data.source = new ShoppingItemGetResponse();
        await shoppingItemStore.fetchShoppingItemsList();
        router
          .push({ name: 'shoppingItemList', query: shoppingItemStore.currentQueryParams })
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

  const newWorkingClass = new ShoppingItemWorking();

  const validProperties = Object.keys(newWorkingClass);

  // Remove properties that aren't in the request class.
  Object.keys(sourceCopy).forEach((key) => {
    if (!validProperties.includes(key)) {
      delete sourceCopy[key];
    }
  });

  const newWorking: ShoppingItemWorking = {
    ...newWorkingClass,
    ...sourceCopy,
    groceryDepartmentId: data.source.groceryDepartment?.id || null,
  };

  data.workingInitial = JSON.stringify(newWorking);

  data.working = newWorking;
}

function addPantryLocation(tag: string) {
  const pantryLocationName = trimAndTitleCase(tag);

  const pantryLocations = data.working.pantryLocations?.slice() || [];

  const pantryLocationDoesNotExist =
    pantryLocations.map((value) => value.toUpperCase()).indexOf(pantryLocationName.toUpperCase()) <
    0;

  if (pantryLocationDoesNotExist && pantryLocationName.length > 0) {
    pantryLocations.push(pantryLocationName);
    data.working.pantryLocations = pantryLocations;
  }
}

function removePantryLocation(pantryLocationName: string) {
  const pantryLocations = data.working.pantryLocations?.slice() || [];
  const pantryLocationIndex = pantryLocations.indexOf(pantryLocationName);

  if (pantryLocationIndex > -1) {
    pantryLocations.splice(pantryLocationIndex, 1);
    data.working.pantryLocations = pantryLocations;
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
          <label for="groceryDepartmentId" class="form-label">Grocery Aisle</label>
          <ShoppingItemGroceryDepartmentSelect v-model="data.working.groceryDepartmentId" />
        </div>
        <TagEditor
          :class="{
            'g-col-12 g-col-md-6': true,
            danger: messageStore.isFieldInError('pantryLocations'),
          }"
          :tags="data.working.pantryLocations || []"
          :on-add-tag="addPantryLocation"
          :on-remove-tag="removePantryLocation"
          :suggestions="pantryLocationOptions"
          field-name="pantryLocations"
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

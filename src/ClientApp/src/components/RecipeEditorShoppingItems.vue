<script lang="ts" setup>
import { Collapse } from 'bootstrap';
import { ref, computed, nextTick, onMounted, onBeforeUnmount, type PropType } from 'vue';
import { VueDraggable } from 'vue-draggable-plus';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { clamp } from '@/models/FormatHelper';
import RecipeShoppingItemWorking from '@/models/RecipeShoppingItemWorking';
import type { ListShoppingItemsResponse } from '@/api/data-contracts';
import RecipeEditorShoppingItemSelect from './RecipeEditorShoppingItemSelect.vue';
import AppSortHandle from './AppSortHandle.vue';
import ShoppingItemInventoryQuantity from './ShoppingItemInventoryQuantity.vue';

const model = defineModel({
  type: Array as PropType<Array<RecipeShoppingItemWorking>>,
  required: true,
});

const props = defineProps({
  isFieldInError: {
    type: Function,
    required: true,
  },
  suggestions: {
    type: Array as PropType<Array<ListShoppingItemsResponse>>,
    required: false,
    default: () => [],
  },
  onCreateItem: {
    type: Function,
    required: true,
  },
});

function showInAccordion(index: number, focus: boolean = false) {
  const safeIndex = clamp(index, 0, model.value.length - 1);
  const item = model.value[safeIndex];

  if (item) {
    const elementId = `#item-${item.uiKey}`;
    nextTick(() => {
      Collapse.getOrCreateInstance(`${elementId}-accordion-collapse`, { toggle: false }).show();

      if (focus) {
        const nameInput = document.querySelector(`${elementId}-name`) as HTMLElement;
        if (nameInput !== null) {
          nameInput.focus();
        }
      }
    });
  }
}

function onNewClick(itemId: number | null) {
  const newItem = new RecipeShoppingItemWorking();
  newItem.order = model.value.length + 1;

  if (itemId !== null) {
    newItem.id = itemId;
  }

  const newLength = model.value.push(newItem);

  showInAccordion(newLength - 1, itemId === null);
}

function updateOrdersByIndex() {
  model.value.forEach((x, i) => {
    // eslint-disable-next-line no-param-reassign
    x.order = i + 1;
  });
}

function onDeleteClick(uiKey: string) {
  const index = model.value.findIndex((x) => x.uiKey === uiKey);
  model.value.splice(index, 1);
  updateOrdersByIndex();
  showInAccordion(index);
}

const usedIds = computed(() => model.value.map((x) => x.id));

function getShoppingItem(id: number | undefined) {
  return props.suggestions.find((x) => x.id === id);
}

function onSortEnd() {
  nextTick(() => {
    updateOrdersByIndex();
  });
}

function onShow(event: Event) {
  if (event.target instanceof HTMLElement) {
    const target = event.target as HTMLElement;
    const focusableElement = target.querySelector('.first-form-item') as HTMLElement;
    if (focusableElement) {
      focusableElement.focus();
    }
  }
}

const shoppingItemList = ref<HTMLElement | null>(null);

onMounted(() => {
  if (shoppingItemList.value) {
    shoppingItemList.value.addEventListener('shown.bs.collapse', onShow);
  }
});

onBeforeUnmount(() => {
  if (shoppingItemList.value) {
    shoppingItemList.value.removeEventListener('shown.bs.collapse', onShow);
  }
});
</script>

<template>
  <div ref="shoppingItemList">
    <div v-if="model.length < 1" id="shopping-item-list" class="card p-4 text-center border">
      No grocery items.
    </div>
    <vue-draggable
      v-else
      id="shopping-item-list"
      v-model="model"
      :animation="200"
      group="item"
      ghost-class="ghost"
      handle=".sort-handle"
      class="accordion"
      @end="onSortEnd"
    >
      <div v-for="item in model" :key="item.uiKey" class="accordion-item">
        <div :id="`item-${item.uiKey}-accordion-header`" class="h2 accordion-header">
          <button
            class="accordion-button collapsed"
            type="button"
            data-bs-toggle="collapse"
            :data-bs-target="`#item-${item.uiKey}-accordion-collapse`"
            aria-expanded="false"
            :aria-controls="`item-${item.uiKey}-accordion-collapse`"
          >
            <AppSortHandle class="pe-3" />
            {{ item.quantity }}x {{ getShoppingItem(item.id)?.name }}
          </button>
        </div>
        <div
          :id="`item-${item.uiKey}-accordion-collapse`"
          class="accordion-collapse collapse"
          :aria-labelledby="`item-${item.uiKey}-accordion-header`"
          data-bs-parent="#shopping-item-list"
        >
          <div class="grid p-3 gap-sm">
            <div class="g-col-12 g-col-md-12">
              <label :for="`item-${item.uiKey}-name`" class="form-label">Grocery item</label>
              <RecipeEditorShoppingItemSelect
                v-model="item.id"
                :is-field-in-error="(name: string) => isFieldInError(`shoppingItems-${name}`)"
                :item="item"
                :item-name="getShoppingItem(item.id)?.name"
                :suggestions="suggestions"
                :used-ids="usedIds"
                :on-create-item="onCreateItem"
              />
            </div>
            <div class="g-col-12 g-col-md-6">
              <label :for="`item-${item.uiKey}-quantity`" class="form-label">Quantity</label>
              <input
                :id="`item-${item.uiKey}-quantity`"
                v-model="item.quantity"
                required
                type="number"
                min="1"
                :class="{
                  'form-control': true,
                  'is-invalid': isFieldInError('shoppingItems'),
                }"
              />
            </div>
            <ShoppingItemInventoryQuantity
              :id="`item-${item.uiKey}-inventoryQuantity`"
              v-model="item.inventoryQuantity"
              class="g-col-12 g-col-md-6"
              :item="item"
            />
            <div class="btn-toolbar g-col-12">
              <router-link
                :to="{ name: 'shoppingItemEdit', params: { id: item.id } }"
                class="btn btn-secondary btn-sm"
                >Edit</router-link
              >
              <button
                type="button"
                class="btn btn-danger btn-sm ms-auto"
                @click.stop.prevent="onDeleteClick(item.uiKey)"
              >
                Delete
              </button>
            </div>
          </div>
        </div>
      </div>
    </vue-draggable>
    <button
      type="button"
      class="btn btn-secondary btn-sm btn-add-item"
      aria-label="add grocery item"
      @click.stop.prevent="onNewClick(null)"
    >
      <font-awesome-icon icon="fa-plus" />
    </button>
  </div>
</template>

<style lang="scss" scoped>
.accordion {
  .accordion-button {
    padding: 0.75em;
  }
}

div#shopping-item-list,
div#shopping-item-list .accordion-item:last-of-type {
  border-bottom-left-radius: 0;
}

.btn.btn-sm.btn-add-item {
  min-width: 4rem;
  border-top-left-radius: 0;
  border-top-right-radius: 0;
}

.ghost {
  background: var(--bs-accordion-active-bg);
}
</style>

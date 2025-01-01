<script lang="ts" setup>
import { Collapse } from 'bootstrap';
import { computed, nextTick, ref, type PropType } from 'vue';
import { VueDraggable } from 'vue-draggable-plus';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { clamp } from '@/models/FormatHelpers';
import WorkingRecipeShoppingItem from '@/models/WorkingRecipeShoppingItem';
import type { ListShoppingItemsResponse } from '@/api/data-contracts';
import RecipeEditorShoppingItemSelect from './RecipeEditorShoppingItemSelect.vue';

const model = defineModel({
  type: Array as PropType<Array<WorkingRecipeShoppingItem>>,
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

const newShoppingItemName = ref('');

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
  const newItem = new WorkingRecipeShoppingItem();
  newItem.order = model.value.length + 1;

  if (itemId !== null) {
    newItem.id = itemId;
  }

  const newLength = model.value.push(newItem);

  showInAccordion(newLength - 1, itemId === null);
}

async function createShoppingItem(name: string) {
  const itemId = await props.onCreateItem(name);

  if (itemId !== null) {
    newShoppingItemName.value = '';
    onNewClick(itemId);
  }
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
</script>

<template>
  <div>
    <label for="shoppingItemName" class="form-label visually-hidden">Create a shopping item</label>
    <div class="input-group">
      <input
        id="shoppingItemName"
        v-model="newShoppingItemName"
        name="shoppingItemName"
        :class="{ 'form-control': true, 'is-invalid': isFieldInError('shoppingItemName') }"
        placeholder="Create a shopping item"
        @keydown.stop.prevent.enter="createShoppingItem(newShoppingItemName)"
      />
      <button
        class="btn btn-secondary"
        type="button"
        @click.stop.prevent="createShoppingItem(newShoppingItemName)"
      >
        Save
      </button>
    </div>
    <div v-if="model.length < 1" id="shopping-item-list" class="card p-4 text-center">
      No shopping items.
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
            <span class="pe-3 sort-handle">
              <div class="visually-hidden">Drag to sort</div>
              <font-awesome-icon icon="fa-grip-lines" class="text-muted"
            /></span>
            <span>{{ item.quantity }}x {{ getShoppingItem(item.id)?.name }}</span>
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
              <label :for="`item-${item.uiKey}-name`" class="form-label">Shopping item</label>
              <RecipeEditorShoppingItemSelect
                v-model="item.id"
                :item="item"
                :item-name="getShoppingItem(item.id)?.name"
                :suggestions="suggestions"
                :used-ids="usedIds"
              />
            </div>
            <div class="g-col-12 g-col-md-4">
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
            <div class="btn-toolbar g-col-12">
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
      aria-label="add shopping item"
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

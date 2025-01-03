<script lang="ts" setup>
import { Collapse } from 'bootstrap';
import { nextTick, type PropType } from 'vue';
import { VueDraggable } from 'vue-draggable-plus';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { clamp } from '@/models/FormatHelpers';
import WorkingRecipeIngredient from '@/models/WorkingRecipeIngredient';
import AppSortHandle from './AppSortHandle.vue';

const model = defineModel({
  type: Array as PropType<Array<WorkingRecipeIngredient>>,
  required: true,
});

defineProps({
  isFieldInError: {
    type: Function,
    required: true,
  },
});

function showInAccordion(index: number, focus: boolean = false) {
  const safeIndex = clamp(index, 0, model.value.length - 1);
  const item = model.value[safeIndex];

  if (item) {
    const elementId = `#ingredient-${item.uiKey}`;
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

function onNewClick() {
  const newItem = new WorkingRecipeIngredient();
  newItem.name = '';
  newItem.quantity = 1;
  newItem.order = model.value.length + 1;
  newItem.isCategory = false;

  const newLength = model.value.push(newItem);

  showInAccordion(newLength - 1, true);
}

function updateOrdersByIndex() {
  model.value.forEach((x, i) => {
    // eslint-disable-next-line no-param-reassign
    x.order = i + 1;
  });
}

function onDeleteClick(id: string) {
  const index = model.value.findIndex((x) => x.uiKey === id);
  model.value.splice(index, 1);
  updateOrdersByIndex();
  showInAccordion(index);
}

function onSortEnd() {
  nextTick(() => {
    updateOrdersByIndex();
  });
}
</script>

<template>
  <div v-if="model.length < 1" id="ingredient-list" class="card p-4 text-center">
    No ingredients.
  </div>
  <vue-draggable
    v-else
    id="ingredient-list"
    v-model="model"
    :animation="200"
    group="ingredients"
    ghost-class="ghost"
    handle=".sort-handle"
    class="accordion"
    @end="onSortEnd"
  >
    <div v-for="ing in model" :key="ing.uiKey" class="accordion-item">
      <div :id="`ingredient-${ing.uiKey}-accordion-header`" class="h2 accordion-header">
        <button
          class="accordion-button collapsed"
          type="button"
          data-bs-toggle="collapse"
          :data-bs-target="`#ingredient-${ing.uiKey}-accordion-collapse`"
          aria-expanded="false"
          :aria-controls="`ingredient-${ing.uiKey}-accordion-collapse`"
        >
          <AppSortHandle class="pe-3" />
          <span v-if="ing.isCategory" class="fw-bold">{{ ing.name }}</span>
          <span v-else>{{ ing.quantity }}x {{ ing.name }}</span>
        </button>
      </div>
      <div
        :id="`ingredient-${ing.uiKey}-accordion-collapse`"
        class="accordion-collapse collapse"
        :aria-labelledby="`ingredient-${ing.uiKey}-accordion-header`"
        data-bs-parent="#ingredient-list"
      >
        <div class="grid p-3 gap-sm">
          <div class="g-col-12 g-col-md-12">
            <label :for="`ingredient-${ing.uiKey}-name`" class="form-label">Name</label>
            <input
              :id="`ingredient-${ing.uiKey}-name`"
              v-model="ing.name"
              required
              type="text"
              :class="{
                'form-control': true,
                'is-invalid': isFieldInError('ingredients'),
              }"
              @keydown.stop.prevent.enter
            />
          </div>
          <div v-if="!ing.isCategory" class="g-col-12 g-col-md-4">
            <label :for="`ingredient-${ing.uiKey}-quantity`" class="form-label">Quantity</label>
            <input
              :id="`ingredient-${ing.uiKey}-quantity`"
              v-model="ing.quantity"
              required
              type="number"
              min="1"
              :class="{
                'form-control': true,
                'is-invalid': isFieldInError('ingredients'),
              }"
            />
          </div>
          <div class="g-col-12">
            <div class="form-check">
              <input
                :id="`ingredient-${ing.uiKey}-isCategory`"
                v-model="ing.isCategory"
                class="form-check-input"
                type="checkbox"
                :class="{ 'is-invalid': isFieldInError('ingredients') }"
              />
              <label :for="`ingredient-${ing.uiKey}-isCategory`" class="form-check-label"
                >Is Category</label
              >
            </div>
          </div>
          <div class="btn-toolbar g-col-12">
            <button
              type="button"
              class="btn btn-danger btn-sm ms-auto"
              @click.stop.prevent="onDeleteClick(ing.uiKey)"
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
    class="btn btn-secondary btn-sm btn-add-ingredient"
    aria-label="add ingredient"
    @click.stop.prevent="onNewClick()"
  >
    <font-awesome-icon icon="fa-plus" />
  </button>
</template>

<style lang="scss" scoped>
.accordion {
  .accordion-button {
    padding: 0.75em;
  }
}

div#ingredient-list,
div#ingredient-list .accordion-item:last-of-type {
  border-bottom-left-radius: 0;
}

.btn.btn-sm.btn-add-ingredient {
  min-width: 4rem;
  border-top-left-radius: 0;
  border-top-right-radius: 0;
}

.ghost {
  background: var(--bs-accordion-active-bg);
}
</style>

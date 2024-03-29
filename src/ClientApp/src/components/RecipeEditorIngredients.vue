<script lang="ts" setup>
import { Collapse } from 'bootstrap';
import { nextTick, reactive, watch, type PropType } from 'vue';
import { VueDraggable } from 'vue-draggable-plus';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { clamp } from '@/models/FormatHelpers';
import WorkingRecipeIngredient from '@/models/WorkingRecipeIngredient';

const props = defineProps({
  modelValue: {
    type: Object as PropType<Array<WorkingRecipeIngredient>>,
    required: true,
  },
  isFieldInError: {
    type: Function,
    required: true,
  },
});

const emit = defineEmits(['update:modelValue']);

const data = reactive({
  ingredients: [] as WorkingRecipeIngredient[],
});

function copy(ingredients: WorkingRecipeIngredient[]) {
  return JSON.parse(JSON.stringify(ingredients)) as WorkingRecipeIngredient[];
}

function showInAccordion(index: number, focus: boolean = false) {
  const safeIndex = clamp(index, 0, data.ingredients.length - 1);
  const ingredient = data.ingredients[safeIndex];

  if (ingredient) {
    const elementId = `#ingredient-${ingredient.id}`;
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
  const ingredients = copy(data.ingredients);

  const newLength = ingredients.push({
    ...new WorkingRecipeIngredient(),
    name: '',
    quantity: 1,
    order: Math.max(...ingredients.map((x) => x.order || 0)) + 1,
    isCategory: false,
  });

  data.ingredients = ingredients;
  showInAccordion(newLength - 1, true);
}

function onDeleteClick(id: string) {
  const ingredients = copy(data.ingredients);

  const index = ingredients.findIndex((x) => x.id === id);
  ingredients.splice(index, 1);

  data.ingredients = ingredients;
  showInAccordion(index);
}

function setOrderFromIndex(ingredients: WorkingRecipeIngredient[]) {
  ingredients.forEach((x, i) => {
    // eslint-disable-next-line no-param-reassign
    x.order = i + 1;
  });
}

// When props change, we'll update the working version.
watch(props, () => {
  const ingredients = copy(props.modelValue);

  ingredients.sort((a, b) => (a.order || 0) - (b.order || 0));
  setOrderFromIndex(ingredients);

  // Deep compare to prevent circular changes.
  if (JSON.stringify(data.ingredients) !== JSON.stringify(ingredients)) {
    data.ingredients = ingredients;
  }
});

// When data changes, we'll emit the new model.
watch(data, (newValue) => {
  const ingredients = copy(newValue.ingredients);

  setOrderFromIndex(ingredients);

  // Deep compare to prevent circular changes.
  if (JSON.stringify(props.modelValue) !== JSON.stringify(ingredients)) {
    emit('update:modelValue', ingredients);
  }
});
</script>

<template>
  <div v-if="data.ingredients.length < 1" id="ingredient-list" class="card p-4 text-center">
    No ingredients
  </div>
  <vue-draggable
    v-else
    id="ingredient-list"
    v-model="data.ingredients"
    :animation="200"
    group="ingredients"
    ghost-class="ghost"
    handle=".sort-handle"
    class="accordion"
  >
    <div v-for="ing in data.ingredients" :key="ing.id" class="accordion-item">
      <div :id="`ingredient-${ing.id}-accordion-header`" class="h2 accordion-header">
        <button
          class="accordion-button collapsed"
          type="button"
          data-bs-toggle="collapse"
          :data-bs-target="`#ingredient-${ing.id}-accordion-collapse`"
          aria-expanded="false"
          :aria-controls="`ingredient-${ing.id}-accordion-collapse`"
        >
          <span class="pe-3 sort-handle">
            <div class="visually-hidden">Drag to sort</div>
            <font-awesome-icon icon="fa-grip-lines" class="text-muted"
          /></span>
          <span v-if="ing.isCategory" class="fw-bold">{{ ing.name }}</span>
          <span v-else>{{ ing.quantity }}x {{ ing.name }}</span>
        </button>
      </div>
      <div
        :id="`ingredient-${ing.id}-accordion-collapse`"
        class="accordion-collapse collapse"
        :aria-labelledby="`ingredient-${ing.id}-accordion-header`"
        data-bs-parent="#ingredient-list"
      >
        <div class="grid p-3 gap-sm">
          <div class="g-col-12 g-col-md-12">
            <label :for="`ingredient-${ing.id}-name`" class="form-label">Name</label>
            <input
              :id="`ingredient-${ing.id}-name`"
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
            <label :for="`ingredient-${ing.id}-quantity`" class="form-label">Quantity</label>
            <input
              :id="`ingredient-${ing.id}-quantity`"
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
                :id="`ingredient-${ing.id}-isCategory`"
                v-model="ing.isCategory"
                class="form-check-input"
                type="checkbox"
                :class="{ 'is-invalid': isFieldInError('ingredients') }"
              />
              <label :for="`ingredient-${ing.id}-isCategory`" class="form-check-label"
                >Is Category</label
              >
            </div>
          </div>
          <div class="btn-toolbar g-col-12">
            <button
              type="button"
              class="btn btn-danger btn-sm d-inline ms-auto"
              @click.stop.prevent="onDeleteClick(ing.id)"
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
    class="btn btn-outline-light btn-sm btn-add-ingredient"
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

  .sort-handle {
    cursor: grab;
  }
}

div#ingredient-list,
div#ingredient-list .accordion-item:last-of-type {
  border-bottom-left-radius: 0;
}

.btn.btn-sm.btn-add-ingredient {
  min-width: 3rem;
  border-top-left-radius: 0;
  border-top-right-radius: 0;
}

.ghost {
  background: var(--bs-accordion-active-bg);
}
</style>

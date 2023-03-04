<script lang="ts" setup>
import { Collapse } from 'bootstrap';
import { nextTick, reactive, watch, type PropType } from 'vue';
import { Sortable } from 'sortablejs-vue3';
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

function showInAccordion(index: number) {
  const safeIndex = clamp(index, 0, data.ingredients.length - 1);
  const ingredient = data.ingredients[safeIndex];

  if (ingredient) {
    const element = `#ingredient-${ingredient.id}-accordion-collapse`;
    nextTick(() => Collapse.getOrCreateInstance(element).show());
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
  showInAccordion(newLength - 1);
}

function onDeleteClick(index: number) {
  const ingredients = copy(data.ingredients);
  ingredients.splice(index, 1);
  data.ingredients = ingredients;

  showInAccordion(index);
}

function onSortEnd(event: any) {
  if (event.oldIndex === event.newIndex) {
    return;
  }

  const ingredients = copy(data.ingredients);
  const item = ingredients.splice(event.oldIndex, 1)[0];
  ingredients.splice(event.newIndex, 0, item);
  data.ingredients = ingredients;
}

function setOrderFromIndex(ingredients: WorkingRecipeIngredient[]) {
  ingredients.forEach((x, i) => {
    // eslint-disable-next-line no-param-reassign
    x.order = i + 1;
  });
}

watch(props, () => {
  const ingredients = copy(props.modelValue);

  ingredients.sort((a, b) => (a.order || 0) - (b.order || 0));
  setOrderFromIndex(ingredients);

  // Deep compare to prevent circular changes.
  if (JSON.stringify(data.ingredients) !== JSON.stringify(ingredients)) {
    data.ingredients = ingredients;
  }
});

watch(data, (newValue) => {
  const { ingredients } = newValue;
  setOrderFromIndex(ingredients);
  emit('update:modelValue', ingredients);
});
</script>

<template>
  <div>
    <div id="ingredient-accordion" class="accordion">
      <Sortable :list="data.ingredients" item-key="id" @end="onSortEnd">
        <template #item="{ element, index }">
          <div :key="element.id" class="accordion-item sortable-draggable">
            <h2 :id="`ingredient-${element.id}-accordion-header`" class="accordion-header">
              <button
                class="accordion-button collapsed"
                type="button"
                data-bs-toggle="collapse"
                :data-bs-target="`#ingredient-${element.id}-accordion-collapse`"
                aria-expanded="false"
                :aria-controls="`ingredient-${element.id}-accordion-collapse`"
              >
                <font-awesome-icon icon="fa-sort" class="text-muted me-3" />
                <span v-if="element.isCategory" class="fw-bold">{{ element.name }}</span>
                <span v-else>{{ element.quantity }}x {{ element.name }}</span>
              </button>
            </h2>
            <div
              :id="`ingredient-${element.id}-accordion-collapse`"
              class="accordion-collapse collapse"
              :aria-labelledby="`ingredient-${element.id}-accordion-header`"
              data-bs-parent="#ingredient-accordion"
            >
              <div class="grid p-3" style="--bs-gap: 1em">
                <div class="g-col-12 g-col-md-12">
                  <label :for="`ingredient-${element.id}-name`" class="form-label">Name</label>
                  <input
                    :id="`ingredient-${element.id}-name`"
                    v-model="element.name"
                    required
                    type="text"
                    :class="{
                      'form-control': true,
                      'is-invalid': isFieldInError('ingredients'),
                    }"
                  />
                </div>
                <div v-if="!element.isCategory" class="g-col-12 g-col-md-4">
                  <label :for="`ingredient-${element.id}-quantity`" class="form-label"
                    >Quantity</label
                  >
                  <input
                    :id="`ingredient-${element.id}-quantity`"
                    v-model="element.quantity"
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
                      :id="`ingredient-${element.id}-isCategory`"
                      v-model="element.isCategory"
                      class="form-check-input"
                      type="checkbox"
                      :class="{ 'is-invalid': isFieldInError('ingredients') }"
                    />
                    <label :for="`ingredient-${element.id}-isCategory`" class="form-check-label"
                      >Is Category</label
                    >
                  </div>
                </div>
                <div class="btn-toolbar g-col-12">
                  <button
                    class="btn btn-danger btn-sm d-inline ms-auto"
                    @click.stop.prevent="onDeleteClick(index)"
                  >
                    Delete
                  </button>
                </div>
              </div>
            </div>
          </div>
        </template>
      </Sortable>
    </div>
    <div class="btn-toolbar">
      <button class="btn btn-secondary btn-sm me-2 mt-3" @click.stop.prevent="onNewClick()">
        New ingredient
      </button>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.accordion-button {
  padding: 0.75em;
}

.accordion .sortable-draggable {
  &,
  .fa-sort {
    cursor: grab;
  }
}
</style>

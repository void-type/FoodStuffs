<script lang="ts" setup>
import ApiHelper from '@/models/ApiHelper';
import { ref, onMounted, type PropType } from 'vue';
import useMessageStore from '@/stores/messageStore';
import { type ListStorageLocationsResponse, type SearchFacetValue } from '@/api/data-contracts';
import { toNumberOrNull } from '@/models/FormatHelper';

const props = defineProps({
  facetValues: {
    type: Array<SearchFacetValue>,
    required: false,
    default: [],
  },
  parentAccordionId: {
    type: String,
    required: false,
    default: 'filterAccordion',
  },
  checkClass: {
    type: String,
    required: false,
    default: 'g-col-6 g-col-md-4',
  },
});

const model = defineModel({
  type: Object as PropType<{ storageLocations: Array<number>; matchAllStorageLocations: boolean }>,
  required: true,
  default: [],
});

const messageStore = useMessageStore();
const api = ApiHelper.client;

const storageLocationOptions = ref([] as Array<ListStorageLocationsResponse>);

function selectAll() {
  model.value.storageLocations = storageLocationOptions.value.flatMap((x) => {
    const n = toNumberOrNull(x.id);
    return n ? [n] : [];
  });
}

function getFacetCount(facetValue: number | null | undefined) {
  if (facetValue === null || typeof facetValue === 'undefined') {
    return null;
  }

  const count = props.facetValues?.find((x) => x.fieldValue === facetValue.toString())?.count || 0;

  return ` (${count})`;
}

onMounted(() => {
  api()
    .storageLocationsList({ isPagingEnabled: false })
    .then((response) => {
      storageLocationOptions.value = response.data.items || [];
    })
    .catch((response) => messageStore.setApiFailureMessages(response));
});
</script>

<template>
  <div class="accordion-item">
    <div class="accordion-header">
      <button
        class="accordion-button collapsed"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#storageLocationsCollapse"
        aria-expanded="false"
        aria-controls="storageLocationsCollapse"
      >
        <label for="storageLocationSearch"
          >Storage Locations
          <span v-if="model.storageLocations.length">
            ({{ model.storageLocations.length }} selected)
          </span>
        </label>
      </button>
    </div>
    <div
      id="storageLocationsCollapse"
      class="accordion-collapse collapse"
      :data-bs-parent="`#${props.parentAccordionId}`"
    >
      <div class="accordion-body">
        <div class="btn-toolbar mb-3">
          <button
            v-if="model.storageLocations.length"
            class="btn btn-sm btn-secondary me-2"
            @click.stop.prevent="model.storageLocations = []"
          >
            Select none
          </button>
          <button v-else class="btn btn-sm btn-secondary me-2" @click.stop.prevent="selectAll">
            Select all
          </button>
          <div class="form-check form-switch my-auto">
            <label
              class="w-100"
              for="matchAllStorageLocations"
              aria-label="Match all selected storage locations"
              >Match all</label
            >
            <input
              id="matchAllStorageLocations"
              v-model="model.matchAllStorageLocations"
              :checked="model.matchAllStorageLocations"
              class="form-check-input"
              type="checkbox"
            />
          </div>
        </div>
        <div class="grid slim-scroll storage-location-scroll">
          <div
            v-for="storageLocationOption in storageLocationOptions"
            :key="storageLocationOption.id"
            :class="`${checkClass} form-check m-0`"
          >
            <input
              :id="`storageLocation-${storageLocationOption.id}`"
              v-model.lazy.number="model.storageLocations"
              class="form-check-input"
              type="checkbox"
              :value="storageLocationOption.id"
            />
            <label class="form-check-label" :for="`storageLocation-${storageLocationOption.id}`">
              {{ storageLocationOption.name }}{{ getFacetCount(storageLocationOption.id) }}
            </label>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.storage-location-scroll {
  row-gap: 0.1rem;
  column-gap: 0;
}
</style>

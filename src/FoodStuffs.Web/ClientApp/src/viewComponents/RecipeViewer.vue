<template>
  <div
    v-if="recipe.name"
    class="viewer"
  >
    <b-row
      class="no-print"
    >
      <b-col
        cols="12"
      >
        <b-button-toolbar>
          <b-button
            :to="{name: 'edit', params: {id: recipe.id}}"
            class="mr-2"
            variant="primary"
          >
            Edit
          </b-button>
          <b-form-checkbox
            id="showImage"
            v-model="showImage"
            name="showImage"
            class="mt-2"
            switch
          >
            Image
          </b-form-checkbox>
        </b-button-toolbar>
      </b-col>
    </b-row>
    <b-row>
      <b-col
        v-if="showImage"
        cols="12"
        class="text-center mt-3"
      >
        <b-carousel
          v-if="recipe.images.length > 0"
          id="image-carousel"
          v-model="carouselIndex"
          :interval="0"
          no-animation
          controls
          indicators
        >
          <b-carousel-slide
            v-for="image in recipe.images"
            :key="image"
          >
            <template v-slot:img>
              <b-img
                fluid
                rounded
                :src="imageUrl(image)"
              />
            </template>
          </b-carousel-slide>
        </b-carousel>
      </b-col>
    </b-row>
    <h3
      class="mt-3"
    >
      Ingredients
    </h3>
    <b-form-textarea
      plaintext
      rows="1"
      :max-rows="Number.MAX_SAFE_INTEGER"
      :value="recipe.ingredients"
    />
    <h3
      class="mt-3"
    >
      Directions
    </h3>
    <b-form-textarea
      plaintext
      rows="1"
      :max-rows="Number.MAX_SAFE_INTEGER"
      :value="recipe.directions"
    />
    <h3
      class="mt-3"
    >
      Stats
    </h3>
    <div v-if="recipe.prepTimeMinutes > 0">
      Prep Time: {{ recipe.prepTimeMinutes | TimeSpan }}
    </div>
    <div v-if="recipe.cookTimeMinutes > 0">
      Cook Time: {{ recipe.cookTimeMinutes | TimeSpan }}
    </div>
    <div v-if="recipe.categories.length > 0">
      Categories: {{ recipe.categories.join(', ') }}
    </div>
    <div>
      <EntityDetailsAuditInfo
        class="mt-3"
        :entity="recipe"
      />
    </div>
  </div>
</template>

<script>
import webApi from '../webApi';
import RecipeTimeSpan from '../models/RecipeTimeSpan';
import EntityDetailsAuditInfo from './EntityDetailsAuditInfo.vue';

export default {
  components: {
    EntityDetailsAuditInfo,
  },
  filters: {
    TimeSpan(value) {
      return new RecipeTimeSpan(value).toString();
    },
  },
  props: {
    recipe: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      showImage: true,
      carouselIndex: 0,
    };
  },
  methods: {
    imageUrl(id) {
      return webApi.images.url(id);
    },
  },
};
</script>

<style lang="scss" scoped>
textarea {
  overflow: hidden !important;
  resize: none;
}

// Don't print carousel controls.
@media print {
  #image-carousel ::v-deep {
    & a,
    ol {
      display: none;
    }
  }
}
</style>

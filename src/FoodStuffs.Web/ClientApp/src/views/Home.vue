<template>
  <b-container>
    <b-form-row
      class="mt-3"
    >
      <b-col
        v-for="recipe in recipes"
        :key="recipe.id"
        class="mt-3"
        lg="4"
      >
        <b-card
          no-body
          class="overflow-hidden"
        >
          <router-link
            class="card-link"
            :to="{ name: 'view', params: { id: recipe.id } }"
          >
            <div
              v-if="recipe.imageId != null"
              cols="4"
            >
              <b-card-img
                fluid
                :src="imageUrl(recipe.imageId)"
                alt="recipe image"
              />
            </div>
            <div
              class="p-3"
            >
              <b-card-title>
                {{ recipe.name }}
              </b-card-title>
              <b-card-text>
                {{ recipe.categories.join(", ") }}
              </b-card-text>
            </div>
          </router-link>
        </b-card>
      </b-col>
    </b-form-row>
  </b-container>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import webApi from '../webApi';

export default {
  computed: {
    ...mapGetters({
      listResponse: 'recipes/listResponse',
      listRequest: 'recipes/listRequest',
    }),
    recipes() {
      return this.listResponse.items.slice(0, 12);
    },
  },
  created() {
    if (this.listResponse.count === 0) {
      webApi.recipes.list(
        this.listRequest,
        (data) => this.setListResponse(data),
        (response) => this.setApiFailureMessages(response),
      );
    }
  },
  methods: {
    ...mapActions({
      setApiFailureMessages: 'app/setApiFailureMessages',
      setListResponse: 'recipes/setListResponse',
    }),
    imageUrl(id) {
      return webApi.images.url(id);
    },
  },
};
</script>

<style lang="scss" scoped>
.card-link {
  text-decoration: none;
}
</style>

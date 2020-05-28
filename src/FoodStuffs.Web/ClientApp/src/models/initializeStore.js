import store from '../store';
import webApi from '../webApi';

export default function () {
  webApi.app.getInfo(
    data => store.dispatch('app/setApplicationInfo', data),
    response => store.dispatch('app/setApiFailureMessages', response),
  );

  webApi.recipes.list(
    store.getters['recipes/listRequest'],
    data => store.dispatch('recipes/setListResponse', data),
    response => store.dispatch('app/setApiFailureMessages', response),
  );
}

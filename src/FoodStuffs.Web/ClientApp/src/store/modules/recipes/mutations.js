import recipesApiModels from '../../../models/recipesApiModels';

/* eslint-disable no-param-reassign */
export default {
  SET_LIST_RESPONSE(state, listResponse) {
    state.listResponse = listResponse;
  },
  RESET_LIST_REQUEST(state) {
    state.listRequest = new recipesApiModels.ListRequest();
  },
  SET_LIST_REQUEST_IS_PAGING_ENABLED(state, isEnabled) {
    state.listRequest.isPagingEnabled = isEnabled;
  },
  SET_LIST_REQUEST_PAGE(state, page) {
    state.listRequest.page = page;
  },
  SET_LIST_REQUEST_TAKE(state, take) {
    state.listRequest.take = take;
  },
  SET_LIST_REQUEST_CATEGORY_SEARCH(state, categorySearch) {
    state.listRequest.categorySearch = categorySearch;
  },
  SET_LIST_REQUEST_NAME_SEARCH(state, nameSearch) {
    state.listRequest.nameSearch = nameSearch;
  },
  SET_LIST_REQUEST_SORT(state, sortName) {
    state.listRequest.sort = sortName;
  },
  SET_RECENT_RECIPES(state, recent) {
    state.recent = recent;
  },
};
/* eslint-enable no-param-reassign */

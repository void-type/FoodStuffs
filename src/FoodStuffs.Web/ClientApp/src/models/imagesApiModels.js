class DeleteRequest {
  constructor() {
    this.id = 0;
  }
}

class SaveRequest {
  constructor() {
    this.recipeId = 0;
    this.file = null;
  }
}

export default {
  DeleteRequest,
  SaveRequest,
};

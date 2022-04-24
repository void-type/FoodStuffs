export default class ApiHelpers {
  static imageUrl(id: number | string) {
    return `/api/images/${id}`;
  }
}

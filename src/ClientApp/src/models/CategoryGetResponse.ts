import type { GetCategoryResponse, GetCategoryResponseRecipe } from '@/api/data-contracts';

export default class CategoryGetResponse implements GetCategoryResponse {
  public id = 0;

  public name: string = '';

  public createdBy: string = '';

  public createdOn = '';

  public modifiedBy: string = '';

  public modifiedOn = '';

  public recipes: GetCategoryResponseRecipe[] = [];
}

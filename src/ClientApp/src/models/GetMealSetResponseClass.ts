import type { GetMealSetResponse, GetRecipeResponse } from '@/api/data-contracts';

export default class GetMealSetResponseClass implements GetMealSetResponse {
  public id = 0;

  public name: string | null = '';

  public createdBy: string | null = '';

  public createdOn = '';

  public modifiedBy: string | null = '';

  public modifiedOn = '';

  public recipes: GetRecipeResponse[] = [];
}

/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface WebClientInfo {
  antiforgeryToken?: string;
  antiforgeryTokenHeaderName?: string;
  applicationName?: string;
  user?: DomainUser;
}

export interface DomainUser {
  login?: string;
  authorizedAs?: string[];
}

export interface AppVersion {
  version?: string | null;
  isPublicRelease?: boolean;
  isPrerelease?: boolean;
  gitCommitId?: string;
  /** @format date-time */
  gitCommitDate?: string;
  assemblyConfiguration?: string;
}

export interface IItemSetOfListCategoriesResponse {
  /** @format int32 */
  count?: number;
  items?: ListCategoriesResponse[];
  isPagingEnabled?: boolean;
  /** @format int32 */
  page?: number;
  /** @format int32 */
  take?: number;
  /** @format int32 */
  totalCount?: number;
}

export interface ListCategoriesResponse {
  /** @format int32 */
  id?: number;
  name?: string;
}

export interface IItemSetOfIFailure {
  /** @format int32 */
  count?: number;
  items?: IFailure[];
  isPagingEnabled?: boolean;
  /** @format int32 */
  page?: number;
  /** @format int32 */
  take?: number;
  /** @format int32 */
  totalCount?: number;
}

export interface IFailure {
  message?: string;
  uiHandle?: string | null;
  code?: string | null;
}

export interface GetCategoryResponse {
  /** @format int32 */
  id?: number;
  name?: string;
  createdBy?: string;
  /** @format date-time */
  createdOn?: string;
  modifiedBy?: string;
  /** @format date-time */
  modifiedOn?: string;
  recipes?: GetCategoryResponseRecipe[];
}

export interface GetCategoryResponseRecipe {
  /** @format int32 */
  id?: number;
  name?: string;
  slug?: string;
  image?: string | null;
}

export type EntityMessageOfInteger = UserMessage & {
  /** @format int32 */
  id?: number;
};

export interface UserMessage {
  message?: string;
}

export interface SaveCategoryRequest {
  /** @format int32 */
  id?: number;
  name?: string;
}

export interface AddCategoryToAllRecipesRequest {
  /** @format int32 */
  id?: number;
}

export interface IItemSetOfListGroceryDepartmentsResponse {
  /** @format int32 */
  count?: number;
  items?: ListGroceryDepartmentsResponse[];
  isPagingEnabled?: boolean;
  /** @format int32 */
  page?: number;
  /** @format int32 */
  take?: number;
  /** @format int32 */
  totalCount?: number;
}

export interface ListGroceryDepartmentsResponse {
  /** @format int32 */
  id?: number;
  name?: string;
  /** @format int32 */
  order?: number;
}

export interface GetGroceryDepartmentResponse {
  /** @format int32 */
  id?: number;
  name?: string;
  /** @format int32 */
  order?: number;
  createdBy?: string;
  /** @format date-time */
  createdOn?: string;
  modifiedBy?: string;
  /** @format date-time */
  modifiedOn?: string;
  shoppingItems?: GetGroceryDepartmentResponseShoppingItem[];
}

export interface GetGroceryDepartmentResponseShoppingItem {
  /** @format int32 */
  id?: number;
  name?: string;
}

export interface SaveGroceryDepartmentRequest {
  /** @format int32 */
  id?: number;
  name?: string;
  /** @format int32 */
  order?: number;
}

export type EntityMessageOfString = UserMessage & {
  id?: string | null;
};

export interface IItemSetOfListMealPlansResponse {
  /** @format int32 */
  count?: number;
  items?: ListMealPlansResponse[];
  isPagingEnabled?: boolean;
  /** @format int32 */
  page?: number;
  /** @format int32 */
  take?: number;
  /** @format int32 */
  totalCount?: number;
}

export interface ListMealPlansResponse {
  /** @format int32 */
  id?: number;
  name?: string;
  /** @format date-time */
  createdOn?: string;
  /** @format date-time */
  modifiedOn?: string;
}

export interface GetMealPlanResponse {
  /** @format int32 */
  id?: number;
  name?: string;
  createdBy?: string;
  /** @format date-time */
  createdOn?: string;
  modifiedBy?: string;
  /** @format date-time */
  modifiedOn?: string;
  excludedShoppingItems?: GetMealPlanResponseExcludedShoppingItem[];
  recipes?: GetMealPlanResponseRecipe[];
}

export interface GetMealPlanResponseExcludedShoppingItem {
  /** @format int32 */
  id?: number;
  /** @format decimal */
  quantity?: number;
}

export interface GetMealPlanResponseRecipe {
  /** @format int32 */
  id?: number;
  name?: string;
  /** @format int32 */
  order?: number;
  image?: string | null;
  categories?: string[];
  shoppingItems?: GetMealPlanResponseRecipeShoppingItem[];
}

export interface GetMealPlanResponseRecipeShoppingItem {
  /** @format int32 */
  id?: number;
  name?: string;
  /** @format int32 */
  inventoryQuantity?: number;
  /** @format decimal */
  quantity?: number;
  /** @format int32 */
  order?: number;
  /** @format int32 */
  groceryDepartmentId?: number | null;
}

export interface SaveMealPlanRequest {
  /** @format int32 */
  id?: number;
  name?: string;
  excludedShoppingItems?: SaveMealPlanRequestExcludedShoppingItem[];
  recipes?: SaveMealPlanRequestRecipe[];
}

export interface SaveMealPlanRequestExcludedShoppingItem {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  quantity?: number;
}

export interface SaveMealPlanRequestRecipe {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  order?: number;
}

export interface IItemSetOfListPantryLocationsResponse {
  /** @format int32 */
  count?: number;
  items?: ListPantryLocationsResponse[];
  isPagingEnabled?: boolean;
  /** @format int32 */
  page?: number;
  /** @format int32 */
  take?: number;
  /** @format int32 */
  totalCount?: number;
}

export interface ListPantryLocationsResponse {
  /** @format int32 */
  id?: number;
  name?: string;
}

export interface GetPantryLocationResponse {
  /** @format int32 */
  id?: number;
  name?: string;
  createdBy?: string;
  /** @format date-time */
  createdOn?: string;
  modifiedBy?: string;
  /** @format date-time */
  modifiedOn?: string;
  shoppingItems?: GetPantryLocationResponseShoppingItem[];
}

export interface GetPantryLocationResponseShoppingItem {
  /** @format int32 */
  id?: number;
  name?: string;
}

export interface SavePantryLocationRequest {
  /** @format int32 */
  id?: number;
  name?: string;
}

export interface SearchRecipesResponse {
  results?: IItemSetOfSearchRecipesResultItem;
  facets?: SearchFacet[];
}

export interface IItemSetOfSearchRecipesResultItem {
  /** @format int32 */
  count?: number;
  items?: SearchRecipesResultItem[];
  isPagingEnabled?: boolean;
  /** @format int32 */
  page?: number;
  /** @format int32 */
  take?: number;
  /** @format int32 */
  totalCount?: number;
}

export interface SearchRecipesResultItem {
  /** @format int32 */
  id?: number;
  name?: string;
  isForMealPlanning?: boolean;
  /** @format date-time */
  createdOn?: string;
  slug?: string;
  categories?: string[];
  shoppingItems?: SearchRecipesResultItemShoppingItem[];
  image?: string | null;
}

export interface SearchRecipesResultItemShoppingItem {
  name?: string;
  /** @format int32 */
  quantity?: number;
  /** @format int32 */
  order?: number;
}

export interface SearchFacet {
  fieldName?: string;
  values?: SearchFacetValue[];
}

export interface SearchFacetValue {
  fieldValue?: string;
  /** @format int32 */
  count?: number;
}

export interface IItemSetOfSuggestRecipesResultItem {
  /** @format int32 */
  count?: number;
  items?: SuggestRecipesResultItem[];
  isPagingEnabled?: boolean;
  /** @format int32 */
  page?: number;
  /** @format int32 */
  take?: number;
  /** @format int32 */
  totalCount?: number;
}

export interface SuggestRecipesResultItem {
  /** @format int32 */
  id?: number;
  name?: string;
  slug?: string;
  image?: string | null;
}

export interface GetRecipeResponse {
  /** @format int32 */
  id?: number;
  name?: string;
  directions?: string;
  sides?: string;
  /** @format int32 */
  prepTimeMinutes?: number | null;
  /** @format int32 */
  cookTimeMinutes?: number | null;
  isForMealPlanning?: boolean;
  createdBy?: string;
  /** @format date-time */
  createdOn?: string;
  modifiedBy?: string;
  /** @format date-time */
  modifiedOn?: string;
  slug?: string;
  defaultImage?: string | null;
  pinnedImage?: string | null;
  images?: string[];
  categories?: string[];
  shoppingItems?: GetRecipeResponseShoppingItem[];
}

export interface GetRecipeResponseShoppingItem {
  /** @format int32 */
  id?: number;
  name?: string;
  /** @format int32 */
  inventoryQuantity?: number;
  /** @format decimal */
  quantity?: number;
  /** @format int32 */
  order?: number;
}

export interface SaveRecipeRequest {
  /** @format int32 */
  id?: number;
  name?: string;
  directions?: string;
  sides?: string;
  /** @format int32 */
  cookTimeMinutes?: number | null;
  /** @format int32 */
  prepTimeMinutes?: number | null;
  isForMealPlanning?: boolean;
  shoppingItems?: SaveRecipeRequestShoppingItem[];
  categories?: string[];
}

export interface SaveRecipeRequestShoppingItem {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  quantity?: number;
  /** @format int32 */
  order?: number;
}

export interface IItemSetOfListShoppingItemsResponse {
  /** @format int32 */
  count?: number;
  items?: ListShoppingItemsResponse[];
  isPagingEnabled?: boolean;
  /** @format int32 */
  page?: number;
  /** @format int32 */
  take?: number;
  /** @format int32 */
  totalCount?: number;
}

export interface ListShoppingItemsResponse {
  /** @format int32 */
  id?: number;
  name?: string;
  /** @format int32 */
  inventoryQuantity?: number;
  pantryLocations?: string[];
  /** @format int32 */
  groceryDepartmentId?: number | null;
}

export interface GetShoppingItemResponse {
  /** @format int32 */
  id?: number;
  name?: string;
  /** @format int32 */
  inventoryQuantity?: number;
  createdBy?: string;
  /** @format date-time */
  createdOn?: string;
  modifiedBy?: string;
  /** @format date-time */
  modifiedOn?: string;
  recipes?: GetShoppingItemResponseRecipe[];
  groceryDepartment?: GetShoppingItemResponseGroceryDepartment | null;
  pantryLocations?: string[];
}

export interface GetShoppingItemResponseRecipe {
  /** @format int32 */
  id?: number;
  name?: string;
  slug?: string;
  image?: string | null;
}

export interface GetShoppingItemResponseGroceryDepartment {
  /** @format int32 */
  id?: number;
  name?: string;
  /** @format int32 */
  order?: number;
}

export interface SaveShoppingItemRequest {
  /** @format int32 */
  id?: number;
  name?: string;
  /** @format int32 */
  inventoryQuantity?: number;
  /** @format int32 */
  groceryDepartmentId?: number | null;
  pantryLocations?: string[];
}

export interface SaveShoppingItemInventoryRequest {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  inventoryQuantity?: number;
}

export interface CategoriesListParams {
  /** Name contains (case-insensitive) */
  name?: string | null;
  /** Specify to show items that have relations or no relations */
  isUnused?: boolean | null;
  /**
   * Set false to get all results
   * @default true
   */
  isPagingEnabled?: boolean;
  /**
   * The page of results to retrieve
   * @format int32
   * @default 1
   */
  page?: number;
  /**
   * How many items in a page
   * @format int32
   * @default 30
   */
  take?: number;
}

export interface GroceryDepartmentsListParams {
  /** Name contains (case-insensitive) */
  name?: string | null;
  /** Specify to show items that have relations or no relations */
  isUnused?: boolean | null;
  /**
   * Set false to get all results
   * @default true
   */
  isPagingEnabled?: boolean;
  /**
   * The page of results to retrieve
   * @format int32
   * @default 1
   */
  page?: number;
  /**
   * How many items in a page
   * @format int32
   * @default 30
   */
  take?: number;
}

export interface ImagesUploadParams {
  /**
   * The ID of the recipe of which the image belongs to
   * @format int32
   */
  recipeId?: number;
}

export interface MealPlansListParams {
  /**
   * Set false to get all results
   * @default true
   */
  isPagingEnabled?: boolean;
  /**
   * The page of results to retrieve
   * @format int32
   * @default 1
   */
  page?: number;
  /**
   * How many items in a page
   * @format int32
   * @default 30
   */
  take?: number;
}

export interface PantryLocationsListParams {
  /** Name contains (case-insensitive) */
  name?: string | null;
  /** Specify to show items that have relations or no relations */
  isUnused?: boolean | null;
  /**
   * Set false to get all results
   * @default true
   */
  isPagingEnabled?: boolean;
  /**
   * The page of results to retrieve
   * @format int32
   * @default 1
   */
  page?: number;
  /**
   * How many items in a page
   * @format int32
   * @default 30
   */
  take?: number;
}

export interface RecipesSearchParams {
  /** Search text (case-insensitive) */
  searchText?: string | null;
  /** Category IDs to filter on */
  categories?: number[] | null;
  /** @default false */
  allCategories?: boolean;
  /** If the recipes should be enabled for meal planning */
  isForMealPlanning?: boolean | null;
  /** Field name to sort by (case-insensitive). Options are: newest, oldest, a-z, z-a, random. Default if empty is search score. */
  sortBy?: string | null;
  /** Give a seed for stable random sorting. By default is stable for 24 hours on the server. */
  randomSortSeed?: string | null;
  /**
   * Set false to get all results
   * @default true
   */
  isPagingEnabled?: boolean;
  /**
   * The page of results to retrieve
   * @format int32
   * @default 1
   */
  page?: number;
  /**
   * How many items in a page
   * @format int32
   * @default 30
   */
  take?: number;
}

export interface RecipesSuggestParams {
  /** Search text (case-insensitive) */
  searchText?: string | null;
  /**
   * Set false to get all results
   * @default true
   */
  isPagingEnabled?: boolean;
  /**
   * How many items in a page
   * @format int32
   * @default 8
   */
  take?: number;
}

export interface ShoppingItemsListParams {
  /** Name contains (case-insensitive) */
  name?: string | null;
  /** Specify to show items that have relations or no relations */
  isUnused?: boolean | null;
  /**
   * Set false to get all results
   * @default true
   */
  isPagingEnabled?: boolean;
  /**
   * The page of results to retrieve
   * @format int32
   * @default 1
   */
  page?: number;
  /**
   * How many items in a page
   * @format int32
   * @default 30
   */
  take?: number;
}

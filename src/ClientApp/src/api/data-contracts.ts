/* eslint-disable */
/* tslint:disable */
// @ts-nocheck
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

/** Information for bootstrapping a web client. */
export interface WebClientInfo {
  /** The value of the header antiforgery token */
  antiforgeryToken?: string;
  /** The header name of the antiforgery token */
  antiforgeryTokenHeaderName?: string;
  /** The UI-friendly application name. */
  applicationName?: string;
  /** The current user */
  user?: DomainUser;
}

/** A user for use in the domain layer and model services. */
export interface DomainUser {
  /** UI-friendly name for the current user */
  login?: string;
  /** Names of the authorization policies that the user fulfills. */
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

/** A set of items. Can optionally by a page of a full set. */
export interface IItemSetOfListCategoriesResponse {
  /**
   * The count of items in this set.
   * @format int32
   */
  count?: number;
  /** The items in this set. */
  items?: ListCategoriesResponse[];
  /** When true, this is a page of a full set. */
  isPagingEnabled?: boolean;
  /**
   * If paging is enabled, this represents the page number in the total set.
   * @format int32
   */
  page?: number;
  /**
   * If paging is enabled, the requested number of results per page.
   * @format int32
   */
  take?: number;
  /**
   * The count of all the items in the total set. If paging is enabled, the total number of results in all pages.
   * @format int32
   */
  totalCount?: number;
}

export interface ListCategoriesResponse {
  /** @format int32 */
  id?: number;
  name?: string;
  /** @format int32 */
  recipeCount?: number;
}

/** A set of items. Can optionally by a page of a full set. */
export interface IItemSetOfIFailure {
  /**
   * The count of items in this set.
   * @format int32
   */
  count?: number;
  /** The items in this set. */
  items?: IFailure[];
  /** When true, this is a page of a full set. */
  isPagingEnabled?: boolean;
  /**
   * If paging is enabled, this represents the page number in the total set.
   * @format int32
   */
  page?: number;
  /**
   * If paging is enabled, the requested number of results per page.
   * @format int32
   */
  take?: number;
  /**
   * The count of all the items in the total set. If paging is enabled, the total number of results in all pages.
   * @format int32
   */
  totalCount?: number;
}

/** A domain logic failure with UI-friendly error message and optional field name or UI handle. */
export interface IFailure {
  /** The UI-friendly error message to be displayed to the user. */
  message?: string;
  /** The name of the UI field corresponding to the invalid user input. */
  uiHandle?: string | null;
  /** A A code name or identifier for the error for programmatic error discrimination. */
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

/** A UI-friendly message and the Id of the entity that was affected during an event. */
export type EntityMessageOfInteger = UserMessage & {
  /**
   * The Id of the entity affected during an event.
   * @format int32
   */
  id?: number;
};

/** A UI-friendly message. */
export interface UserMessage {
  /** The UI-friendly message. */
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

/** A set of items. Can optionally by a page of a full set. */
export interface IItemSetOfListGroceryDepartmentsResponse {
  /**
   * The count of items in this set.
   * @format int32
   */
  count?: number;
  /** The items in this set. */
  items?: ListGroceryDepartmentsResponse[];
  /** When true, this is a page of a full set. */
  isPagingEnabled?: boolean;
  /**
   * If paging is enabled, this represents the page number in the total set.
   * @format int32
   */
  page?: number;
  /**
   * If paging is enabled, the requested number of results per page.
   * @format int32
   */
  take?: number;
  /**
   * The count of all the items in the total set. If paging is enabled, the total number of results in all pages.
   * @format int32
   */
  totalCount?: number;
}

export interface ListGroceryDepartmentsResponse {
  /** @format int32 */
  id?: number;
  name?: string;
  /** @format int32 */
  order?: number;
  /** @format int32 */
  shoppingItemCount?: number;
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

/** A UI-friendly message and the Id of the entity that was affected during an event. */
export type EntityMessageOfString = UserMessage & {
  /** The Id of the entity affected during an event. */
  id?: string | null;
};

/** A set of items. Can optionally by a page of a full set. */
export interface IItemSetOfListMealPlansResponse {
  /**
   * The count of items in this set.
   * @format int32
   */
  count?: number;
  /** The items in this set. */
  items?: ListMealPlansResponse[];
  /** When true, this is a page of a full set. */
  isPagingEnabled?: boolean;
  /**
   * If paging is enabled, this represents the page number in the total set.
   * @format int32
   */
  page?: number;
  /**
   * If paging is enabled, the requested number of results per page.
   * @format int32
   */
  take?: number;
  /**
   * The count of all the items in the total set. If paging is enabled, the total number of results in all pages.
   * @format int32
   */
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
  /** @format int32 */
  recipeCount?: number;
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
  isComplete?: boolean;
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
  isComplete?: boolean;
}

/** A set of items. Can optionally by a page of a full set. */
export interface IItemSetOfListPantryLocationsResponse {
  /**
   * The count of items in this set.
   * @format int32
   */
  count?: number;
  /** The items in this set. */
  items?: ListPantryLocationsResponse[];
  /** When true, this is a page of a full set. */
  isPagingEnabled?: boolean;
  /**
   * If paging is enabled, this represents the page number in the total set.
   * @format int32
   */
  page?: number;
  /**
   * If paging is enabled, the requested number of results per page.
   * @format int32
   */
  take?: number;
  /**
   * The count of all the items in the total set. If paging is enabled, the total number of results in all pages.
   * @format int32
   */
  totalCount?: number;
}

export interface ListPantryLocationsResponse {
  /** @format int32 */
  id?: number;
  name?: string;
  /** @format int32 */
  shoppingItemCount?: number;
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
  /** A set of items. Can optionally by a page of a full set. */
  results?: IItemSetOfSearchRecipesResultItem;
  facets?: SearchFacet[];
}

/** A set of items. Can optionally by a page of a full set. */
export interface IItemSetOfSearchRecipesResultItem {
  /**
   * The count of items in this set.
   * @format int32
   */
  count?: number;
  /** The items in this set. */
  items?: SearchRecipesResultItem[];
  /** When true, this is a page of a full set. */
  isPagingEnabled?: boolean;
  /**
   * If paging is enabled, this represents the page number in the total set.
   * @format int32
   */
  page?: number;
  /**
   * If paging is enabled, the requested number of results per page.
   * @format int32
   */
  take?: number;
  /**
   * The count of all the items in the total set. If paging is enabled, the total number of results in all pages.
   * @format int32
   */
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

/** A set of items. Can optionally by a page of a full set. */
export interface IItemSetOfSuggestRecipesResultItem {
  /**
   * The count of items in this set.
   * @format int32
   */
  count?: number;
  /** The items in this set. */
  items?: SuggestRecipesResultItem[];
  /** When true, this is a page of a full set. */
  isPagingEnabled?: boolean;
  /**
   * If paging is enabled, this represents the page number in the total set.
   * @format int32
   */
  page?: number;
  /**
   * If paging is enabled, the requested number of results per page.
   * @format int32
   */
  take?: number;
  /**
   * The count of all the items in the total set. If paging is enabled, the total number of results in all pages.
   * @format int32
   */
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

/** A set of items. Can optionally by a page of a full set. */
export interface IItemSetOfListShoppingItemsResponse {
  /**
   * The count of items in this set.
   * @format int32
   */
  count?: number;
  /** The items in this set. */
  items?: ListShoppingItemsResponse[];
  /** When true, this is a page of a full set. */
  isPagingEnabled?: boolean;
  /**
   * If paging is enabled, this represents the page number in the total set.
   * @format int32
   */
  page?: number;
  /**
   * If paging is enabled, the requested number of results per page.
   * @format int32
   */
  take?: number;
  /**
   * The count of all the items in the total set. If paging is enabled, the total number of results in all pages.
   * @format int32
   */
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
  /** @format int32 */
  recipeCount?: number;
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
  /**
   * When true, recipes returned will match all selected categories
   * @default false
   */
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

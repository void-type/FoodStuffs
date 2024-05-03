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
}

export type EntityMessageOfString = UserMessage & {
  id?: string | null;
};

export interface UserMessage {
  message?: string;
}

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
  pantryIngredients?: GetMealPlanResponsePantryIngredient[];
  recipes?: GetMealPlanResponseRecipe[];
}

export interface GetMealPlanResponsePantryIngredient {
  name?: string;
  /** @format decimal */
  quantity?: number;
}

export interface GetMealPlanResponseRecipe {
  /** @format int32 */
  id?: number;
  name?: string;
  image?: string | null;
  categories?: string[];
  ingredients?: GetMealPlanResponseRecipeIngredient[];
}

export interface GetMealPlanResponseRecipeIngredient {
  name?: string;
  /** @format decimal */
  quantity?: number;
  /** @format int32 */
  order?: number;
  isCategory?: boolean;
}

export type EntityMessageOfInteger = UserMessage & {
  /** @format int32 */
  id?: number;
};

export interface SaveMealPlanRequest {
  /** @format int32 */
  id?: number;
  name?: string;
  recipeIds?: number[];
  pantryIngredients?: SaveMealPlanRequestPantryIngredient[];
}

export interface SaveMealPlanRequestPantryIngredient {
  name?: string;
  /** @format decimal */
  quantity?: number;
}

export interface RecipeSearchResponse {
  results?: IItemSetOfRecipeSearchResultItem;
  facets?: RecipeSearchFacet[];
}

export interface IItemSetOfRecipeSearchResultItem {
  /** @format int32 */
  count?: number;
  items?: RecipeSearchResultItem[];
  isPagingEnabled?: boolean;
  /** @format int32 */
  page?: number;
  /** @format int32 */
  take?: number;
  /** @format int32 */
  totalCount?: number;
}

export interface RecipeSearchResultItem {
  /** @format int32 */
  id?: number;
  name?: string;
  isForMealPlanning?: boolean;
  /** @format date-time */
  createdOn?: string;
  slug?: string;
  categories?: string[];
  ingredients?: RecipeSearchResultItemIngredient[];
  image?: string | null;
}

export interface RecipeSearchResultItemIngredient {
  name?: string;
  /** @format decimal */
  quantity?: number;
  /** @format int32 */
  order?: number;
  isCategory?: boolean;
}

export interface RecipeSearchFacet {
  fieldName?: string;
  values?: RecipeSearchFacetValue[];
}

export interface RecipeSearchFacetValue {
  fieldValue?: string;
  /** @format int32 */
  count?: number;
}

export interface GetRecipeResponse {
  /** @format int32 */
  id?: number;
  name?: string;
  directions?: string;
  /** @format int32 */
  cookTimeMinutes?: number | null;
  /** @format int32 */
  prepTimeMinutes?: number | null;
  createdBy?: string;
  /** @format date-time */
  createdOn?: string;
  modifiedBy?: string;
  /** @format date-time */
  modifiedOn?: string;
  slug?: string;
  pinnedImage?: string | null;
  isForMealPlanning?: boolean;
  categories?: string[];
  images?: string[];
  ingredients?: GetRecipeResponseIngredient[];
}

export interface GetRecipeResponseIngredient {
  name?: string;
  /** @format decimal */
  quantity?: number;
  /** @format int32 */
  order?: number;
  isCategory?: boolean;
}

export interface SaveRecipeRequest {
  /** @format int32 */
  id?: number;
  name?: string;
  directions?: string;
  /** @format int32 */
  cookTimeMinutes?: number | null;
  /** @format int32 */
  prepTimeMinutes?: number | null;
  isForMealPlanning?: boolean;
  ingredients?: SaveRecipeRequestIngredient[];
  categories?: string[];
}

export interface SaveRecipeRequestIngredient {
  name?: string;
  /** @format decimal */
  quantity?: number;
  /** @format int32 */
  order?: number;
  isCategory?: boolean;
}

export interface CategoriesSearchParams {
  /** Name contains (case-insensitive) */
  name?: string | null;
  /**
   * False for all results
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

export interface MealPlansSearchParams {
  /** Name contains (case-insensitive) */
  name?: string | null;
  /**
   * False for all results
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
  /** Name contains (case-insensitive) */
  name?: string | null;
  /** Category IDs to filter on */
  categories?: number[] | null;
  /** If the recipes should be enabled for meal planning */
  isForMealPlanning?: boolean | null;
  /** Field name to sort by (case-insensitive). Options are: newest, oldest, a-z, z-a, random. Default if empty is search score. */
  sortBy?: string | null;
  /** Give a seed for stable random sorting. By default is stable for 24 hours on the server. */
  randomSortSeed?: string | null;
  /**
   * False for all results
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

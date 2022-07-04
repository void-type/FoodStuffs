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

export interface AppVersion {
  version?: string | null;
  isPublicRelease?: boolean;
  isPrerelease?: boolean;
  gitCommitId?: string | null;

  /** @format date-time */
  gitCommitDate?: string;
  assemblyConfiguration?: string | null;
}

export interface DomainUser {
  login?: string | null;
  authorizedAs?: string[] | null;
}

export interface GetRecipeResponse {
  /** @format int32 */
  id?: number;
  name?: string | null;
  directions?: string | null;

  /** @format int32 */
  cookTimeMinutes?: number | null;

  /** @format int32 */
  prepTimeMinutes?: number | null;
  createdBy?: string | null;

  /** @format date-time */
  createdOn?: string;
  modifiedBy?: string | null;

  /** @format date-time */
  modifiedOn?: string;

  /** @format int32 */
  pinnedImageId?: number | null;
  isForMealPlanning?: boolean;
  categories?: string[] | null;
  images?: number[] | null;
  ingredients?: GetRecipeResponseIngredient[] | null;
}

export interface GetRecipeResponseIngredient {
  name?: string | null;

  /** @format int32 */
  quantity?: number;

  /** @format int32 */
  order?: number;
  isCategory?: boolean;
}

export interface IFailure {
  message?: string | null;
  uiHandle?: string | null;
}

export interface IFailureIItemSet {
  /** @format int32 */
  count?: number;
  items?: IFailure[] | null;
  isPagingEnabled?: boolean;

  /** @format int32 */
  page?: number;

  /** @format int32 */
  take?: number;

  /** @format int32 */
  totalCount?: number;
}

export interface Int32EntityMessage {
  message?: string | null;

  /** @format int32 */
  id?: number;
}

export interface ListRecipesResponse {
  /** @format int32 */
  id?: number;
  name?: string | null;
  categories?: string[] | null;

  /** @format int32 */
  imageId?: number | null;
}

export interface ListRecipesResponseIItemSet {
  /** @format int32 */
  count?: number;
  items?: ListRecipesResponse[] | null;
  isPagingEnabled?: boolean;

  /** @format int32 */
  page?: number;

  /** @format int32 */
  take?: number;

  /** @format int32 */
  totalCount?: number;
}

export interface PinImageRequest {
  /** @format int32 */
  id?: number;
}

export interface SaveRecipeRequest {
  /** @format int32 */
  id?: number;
  name?: string | null;
  directions?: string | null;

  /** @format int32 */
  cookTimeMinutes?: number | null;

  /** @format int32 */
  prepTimeMinutes?: number | null;
  isForMealPlanning?: boolean;
  ingredients?: SaveRecipeRequestIngredient[] | null;
  categories?: string[] | null;
}

export interface SaveRecipeRequestIngredient {
  name?: string | null;

  /** @format int32 */
  quantity?: number;

  /** @format int32 */
  order?: number;
  isCategory?: boolean;
}

export interface WebClientInfo {
  antiforgeryToken?: string | null;
  antiforgeryTokenHeaderName?: string | null;
  applicationName?: string | null;
  user?: DomainUser;
}

export interface ImagesCreateParams {
  /**
   * The Id of the recipe the image is of
   * @format int32
   */
  recipeId?: number;
}

export interface RecipesListParams {
  /** Name contains (case-insensitive) */
  name?: string;

  /** Category names contain (case-insensitive) */
  category?: string;

  /** If the recipes should be enabled for meal planning */
  isForMealPlanning?: boolean | null;

  /** Field name to sort by (case-insensitive). Options are: name. Default is ID */
  sortBy?: string;

  /** True for descending sort */
  sortDesc?: boolean;

  /** False for all results */
  isPagingEnabled?: boolean;

  /**
   * The page of results to retrieve
   * @format int32
   */
  page?: number;

  /**
   * How many items in a page
   * @format int32
   */
  take?: number;
}

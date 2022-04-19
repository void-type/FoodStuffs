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

import type {
  AppVersion,
  GetRecipeResponse,
  IFailureIItemSet,
  Int32EntityMessage,
  ListRecipesResponseIItemSet,
  PinImageRequest,
  SaveRecipeRequest,
  WebClientInfo,
} from './data-contracts';
import { ContentType, HttpClient } from './http-client';
import type { RequestParams } from './http-client';

export class Api<
  SecurityDataType = unknown
  > extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Application
   * @name AppInfoList
   * @summary Get information to bootstrap the SPA client like application name and user data.
   * @request GET:/api/app/info
   */
  appInfoList = (params: RequestParams = {}) =>
    this.request<WebClientInfo, any>({
      path: `/api/app/info`,
      method: 'GET',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Application
   * @name AppVersionList
   * @summary Get the version of the application.
   * @request GET:/api/app/version
   */
  appVersionList = (params: RequestParams = {}) =>
    this.request<AppVersion, any>({
      path: `/api/app/version`,
      method: 'GET',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Images
   * @name ImagesDetail
   * @summary Get an image.
   * @request GET:/api/images/{id}
   */
  imagesDetail = (id: number, params: RequestParams = {}) =>
    this.request<File, IFailureIItemSet>({
      path: `/api/images/${id}`,
      method: 'GET',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Images
   * @name ImagesDelete
   * @summary Delete an image.
   * @request DELETE:/api/images/{id}
   */
  imagesDelete = (id: number, params: RequestParams = {}) =>
    this.request<Int32EntityMessage, IFailureIItemSet>({
      path: `/api/images/${id}`,
      method: 'DELETE',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Images
   * @name ImagesCreate
   * @summary Upload an image using a multi-part form file.
   * @request POST:/api/images
   */
  imagesCreate = (
    data: { file?: File },
    query?: { recipeId?: number },
    params: RequestParams = {}
  ) =>
    this.request<Int32EntityMessage, IFailureIItemSet>({
      path: `/api/images`,
      method: 'POST',
      query: query,
      body: data,
      type: ContentType.FormData,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Images
   * @name ImagesPinCreate
   * @summary Pin an image for a recipe. This image will be the default image for the recipe.
   * @request POST:/api/images/pin
   */
  imagesPinCreate = (data: PinImageRequest, params: RequestParams = {}) =>
    this.request<Int32EntityMessage, IFailureIItemSet>({
      path: `/api/images/pin`,
      method: 'POST',
      body: data,
      type: ContentType.Json,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Recipes
   * @name RecipesList
   * @summary Search for recipes using the following criteria. All are optional and some have defaults.
   * @request GET:/api/recipes
   */
  recipesList = (
    query?: {
      name?: string;
      category?: string;
      sortBy?: string;
      sortDesc?: boolean;
      isPagingEnabled?: boolean;
      page?: number;
      take?: number;
    },
    params: RequestParams = {}
  ) =>
    this.request<ListRecipesResponseIItemSet, IFailureIItemSet>({
      path: `/api/recipes`,
      method: 'GET',
      query: query,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Recipes
   * @name RecipesCreate
   * @summary Save a recipe. Will update if found, otherwise a new recipe will be created.
   * @request POST:/api/recipes
   */
  recipesCreate = (data: SaveRecipeRequest, params: RequestParams = {}) =>
    this.request<Int32EntityMessage, IFailureIItemSet>({
      path: `/api/recipes`,
      method: 'POST',
      body: data,
      type: ContentType.Json,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Recipes
   * @name RecipesDetail
   * @summary Get a recipe.
   * @request GET:/api/recipes/{id}
   */
  recipesDetail = (id: number, params: RequestParams = {}) =>
    this.request<GetRecipeResponse, IFailureIItemSet>({
      path: `/api/recipes/${id}`,
      method: 'GET',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Recipes
   * @name RecipesDelete
   * @summary Delete a recipe.
   * @request DELETE:/api/recipes/{id}
   */
  recipesDelete = (id: number, params: RequestParams = {}) =>
    this.request<Int32EntityMessage, IFailureIItemSet>({
      path: `/api/recipes/${id}`,
      method: 'DELETE',
      format: 'json',
      ...params,
    });
}

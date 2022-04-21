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
  ImagesCreateParams,
  Int32EntityMessage,
  ListRecipesResponseIItemSet,
  PinImageRequest,
  RecipesListParams,
  SaveRecipeRequest,
  WebClientInfo,
} from './data-contracts';
import { ContentType, HttpClient, type RequestParams } from './http-client';

export class Api<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Application
   * @name AppInfoList
   * @summary Get information to bootstrap the SPA client like application name and user data.
   * @request GET:/api/app/info
   * @response `200` `WebClientInfo` Success
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
   * @response `200` `AppVersion` Success
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
   * @response `200` `File` Success
   * @response `400` `IFailureIItemSet` Bad Request
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
   * @response `200` `Int32EntityMessage` Success
   * @response `400` `IFailureIItemSet` Bad Request
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
   * @response `200` `Int32EntityMessage` Success
   * @response `400` `IFailureIItemSet` Bad Request
   */
  imagesCreate = (query: ImagesCreateParams, data: { file?: File }, params: RequestParams = {}) =>
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
   * @response `200` `Int32EntityMessage` Success
   * @response `400` `IFailureIItemSet` Bad Request
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
   * @response `200` `ListRecipesResponseIItemSet` Success
   * @response `400` `IFailureIItemSet` Bad Request
   */
  recipesList = (query: RecipesListParams, params: RequestParams = {}) =>
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
   * @response `200` `Int32EntityMessage` Success
   * @response `400` `IFailureIItemSet` Bad Request
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
   * @response `200` `GetRecipeResponse` Success
   * @response `400` `IFailureIItemSet` Bad Request
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
   * @response `200` `Int32EntityMessage` Success
   * @response `400` `IFailureIItemSet` Bad Request
   */
  recipesDelete = (id: number, params: RequestParams = {}) =>
    this.request<Int32EntityMessage, IFailureIItemSet>({
      path: `/api/recipes/${id}`,
      method: 'DELETE',
      format: 'json',
      ...params,
    });
}

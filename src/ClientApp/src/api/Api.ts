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

import type {
  AddCategoryToAllRecipesRequest,
  AppVersion,
  CategoriesListParams,
  EntityMessageOfInteger,
  EntityMessageOfString,
  GetCategoryResponse,
  GetGroceryAisleResponse,
  GetMealPlanResponse,
  GetPantryLocationResponse,
  GetRecipeResponse,
  GetGroceryItemResponse,
  GroceryAislesListParams,
  IItemSetOfIFailure,
  IItemSetOfListCategoriesResponse,
  IItemSetOfListGroceryAislesResponse,
  IItemSetOfListMealPlansResponse,
  IItemSetOfListPantryLocationsResponse,
  IItemSetOfListGroceryItemsResponse,
  IItemSetOfSuggestRecipesResultItem,
  ImagesUploadParams,
  MealPlansListParams,
  PantryLocationsListParams,
  RecipesSearchParams,
  RecipesSuggestParams,
  SaveCategoryRequest,
  SaveGroceryAisleRequest,
  SaveMealPlanRequest,
  SavePantryLocationRequest,
  SaveRecipeRequest,
  SaveGroceryItemInventoryRequest,
  SaveGroceryItemRequest,
  SearchRecipesResponse,
  GroceryItemsListParams,
  UserMessage,
  WebClientInfo,
} from './data-contracts';
import { ContentType, HttpClient, type RequestParams } from './http-client';

export class Api<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags App
   * @name AppGetInfo
   * @summary Get information to bootstrap the SPA client like application name and user data.
   * @request GET:/api/app/info
   * @response `200` `WebClientInfo`
   */
  appGetInfo = (params: RequestParams = {}) =>
    this.request<WebClientInfo, any>({
      path: `/api/app/info`,
      method: 'GET',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags App
   * @name AppGetVersion
   * @summary Get the version of the application.
   * @request GET:/api/app/version
   * @response `200` `AppVersion`
   */
  appGetVersion = (params: RequestParams = {}) =>
    this.request<AppVersion, any>({
      path: `/api/app/version`,
      method: 'GET',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Categories
   * @name CategoriesList
   * @summary List categories. All parameters are optional and some have defaults.
   * @request GET:/api/categories
   * @response `200` `IItemSetOfListCategoriesResponse`
   * @response `400` `IItemSetOfIFailure`
   */
  categoriesList = (query: CategoriesListParams, params: RequestParams = {}) =>
    this.request<IItemSetOfListCategoriesResponse, IItemSetOfIFailure>({
      path: `/api/categories`,
      method: 'GET',
      query: query,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Categories
   * @name CategoriesSave
   * @summary Save a category. Will update if found, otherwise a new item will be created.
   * @request POST:/api/categories
   * @response `200` `EntityMessageOfInteger`
   * @response `400` `IItemSetOfIFailure`
   */
  categoriesSave = (data: SaveCategoryRequest, params: RequestParams = {}) =>
    this.request<EntityMessageOfInteger, IItemSetOfIFailure>({
      path: `/api/categories`,
      method: 'POST',
      body: data,
      type: ContentType.Json,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Categories
   * @name CategoriesGet
   * @summary Get a category.
   * @request GET:/api/categories/{id}
   * @response `200` `GetCategoryResponse`
   * @response `400` `IItemSetOfIFailure`
   */
  categoriesGet = (id: number, params: RequestParams = {}) =>
    this.request<GetCategoryResponse, IItemSetOfIFailure>({
      path: `/api/categories/${id}`,
      method: 'GET',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Categories
   * @name CategoriesDelete
   * @summary Delete a category.
   * @request DELETE:/api/categories/{id}
   * @response `200` `EntityMessageOfInteger`
   * @response `400` `IItemSetOfIFailure`
   */
  categoriesDelete = (id: number, params: RequestParams = {}) =>
    this.request<EntityMessageOfInteger, IItemSetOfIFailure>({
      path: `/api/categories/${id}`,
      method: 'DELETE',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Categories
   * @name CategoriesAddToAllRecipes
   * @summary Add category to all recipes.
   * @request POST:/api/categories/add-to-all-recipes
   * @response `200` `EntityMessageOfInteger`
   * @response `400` `IItemSetOfIFailure`
   */
  categoriesAddToAllRecipes = (data: AddCategoryToAllRecipesRequest, params: RequestParams = {}) =>
    this.request<EntityMessageOfInteger, IItemSetOfIFailure>({
      path: `/api/categories/add-to-all-recipes`,
      method: 'POST',
      body: data,
      type: ContentType.Json,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags GroceryAisles
   * @name GroceryAislesList
   * @summary List grocery aisles. All parameters are optional and some have defaults.
   * @request GET:/api/grocery-aisles
   * @response `200` `IItemSetOfListGroceryAislesResponse`
   * @response `400` `IItemSetOfIFailure`
   */
  groceryAislesList = (query: GroceryAislesListParams, params: RequestParams = {}) =>
    this.request<IItemSetOfListGroceryAislesResponse, IItemSetOfIFailure>({
      path: `/api/grocery-aisles`,
      method: 'GET',
      query: query,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags GroceryAisles
   * @name GroceryAislesSave
   * @summary Save a grocery aisle. Will update if found, otherwise a new item will be created.
   * @request POST:/api/grocery-aisles
   * @response `200` `EntityMessageOfInteger`
   * @response `400` `IItemSetOfIFailure`
   */
  groceryAislesSave = (data: SaveGroceryAisleRequest, params: RequestParams = {}) =>
    this.request<EntityMessageOfInteger, IItemSetOfIFailure>({
      path: `/api/grocery-aisles`,
      method: 'POST',
      body: data,
      type: ContentType.Json,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags GroceryAisles
   * @name GroceryAislesGet
   * @summary Get a grocery aisle.
   * @request GET:/api/grocery-aisles/{id}
   * @response `200` `GetGroceryAisleResponse`
   * @response `400` `IItemSetOfIFailure`
   */
  groceryAislesGet = (id: number, params: RequestParams = {}) =>
    this.request<GetGroceryAisleResponse, IItemSetOfIFailure>({
      path: `/api/grocery-aisles/${id}`,
      method: 'GET',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags GroceryAisles
   * @name GroceryAislesDelete
   * @summary Delete a grocery aisle.
   * @request DELETE:/api/grocery-aisles/{id}
   * @response `200` `EntityMessageOfInteger`
   * @response `400` `IItemSetOfIFailure`
   */
  groceryAislesDelete = (id: number, params: RequestParams = {}) =>
    this.request<EntityMessageOfInteger, IItemSetOfIFailure>({
      path: `/api/grocery-aisles/${id}`,
      method: 'DELETE',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Images
   * @name ImagesGet
   * @summary Get an image.
   * @request GET:/api/images/{name}
   * @response `200` `File`
   * @response `400` `IItemSetOfIFailure`
   */
  imagesGet = (name: string, params: RequestParams = {}) =>
    this.request<File, IItemSetOfIFailure>({
      path: `/api/images/${name}`,
      method: 'GET',
      ...params,
    });
  /**
   * No description
   *
   * @tags Images
   * @name ImagesDelete
   * @summary Delete an image.
   * @request DELETE:/api/images/{name}
   * @response `200` `EntityMessageOfString`
   * @response `400` `IItemSetOfIFailure`
   */
  imagesDelete = (name: string, params: RequestParams = {}) =>
    this.request<EntityMessageOfString, IItemSetOfIFailure>({
      path: `/api/images/${name}`,
      method: 'DELETE',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Images
   * @name ImagesUpload
   * @summary Upload an image using a multi-part form file.
   * @request POST:/api/images
   * @response `200` `EntityMessageOfString`
   * @response `400` `IItemSetOfIFailure`
   */
  imagesUpload = (
    query: ImagesUploadParams,
    data: {
      /** @format binary */
      file?: File | null;
    },
    params: RequestParams = {}
  ) =>
    this.request<EntityMessageOfString, IItemSetOfIFailure>({
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
   * @name ImagesPin
   * @summary Pin an image for a recipe. This image will be the default image for the recipe.
   * @request POST:/api/images/pin/{name}
   * @response `200` `EntityMessageOfString`
   * @response `400` `IItemSetOfIFailure`
   */
  imagesPin = (name: string, params: RequestParams = {}) =>
    this.request<EntityMessageOfString, IItemSetOfIFailure>({
      path: `/api/images/pin/${name}`,
      method: 'POST',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags MealPlans
   * @name MealPlansList
   * @summary List meal plans. All parameters are optional and some have defaults.
   * @request GET:/api/meal-plans
   * @response `200` `IItemSetOfListMealPlansResponse`
   * @response `400` `IItemSetOfIFailure`
   */
  mealPlansList = (query: MealPlansListParams, params: RequestParams = {}) =>
    this.request<IItemSetOfListMealPlansResponse, IItemSetOfIFailure>({
      path: `/api/meal-plans`,
      method: 'GET',
      query: query,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags MealPlans
   * @name MealPlansSave
   * @summary Save a meal plan. Will update if found, otherwise a new meal plan will be created.
   * @request POST:/api/meal-plans
   * @response `200` `EntityMessageOfInteger`
   * @response `400` `IItemSetOfIFailure`
   */
  mealPlansSave = (data: SaveMealPlanRequest, params: RequestParams = {}) =>
    this.request<EntityMessageOfInteger, IItemSetOfIFailure>({
      path: `/api/meal-plans`,
      method: 'POST',
      body: data,
      type: ContentType.Json,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags MealPlans
   * @name MealPlansGet
   * @summary Get a meal plan.
   * @request GET:/api/meal-plans/{id}
   * @response `200` `GetMealPlanResponse`
   * @response `400` `IItemSetOfIFailure`
   */
  mealPlansGet = (id: number, params: RequestParams = {}) =>
    this.request<GetMealPlanResponse, IItemSetOfIFailure>({
      path: `/api/meal-plans/${id}`,
      method: 'GET',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags MealPlans
   * @name MealPlansDelete
   * @summary Delete a meal plan.
   * @request DELETE:/api/meal-plans/{id}
   * @response `200` `EntityMessageOfInteger`
   * @response `400` `IItemSetOfIFailure`
   */
  mealPlansDelete = (id: number, params: RequestParams = {}) =>
    this.request<EntityMessageOfInteger, IItemSetOfIFailure>({
      path: `/api/meal-plans/${id}`,
      method: 'DELETE',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags PantryLocations
   * @name PantryLocationsList
   * @summary List storage locations. All parameters are optional and some have defaults.
   * @request GET:/api/pantry-locations
   * @response `200` `IItemSetOfListPantryLocationsResponse`
   * @response `400` `IItemSetOfIFailure`
   */
  pantryLocationsList = (query: PantryLocationsListParams, params: RequestParams = {}) =>
    this.request<IItemSetOfListPantryLocationsResponse, IItemSetOfIFailure>({
      path: `/api/pantry-locations`,
      method: 'GET',
      query: query,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags PantryLocations
   * @name PantryLocationsSave
   * @summary Save a storage location. Will update if found, otherwise a new item will be created.
   * @request POST:/api/pantry-locations
   * @response `200` `EntityMessageOfInteger`
   * @response `400` `IItemSetOfIFailure`
   */
  pantryLocationsSave = (data: SavePantryLocationRequest, params: RequestParams = {}) =>
    this.request<EntityMessageOfInteger, IItemSetOfIFailure>({
      path: `/api/pantry-locations`,
      method: 'POST',
      body: data,
      type: ContentType.Json,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags PantryLocations
   * @name PantryLocationsGet
   * @summary Get a storage location.
   * @request GET:/api/pantry-locations/{id}
   * @response `200` `GetPantryLocationResponse`
   * @response `400` `IItemSetOfIFailure`
   */
  pantryLocationsGet = (id: number, params: RequestParams = {}) =>
    this.request<GetPantryLocationResponse, IItemSetOfIFailure>({
      path: `/api/pantry-locations/${id}`,
      method: 'GET',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags PantryLocations
   * @name PantryLocationsDelete
   * @summary Delete a storage location.
   * @request DELETE:/api/pantry-locations/{id}
   * @response `200` `EntityMessageOfInteger`
   * @response `400` `IItemSetOfIFailure`
   */
  pantryLocationsDelete = (id: number, params: RequestParams = {}) =>
    this.request<EntityMessageOfInteger, IItemSetOfIFailure>({
      path: `/api/pantry-locations/${id}`,
      method: 'DELETE',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Recipes
   * @name RecipesSearch
   * @summary Search for recipes using the following criteria. All are optional and some have defaults.
   * @request GET:/api/recipes
   * @response `200` `SearchRecipesResponse`
   * @response `400` `IItemSetOfIFailure`
   */
  recipesSearch = (query: RecipesSearchParams, params: RequestParams = {}) =>
    this.request<SearchRecipesResponse, IItemSetOfIFailure>({
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
   * @name RecipesSave
   * @summary Save a recipe. Will update if found, otherwise a new recipe will be created.
   * @request POST:/api/recipes
   * @response `200` `EntityMessageOfInteger`
   * @response `400` `IItemSetOfIFailure`
   */
  recipesSave = (data: SaveRecipeRequest, params: RequestParams = {}) =>
    this.request<EntityMessageOfInteger, IItemSetOfIFailure>({
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
   * @name RecipesSuggest
   * @summary Suggest recipes based on search.
   * @request GET:/api/recipes/suggest
   * @response `200` `IItemSetOfSuggestRecipesResultItem`
   * @response `400` `IItemSetOfIFailure`
   */
  recipesSuggest = (query: RecipesSuggestParams, params: RequestParams = {}) =>
    this.request<IItemSetOfSuggestRecipesResultItem, IItemSetOfIFailure>({
      path: `/api/recipes/suggest`,
      method: 'GET',
      query: query,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Recipes
   * @name RecipesGet
   * @summary Get a recipe.
   * @request GET:/api/recipes/{id}
   * @response `200` `GetRecipeResponse`
   * @response `400` `IItemSetOfIFailure`
   */
  recipesGet = (id: number, params: RequestParams = {}) =>
    this.request<GetRecipeResponse, IItemSetOfIFailure>({
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
   * @response `200` `EntityMessageOfInteger`
   * @response `400` `IItemSetOfIFailure`
   */
  recipesDelete = (id: number, params: RequestParams = {}) =>
    this.request<EntityMessageOfInteger, IItemSetOfIFailure>({
      path: `/api/recipes/${id}`,
      method: 'DELETE',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags Recipes
   * @name RecipesRebuild
   * @summary Rebuild the recipe search index.
   * @request POST:/api/recipes/rebuild-index
   * @response `200` `UserMessage`
   */
  recipesRebuild = (params: RequestParams = {}) =>
    this.request<UserMessage, any>({
      path: `/api/recipes/rebuild-index`,
      method: 'POST',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags GroceryItems
   * @name GroceryItemsList
   * @summary List grocery items. All parameters are optional and some have defaults.
   * @request GET:/api/grocery-items
   * @response `200` `IItemSetOfListGroceryItemsResponse`
   * @response `400` `IItemSetOfIFailure`
   */
  groceryItemsList = (query: GroceryItemsListParams, params: RequestParams = {}) =>
    this.request<IItemSetOfListGroceryItemsResponse, IItemSetOfIFailure>({
      path: `/api/grocery-items`,
      method: 'GET',
      query: query,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags GroceryItems
   * @name GroceryItemsSave
   * @summary Save a grocery item. Will update if found, otherwise a new item will be created.
   * @request POST:/api/grocery-items
   * @response `200` `EntityMessageOfInteger`
   * @response `400` `IItemSetOfIFailure`
   */
  groceryItemsSave = (data: SaveGroceryItemRequest, params: RequestParams = {}) =>
    this.request<EntityMessageOfInteger, IItemSetOfIFailure>({
      path: `/api/grocery-items`,
      method: 'POST',
      body: data,
      type: ContentType.Json,
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags GroceryItems
   * @name GroceryItemsGet
   * @summary Get a grocery item.
   * @request GET:/api/grocery-items/{id}
   * @response `200` `GetGroceryItemResponse`
   * @response `400` `IItemSetOfIFailure`
   */
  groceryItemsGet = (id: number, params: RequestParams = {}) =>
    this.request<GetGroceryItemResponse, IItemSetOfIFailure>({
      path: `/api/grocery-items/${id}`,
      method: 'GET',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags GroceryItems
   * @name GroceryItemsDelete
   * @summary Delete a grocery item.
   * @request DELETE:/api/grocery-items/{id}
   * @response `200` `EntityMessageOfInteger`
   * @response `400` `IItemSetOfIFailure`
   */
  groceryItemsDelete = (id: number, params: RequestParams = {}) =>
    this.request<EntityMessageOfInteger, IItemSetOfIFailure>({
      path: `/api/grocery-items/${id}`,
      method: 'DELETE',
      format: 'json',
      ...params,
    });
  /**
   * No description
   *
   * @tags GroceryItems
   * @name GroceryItemsSaveInventory
   * @summary Update a grocery item inventory.
   * @request POST:/api/grocery-items/inventory
   * @response `200` `EntityMessageOfInteger`
   * @response `400` `IItemSetOfIFailure`
   */
  groceryItemsSaveInventory = (data: SaveGroceryItemInventoryRequest, params: RequestParams = {}) =>
    this.request<EntityMessageOfInteger, IItemSetOfIFailure>({
      path: `/api/grocery-items/inventory`,
      method: 'POST',
      body: data,
      type: ContentType.Json,
      format: 'json',
      ...params,
    });
}

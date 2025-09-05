using FoodStuffs.Model.Data.Models;

namespace FoodStuffs.Model.Search.Recipes;

public static class RecipeSearchConstants
{
    public const string INDEX_NAME = "Recipe";

    public const string FIELD_ID = nameof(Recipe.Id);
    public const string FIELD_NAME = nameof(Recipe.Name);
    public const string FIELD_NAME_PREFIX = nameof(Recipe.Name) + "Prefix";
    public const string FIELD_CREATED_ON = nameof(Recipe.CreatedOn);
    public const string FIELD_IS_FOR_MEAL_PLANNING = nameof(Recipe.IsForMealPlanning);
    public const string FIELD_MEAL_PLANNING_SIDES_COUNT = nameof(Recipe.MealPlanningSidesCount);
    public const string FIELD_IMAGE = nameof(Recipe.DefaultImage);
    public const string FIELD_SLUG = nameof(Recipe.Slug);
    public const string FIELD_CATEGORY_IDS = nameof(Category) + nameof(Category.Id) + "s";
    public const string FIELD_CATEGORIES_JSON = nameof(Recipe.Categories) + "Json";
    public const string FIELD_MEAL_GROCERY_ITEMS_JSON = nameof(RecipeGroceryItemRelation.GroceryItem) + "sJson";
}

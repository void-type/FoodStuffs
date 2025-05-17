using FoodStuffs.Model.Data.Models;

namespace FoodStuffs.Model.Search.Recipes;

public static class RecipeSearchConstants
{
    public const string INDEX_NAME = "Recipe";

    public const string FIELD_ID = nameof(Recipe.Id);
    public const string FIELD_NAME = nameof(Recipe.Name);
    public const string FIELD_NAME_PREFIX = nameof(Recipe.Name) + "_PREFIX";
    public const string FIELD_IS_FOR_MEAL_PLANNING = nameof(Recipe.IsForMealPlanning);
    public const string FIELD_CREATED_ON = nameof(Recipe.CreatedOn);
    public const string FIELD_CATEGORY_NAMES = nameof(Category) + "_" + nameof(Category.Name) + "s";
    public const string FIELD_CATEGORY_IDS = nameof(Category) + "_" + nameof(Category.Id) + "s";
    public const string FIELD_IMAGE = nameof(Recipe.DefaultImage);
    public const string FIELD_MEAL_SHOPPING_ITEMS_JSON = nameof(RecipeGroceryItemRelation.GroceryItem) + "_JSON";
    public const string FIELD_SLUG = nameof(Recipe.Slug);
}

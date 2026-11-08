using FoodStuffs.Model.Data.Models;

namespace FoodStuffs.Model.Search.Recipes;

public static class RecipeSearchConstants
{
    public const string IndexName = "Recipe";

    public const string FieldId = nameof(Recipe.Id);
    public const string FieldName = nameof(Recipe.Name);
    public const string FieldNamePrefix = nameof(Recipe.Name) + "Prefix";
    public const string FieldCreatedOn = nameof(Recipe.CreatedOn);
    public const string FieldModifiedOn = nameof(Recipe.ModifiedOn);
    public const string FieldIsForMealPlanning = nameof(Recipe.IsForMealPlanning);
    public const string FieldMealPlanningSidesCount = nameof(Recipe.MealPlanningSidesCount);
    public const string FieldImage = nameof(Recipe.DefaultImage);
    public const string FieldSlug = nameof(Recipe.Slug);
    public const string FieldCategoryIds = nameof(Recipe.Categories);
    public const string FieldCategoriesJson = nameof(Recipe.Categories) + "Json";
    public const string FieldGroceryItemIds = nameof(Recipe.GroceryItemRelations);
    public const string FieldMealGroceryItemsJson = nameof(RecipeGroceryItemRelation.GroceryItem) + "sJson";
}

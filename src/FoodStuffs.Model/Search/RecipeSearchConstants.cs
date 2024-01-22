using FoodStuffs.Model.Data.Models;
using Lucene.Net.Util;

namespace FoodStuffs.Model.Search;

public static class RecipeSearchConstants
{
    public const LuceneVersion Version = LuceneVersion.LUCENE_48;

    private const string JSON_SUFFIX = "_JSON";

    public const int MAX_RESULTS = 1000;

    public const string FIELD_ID = nameof(Recipe.Id);
    public const string FIELD_NAME = nameof(Recipe.Name);
    public const string FIELD_IS_FOR_MEAL_PLANNING = nameof(Recipe.IsForMealPlanning);
    public const string FIELD_CREATED_ON = nameof(Recipe.CreatedOn);
    public const string FIELD_CATEGORY_NAMES = nameof(Category) + "_" + nameof(Category.Name) + "s";
    public const string FIELD_CATEGORY_IDS = nameof(Category) + "_" + nameof(Category.Id) + "s";
    public const string FIELD_IMAGE = nameof(Recipe.DefaultImage);
    public const string FIELD_INGREDIENTS_JSON = nameof(Recipe.Ingredients) + JSON_SUFFIX;
}

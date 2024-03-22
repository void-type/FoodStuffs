using FoodStuffs.Model.Data.Models;
using Lucene.Net.Documents;
using Lucene.Net.Documents.Extensions;
using Lucene.Net.Facet;
using Lucene.Net.Util;
using System.Globalization;
using System.Text.Json;
using C = FoodStuffs.Model.Search.RecipeSearchConstants;

namespace FoodStuffs.Model.Search;

public static class RecipeSearchMappers
{
    public static Document ToDocument(this Recipe recipe)
    {
        var isForMealPlanning = recipe.IsForMealPlanning.ToString();
        var createdOn = recipe.CreatedOn.ToString("o");

        var doc = new Document
        {
            // Id: retrievable, filterable
            new StringField(C.FIELD_ID, recipe.Id.ToString(), Field.Store.YES),

            // Name: retrievable, searchable
            new TextField(C.FIELD_NAME, recipe.Name, Field.Store.YES),
            // Name: sortable
            new SortedDocValuesField(C.FIELD_NAME, new BytesRef(recipe.Name)),

            // IsForMealPlanning: retrievable
            new StoredField(C.FIELD_IS_FOR_MEAL_PLANNING, isForMealPlanning),
            // IsForMealPlanning: facetable
            new FacetField(C.FIELD_IS_FOR_MEAL_PLANNING, isForMealPlanning),

            // CreatedOn: retrievable
            new StoredField(C.FIELD_CREATED_ON, createdOn),
            // CreatedOn: sortable
            new SortedDocValuesField(C.FIELD_CREATED_ON, new BytesRef(createdOn)),

            // Slug: retrievable
            new StoredField(C.FIELD_SLUG, recipe.Slug),
        };

        foreach (var category in recipe.Categories)
        {
            // CategoryName: retrievable
            doc.AddStoredField(C.FIELD_CATEGORY_NAMES, category.Name);

            var categoryId = category.Id.ToString();

            // CategoryId: facetable
            doc.AddFacetField(C.FIELD_CATEGORY_IDS, categoryId);
        }

        foreach (var ingredient in recipe.Ingredients)
        {
            // Ingredients: retrievable
            doc.AddStoredField(C.FIELD_INGREDIENTS_JSON, JsonSerializer.Serialize(ingredient));
        }

        var image = recipe.DefaultImage;

        if (image is not null)
        {
            // Image: retrievable
            doc.AddStoredField(C.FIELD_IMAGE, image.FileName);
        }

        return doc;
    }

    public static RecipeSearchResultItem ToRecipeSearchResultItem(this Document doc)
    {
        return new RecipeSearchResultItem
        (
            Id: int.Parse(doc.Get(C.FIELD_ID)),
            Name: doc.Get(C.FIELD_NAME),
            IsForMealPlanning: bool.Parse(doc.Get(C.FIELD_IS_FOR_MEAL_PLANNING)),
            CreatedOn: doc.GetStringFieldAsDateTimeOrNull(C.FIELD_CREATED_ON) ?? DateTime.MinValue,
            Slug: doc.Get(C.FIELD_SLUG),
            Categories: doc.GetValues(C.FIELD_CATEGORY_NAMES).OrderBy(n => n),
            Ingredients: doc.GetValues(C.FIELD_INGREDIENTS_JSON)
                .Select(x =>
                {
                    var i = JsonSerializer.Deserialize<RecipeIngredient>(x);

                    return i is not null
                        ? new RecipeSearchResultItemIngredient(i.Name, i.Quantity, i.Order, i.IsCategory)
                        : null;
                })
                .Where(i => i is not null)!,
            Image: doc.Get(C.FIELD_IMAGE)
        );
    }

    public static FacetsConfig RecipeFacetsConfig()
    {
        var facetConfig = new FacetsConfig();

        facetConfig.SetMultiValued(C.FIELD_IS_FOR_MEAL_PLANNING, false);
        facetConfig.SetMultiValued(C.FIELD_CATEGORY_IDS, true);

        return facetConfig;
    }

    /// <summary>
    /// Convert a string field to a DateTime, or return null if the field is empty.
    /// DateTimes should be stored using .ToString("o") to ensure they are stored in a sortable and parsable format.
    /// </summary>
    public static DateTime? GetStringFieldAsDateTimeOrNull(this Document doc, string fieldName)
    {
        var value = doc.Get(fieldName);

        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        // This should yield the same DateTime (date, time and kind) that was originally stored and won't be affected by parser's timezone.
        return DateTime.ParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }
}

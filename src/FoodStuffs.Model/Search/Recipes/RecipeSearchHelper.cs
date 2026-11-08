using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Search.Lucene;
using FoodStuffs.Model.Search.Recipes.Models;
using Lucene.Net.Documents;
using Lucene.Net.Documents.Extensions;
using Lucene.Net.Facet;
using Lucene.Net.Util;
using System.Globalization;
using System.Text.Json;
using VoidCore.Model.Functional;
using C = FoodStuffs.Model.Search.Recipes.RecipeSearchConstants;

namespace FoodStuffs.Model.Search.Recipes;

public static class RecipeSearchHelper
{
    public static Document ToDocument(this Recipe recipe)
    {
        var isForMealPlanning = recipe.IsForMealPlanning.ToString();
        var createdOn = recipe.CreatedOn.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture);
        var modifiedOn = recipe.ModifiedOn.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture);

        var doc = new Document
        {
            // Id: retrievable, filterable
            new StringField(C.FieldId, recipe.Id.ToString(CultureInfo.InvariantCulture), Field.Store.YES),

            // Name: retrievable, searchable
            new TextField(C.FieldName, recipe.Name, Field.Store.YES),
            // Name: sortable
            new SortedDocValuesField(C.FieldName, new BytesRef(recipe.Name)),
            // Name: Verbatim string for prefix search, needs to be lowercase
            new StringField(C.FieldNamePrefix, recipe.Name.ToLowerInvariant(), Field.Store.NO),

            // IsForMealPlanning: retrievable
            new StoredField(C.FieldIsForMealPlanning, isForMealPlanning),
            // IsForMealPlanning: facetable
            new FacetField(C.FieldIsForMealPlanning, isForMealPlanning),

            // MealPlanningSidesCount: retrievable
            new StoredField(C.FieldMealPlanningSidesCount, recipe.MealPlanningSidesCount.ToString(CultureInfo.InvariantCulture)),

            // CreatedOn: retrievable
            new StoredField(C.FieldCreatedOn, createdOn),
            // CreatedOn: sortable
            new SortedDocValuesField(C.FieldCreatedOn, new BytesRef(createdOn)),

            // ModifiedOn: retrievable
            new StoredField(C.FieldModifiedOn, modifiedOn),
            // ModifiedOn: sortable
            new SortedDocValuesField(C.FieldModifiedOn, new BytesRef(modifiedOn)),

            // Slug: retrievable
            new StoredField(C.FieldSlug, recipe.Slug),
        };

        AddCategories(doc, recipe);
        AddGroceryItems(doc, recipe);

        var image = recipe.DefaultImage;

        if (image is not null)
        {
            // Image: retrievable
            doc.AddStoredField(C.FieldImage, image.FileName);
        }

        return doc;
    }

    private static void AddCategories(Document doc, Recipe recipe)
    {
        var categories = recipe.Categories
            .Select(c => new SearchRecipesResultItemCategory(
                Id: c.Id,
                Name: c.Name,
                Color: c.Color,
                ShowOnCards: c.ShowOnCards
            ))
            .OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        // Categories: retrievable
        doc.AddStoredField(C.FieldCategoriesJson, JsonSerializer.Serialize(categories));

        foreach (var category in categories)
        {
            // CategoryId: facetable
            doc.AddFacetField(C.FieldCategoryIds, category.Id.ToString(CultureInfo.InvariantCulture));
        }
    }

    private static void AddGroceryItems(Document doc, Recipe recipe)
    {
        var groceryItems = recipe.GroceryItemRelations
            .Select(x => new SearchRecipesResultItemGroceryItem(
                Id: x.GroceryItemId,
                Name: x.GroceryItem.Name,
                Quantity: x.Quantity,
                Order: x.Order
            ))
            .OrderBy(x => x.Order)
            .ToArray();

        // Grocery items: retrievable
        doc.AddStoredField(C.FieldMealGroceryItemsJson, JsonSerializer.Serialize(groceryItems));

        foreach (var groceryItem in groceryItems)
        {
            // GroceryItemId: facetable
            doc.AddFacetField(C.FieldGroceryItemIds, groceryItem.Id.ToString(CultureInfo.InvariantCulture));
        }
    }

    public static SearchRecipesResultItem ToSearchResultItem(this Document doc)
    {
        return new SearchRecipesResultItem
        (
            Id: int.Parse(doc.GetFieldValue(C.FieldId), CultureInfo.InvariantCulture),
            Name: doc.GetFieldValue(C.FieldName),
            IsForMealPlanning: bool.Parse(doc.GetFieldValue(C.FieldIsForMealPlanning)),
            MealPlanningSidesCount: int.Parse(doc.GetFieldValue(C.FieldMealPlanningSidesCount) ?? "0", CultureInfo.InvariantCulture),
            CreatedOn: doc.GetStringFieldAsDateTimeOffsetOrNull(C.FieldCreatedOn) ?? DateTimeOffset.MinValue,
            ModifiedOn: doc.GetStringFieldAsDateTimeOffsetOrNull(C.FieldModifiedOn) ?? DateTimeOffset.MinValue,
            Slug: doc.GetFieldValue(C.FieldSlug),
            Categories: doc.GetFieldValue(C.FieldCategoriesJson)
                .Map(x => JsonSerializer.Deserialize<List<SearchRecipesResultItemCategory>>(x) ?? []),
            GroceryItems: doc.GetFieldValue(C.FieldMealGroceryItemsJson)
                .Map(x => JsonSerializer.Deserialize<List<SearchRecipesResultItemGroceryItem>>(x) ?? []),
            Image: doc.GetFieldValue(C.FieldImage)
        );
    }

    public static SuggestRecipesResultItem ToSuggestResultItem(this Document doc)
    {
        return new SuggestRecipesResultItem
        (
            Id: int.Parse(doc.GetFieldValue(C.FieldId), CultureInfo.InvariantCulture),
            Name: doc.GetFieldValue(C.FieldName),
            Slug: doc.GetFieldValue(C.FieldSlug),
            Image: doc.GetFieldValue(C.FieldImage)
        );
    }

    public static FacetsConfig FacetsConfig()
    {
        var facetConfig = new FacetsConfig();

        facetConfig.SetMultiValued(C.FieldIsForMealPlanning, false);
        facetConfig.SetMultiValued(C.FieldCategoryIds, true);
        facetConfig.SetMultiValued(C.FieldGroceryItemIds, true);

        return facetConfig;
    }
}

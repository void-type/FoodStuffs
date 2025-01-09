using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Search.Recipes.Models;
using Lucene.Net.Documents;
using Lucene.Net.Documents.Extensions;
using Lucene.Net.Facet;
using Lucene.Net.Util;
using System.Text.Json;
using VoidCore.Model.Functional;
using C = FoodStuffs.Model.Search.Recipes.RecipeSearchConstants;

namespace FoodStuffs.Model.Search.Recipes;

public static class RecipeSearchHelper
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

        var shoppingItems = recipe.ShoppingItemRelations
            .Select(x => new SearchRecipesResultItemShoppingItem(
                Name: x.ShoppingItem.Name,
                Quantity: x.Quantity,
                Order: x.Order
            ));

        // Ingredients: retrievable
        doc.AddStoredField(C.FIELD_MEAL_SHOPPING_ITEMS_JSON, JsonSerializer.Serialize(shoppingItems));

        var image = recipe.DefaultImage;

        if (image is not null)
        {
            // Image: retrievable
            doc.AddStoredField(C.FIELD_IMAGE, image.FileName);
        }

        return doc;
    }

    public static SearchRecipesResultItem ToSearchRecipesResultItem(this Document doc)
    {
        return new SearchRecipesResultItem
        (
            Id: int.Parse(doc.Get(C.FIELD_ID)),
            Name: doc.Get(C.FIELD_NAME),
            IsForMealPlanning: bool.Parse(doc.Get(C.FIELD_IS_FOR_MEAL_PLANNING)),
            CreatedOn: doc.GetStringFieldAsDateTimeOrNull(C.FIELD_CREATED_ON) ?? DateTime.MinValue,
            Slug: doc.Get(C.FIELD_SLUG),
            Categories: doc.GetValues(C.FIELD_CATEGORY_NAMES)
                .OrderBy(n => n)
                .ToList(),
            ShoppingItems: doc.Get(C.FIELD_MEAL_SHOPPING_ITEMS_JSON)
                .Map(x => JsonSerializer.Deserialize<List<SearchRecipesResultItemShoppingItem>>(x) ?? []),
            Image: doc.Get(C.FIELD_IMAGE)
        );
    }

    public static SuggestRecipesResultItem ToSuggestRecipesResultItem(this Document doc)
    {
        return new SuggestRecipesResultItem
        (
            Id: int.Parse(doc.Get(C.FIELD_ID)),
            Name: doc.Get(C.FIELD_NAME),
            Slug: doc.Get(C.FIELD_SLUG),
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
}

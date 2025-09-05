using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Search.GroceryItems.Models;
using Lucene.Net.Documents;
using Lucene.Net.Documents.Extensions;
using Lucene.Net.Facet;
using Lucene.Net.Util;
using System.Text.Json;
using VoidCore.Model.Functional;
using C = FoodStuffs.Model.Search.GroceryItems.GroceryItemSearchConstants;

namespace FoodStuffs.Model.Search.GroceryItems;

public static class GroceryItemSearchHelper
{
    public static Document ToDocument(this GroceryItem groceryItem)
    {
        var isForMealPlanning = groceryItem.IsForMealPlanning.ToString();
        var createdOn = groceryItem.CreatedOn.ToString("o");

        var doc = new Document
        {
            // Id: retrievable, filterable
            new StringField(C.FIELD_ID, groceryItem.Id.ToString(), Field.Store.YES),

            // Name: retrievable, searchable
            new TextField(C.FIELD_NAME, groceryItem.Name, Field.Store.YES),
            // Name: sortable
            new SortedDocValuesField(C.FIELD_NAME, new BytesRef(groceryItem.Name)),
            // Name: Verbatim string for prefix search, needs to be lowercase
            new StringField(C.FIELD_NAME_PREFIX, groceryItem.Name.ToLower(), Field.Store.NO),

            // IsForMealPlanning: retrievable
            new StoredField(C.FIELD_IS_OUT_OF_STOCK, isForMealPlanning),
            // IsForMealPlanning: facetable
            new FacetField(C.FIELD_IS_OUT_OF_STOCK, isForMealPlanning),

            // MealPlanningSidesCount: retrievable
            new StoredField(C.FIELD_MEAL_PLANNING_SIDES_COUNT, groceryItem.MealPlanningSidesCount.ToString()),

            // CreatedOn: retrievable
            new StoredField(C.FIELD_CREATED_ON, createdOn),
            // CreatedOn: sortable
            new SortedDocValuesField(C.FIELD_CREATED_ON, new BytesRef(createdOn)),

            // Slug: retrievable
            new StoredField(C.FIELD_SLUG, groceryItem.Slug),
        };

        var categories = groceryItem.Categories
            .Select(c => new SearchGroceryItemsResultItemCategory(
                Id: c.Id,
                Name: c.Name,
                Color: c.Color
            ))
            .OrderBy(c => c.Name)
            .ToArray();

        // Categories: retrievable
        doc.AddStoredField(C.FIELD_CATEGORIES, JsonSerializer.Serialize(categories));

        foreach (var category in categories)
        {
            // CategoryId: facetable
            doc.AddFacetField(C.FIELD_CATEGORY_IDS, category.Id.ToString());
        }

        var groceryItems = groceryItem.GroceryItemRelations
            .Select(x => new SearchGroceryItemsResultItemGroceryItem(
                Name: x.GroceryItem.Name,
                Quantity: x.Quantity,
                Order: x.Order
            ));

        // Grocery items: retrievable
        doc.AddStoredField(C.FIELD_MEAL_GROCERY_ITEMS_JSON, JsonSerializer.Serialize(groceryItems));

        var image = groceryItem.DefaultImage;

        if (image is not null)
        {
            // Image: retrievable
            doc.AddStoredField(C.FIELD_IMAGE, image.FileName);
        }

        return doc;
    }

    public static SearchGroceryItemsResultItem ToSearchResultItem(this Document doc)
    {
        return new SearchGroceryItemsResultItem
        (
            Id: int.Parse(doc.Get(C.FIELD_ID)),
            Name: doc.Get(C.FIELD_NAME),
            IsForMealPlanning: bool.Parse(doc.Get(C.FIELD_IS_OUT_OF_STOCK)),
            MealPlanningSidesCount: int.Parse(doc.Get(C.FIELD_MEAL_PLANNING_SIDES_COUNT) ?? "0"),
            CreatedOn: doc.GetStringFieldAsDateTimeOrNull(C.FIELD_CREATED_ON) ?? DateTime.MinValue,
            Slug: doc.Get(C.FIELD_SLUG),
            Categories: doc.Get(C.FIELD_CATEGORIES)
                .Map(x => JsonSerializer.Deserialize<List<SearchGroceryItemsResultItemCategory>>(x) ?? []),
            GroceryItems: doc.Get(C.FIELD_MEAL_GROCERY_ITEMS_JSON)
                .Map(x => JsonSerializer.Deserialize<List<SearchGroceryItemsResultItemGroceryItem>>(x) ?? []),
            Image: doc.Get(C.FIELD_IMAGE)
        );
    }

    public static SuggestGroceryItemsResultItem ToSuggestResultItem(this Document doc)
    {
        return new SuggestGroceryItemsResultItem
        (
            Id: int.Parse(doc.Get(C.FIELD_ID)),
            Name: doc.Get(C.FIELD_NAME),
            Slug: doc.Get(C.FIELD_SLUG),
            Image: doc.Get(C.FIELD_IMAGE)
        );
    }

    public static FacetsConfig FacetsConfig()
    {
        var facetConfig = new FacetsConfig();

        facetConfig.SetMultiValued(C.FIELD_IS_OUT_OF_STOCK, false);
        facetConfig.SetMultiValued(C.FIELD_CATEGORY_IDS, true);

        return facetConfig;
    }
}

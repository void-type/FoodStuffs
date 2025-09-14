using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Search.GroceryItems.Models;
using FoodStuffs.Model.Search.Lucene;
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
        var isOutOfStock = (groceryItem.InventoryQuantity < 1).ToString();
        var isUnused = (!groceryItem.Recipes.Any()).ToString();
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

            // IsOutOfStock: retrievable
            new StoredField(C.FIELD_IS_OUT_OF_STOCK, isOutOfStock),
            // IsOutOfStock: facetable
            new FacetField(C.FIELD_IS_OUT_OF_STOCK, isOutOfStock),

            // IsUnused: retrievable
            new StoredField(C.FIELD_IS_UNUSED, isUnused),
            // IsUnused: facetable
            new FacetField(C.FIELD_IS_UNUSED, isUnused),

            // InventoryQuantity: retrievable
            new StoredField(C.FIELD_INVENTORY_QUANTITY, groceryItem.InventoryQuantity.ToString()),

            // RecipeCount: retrievable
            new StoredField(C.FIELD_RECIPE_COUNT, groceryItem.Recipes.Count.ToString()),

            // CreatedOn: retrievable
            new StoredField(C.FIELD_CREATED_ON, createdOn),
            // CreatedOn: sortable
            new SortedDocValuesField(C.FIELD_CREATED_ON, new BytesRef(createdOn)),
        };

        var storageLocations = groceryItem.StorageLocations
            .Select(c => new SearchGroceryItemsResultItemStorageLocation(
                Id: c.Id,
                Name: c.Name
            ))
            .OrderBy(c => c.Name)
            .ToArray();

        // StorageLocations: retrievable
        doc.AddStoredField(C.FIELD_STORAGE_LOCATIONS_JSON, JsonSerializer.Serialize(storageLocations));

        foreach (var location in storageLocations)
        {
            // LocationId: facetable
            doc.AddFacetField(C.FIELD_STORAGE_LOCATION_IDS, location.Id.ToString());
        }

        if (groceryItem.GroceryAisle is not null)
        {
            // GroceryAisle: retrievable
            doc.AddStoredField(C.FIELD_GROCERY_AISLE_JSON, JsonSerializer.Serialize(new SearchGroceryItemsResultItemGroceryAisle(
                Id: groceryItem.GroceryAisle.Id,
                Name: groceryItem.GroceryAisle.Name,
                Order: groceryItem.GroceryAisle.Order
            )));

            // GroceryAisleId: facetable
            doc.AddFacetField(C.FIELD_GROCERY_AISLE_ID, groceryItem.GroceryAisle.Id.ToString());
        }

        return doc;
    }

    public static SearchGroceryItemsResultItem ToSearchResultItem(this Document doc)
    {
        return new SearchGroceryItemsResultItem
        (
            Id: int.Parse(doc.Get(C.FIELD_ID)),
            Name: doc.Get(C.FIELD_NAME),
            IsOutOfStock: bool.Parse(doc.Get(C.FIELD_IS_OUT_OF_STOCK)),
            IsUnused: bool.Parse(doc.Get(C.FIELD_IS_UNUSED)),
            InventoryQuantity: int.Parse(doc.Get(C.FIELD_INVENTORY_QUANTITY) ?? "0"),
            RecipeCount: int.Parse(doc.Get(C.FIELD_RECIPE_COUNT) ?? "0"),
            CreatedOn: doc.GetStringFieldAsDateTimeOrNull(C.FIELD_CREATED_ON) ?? DateTime.MinValue,
            StorageLocations: doc.Get(C.FIELD_STORAGE_LOCATIONS_JSON)
                .Map(x => JsonSerializer.Deserialize<List<SearchGroceryItemsResultItemStorageLocation>>(x) ?? []),
            GroceryAisle: doc.Get(C.FIELD_GROCERY_AISLE_JSON)
                .Map(x => x is null ? null : JsonSerializer.Deserialize<SearchGroceryItemsResultItemGroceryAisle>(x))
        );
    }

    public static SuggestGroceryItemsResultItem ToSuggestResultItem(this Document doc)
    {
        return new SuggestGroceryItemsResultItem
        (
            Id: int.Parse(doc.Get(C.FIELD_ID)),
            Name: doc.Get(C.FIELD_NAME)
        );
    }

    public static FacetsConfig FacetsConfig()
    {
        var facetConfig = new FacetsConfig();

        facetConfig.SetMultiValued(C.FIELD_IS_OUT_OF_STOCK, false);
        facetConfig.SetMultiValued(C.FIELD_IS_UNUSED, false);

        facetConfig.SetMultiValued(C.FIELD_STORAGE_LOCATION_IDS, true);
        facetConfig.SetMultiValued(C.FIELD_GROCERY_AISLE_ID, false);

        return facetConfig;
    }
}

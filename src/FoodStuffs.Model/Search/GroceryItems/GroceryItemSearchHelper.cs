using FoodStuffs.Model.Data.Models;
using FoodStuffs.Model.Search.GroceryItems.Models;
using FoodStuffs.Model.Search.Lucene;
using Lucene.Net.Documents;
using Lucene.Net.Documents.Extensions;
using Lucene.Net.Facet;
using Lucene.Net.Util;
using System.Globalization;
using System.Text.Json;
using VoidCore.Model.Functional;
using C = FoodStuffs.Model.Search.GroceryItems.GroceryItemSearchConstants;

namespace FoodStuffs.Model.Search.GroceryItems;

public static class GroceryItemSearchHelper
{
    public static Document ToDocument(this GroceryItem groceryItem)
    {
        var isOutOfStock = (groceryItem.InventoryQuantity < 1).ToString();
        var isUnused = (groceryItem.Recipes.Count == 0).ToString();
        var createdOn = groceryItem.CreatedOn.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture);
        var modifiedOn = groceryItem.ModifiedOn.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture);

        var doc = new Document
        {
            // Id: retrievable, filterable
            new StringField(C.FieldId, groceryItem.Id.ToString(CultureInfo.InvariantCulture), Field.Store.YES),

            // Name: retrievable, searchable
            new TextField(C.FieldName, groceryItem.Name, Field.Store.YES),
            // Name: sortable
            new SortedDocValuesField(C.FieldName, new BytesRef(groceryItem.Name)),
            // Name: Verbatim string for prefix search, needs to be lowercase
            new StringField(C.FieldNamePrefix, groceryItem.Name.ToLowerInvariant(), Field.Store.NO),

            // IsOutOfStock: retrievable
            new StoredField(C.FieldIsOutOfStock, isOutOfStock),
            // IsOutOfStock: facetable
            new FacetField(C.FieldIsOutOfStock, isOutOfStock),

            // IsUnused: retrievable
            new StoredField(C.FieldIsUnused, isUnused),
            // IsUnused: facetable
            new FacetField(C.FieldIsUnused, isUnused),

            // InventoryQuantity: retrievable
            new StoredField(C.FieldInventoryQuantity, groceryItem.InventoryQuantity.ToString(CultureInfo.InvariantCulture)),

            // RecipeCount: retrievable
            new StoredField(C.FieldRecipeCount, groceryItem.Recipes.Count.ToString(CultureInfo.InvariantCulture)),

            // CreatedOn: retrievable
            new StoredField(C.FieldCreatedOn, createdOn),
            // CreatedOn: sortable
            new SortedDocValuesField(C.FieldCreatedOn, new BytesRef(createdOn)),

            // ModifiedOn: retrievable
            new StoredField(C.FieldModifiedOn, modifiedOn),
            // ModifiedOn: sortable
            new SortedDocValuesField(C.FieldModifiedOn, new BytesRef(modifiedOn)),
        };

        AddStorageLocations(doc, groceryItem);
        AddGroceryStores(doc, groceryItem);
        AddGroceryAisle(doc, groceryItem);

        return doc;
    }

    private static void AddStorageLocations(Document doc, GroceryItem groceryItem)
    {
        var storageLocations = groceryItem.StorageLocations
            .Select(c => new SearchGroceryItemsResultItemStorageLocation(
                Id: c.Id,
                Name: c.Name
            ))
            .OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        // StorageLocations: retrievable
        doc.AddStoredField(C.FieldStorageLocationsJson, JsonSerializer.Serialize(storageLocations));

        foreach (var location in storageLocations)
        {
            // LocationId: facetable
            doc.AddFacetField(C.FieldStorageLocationIds, location.Id.ToString(CultureInfo.InvariantCulture));
        }
    }

    private static void AddGroceryStores(Document doc, GroceryItem groceryItem)
    {
        var groceryStores = groceryItem.GroceryStores
            .Select(c => new SearchGroceryItemsResultItemGroceryStore(
                Id: c.Id,
                Name: c.Name
            ))
            .OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        // GroceryStores: retrievable
        doc.AddStoredField(C.FieldGroceryStoresJson, JsonSerializer.Serialize(groceryStores));

        foreach (var store in groceryStores)
        {
            // GroceryStoreId: facetable
            doc.AddFacetField(C.FieldGroceryStoreIds, store.Id.ToString(CultureInfo.InvariantCulture));
        }
    }

    private static void AddGroceryAisle(Document doc, GroceryItem groceryItem)
    {
        if (groceryItem.GroceryAisle is not null)
        {
            // GroceryAisle: retrievable
            doc.AddStoredField(C.FieldGroceryAisleJson, JsonSerializer.Serialize(new SearchGroceryItemsResultItemGroceryAisle(
                Id: groceryItem.GroceryAisle.Id,
                Name: groceryItem.GroceryAisle.Name,
                Order: groceryItem.GroceryAisle.Order
            )));

            // GroceryAisleId: facetable
            doc.AddFacetField(C.FieldGroceryAisleId, groceryItem.GroceryAisle.Id.ToString(CultureInfo.InvariantCulture));
        }
    }

    public static SearchGroceryItemsResultItem ToSearchResultItem(this Document doc)
    {
        return new SearchGroceryItemsResultItem
        (
            Id: int.Parse(doc.GetFieldValue(C.FieldId), CultureInfo.InvariantCulture),
            Name: doc.GetFieldValue(C.FieldName),
            IsOutOfStock: bool.Parse(doc.GetFieldValue(C.FieldIsOutOfStock)),
            IsUnused: bool.Parse(doc.GetFieldValue(C.FieldIsUnused)),
            InventoryQuantity: int.Parse(doc.GetFieldValue(C.FieldInventoryQuantity) ?? "0", CultureInfo.InvariantCulture),
            RecipeCount: int.Parse(doc.GetFieldValue(C.FieldRecipeCount) ?? "0", CultureInfo.InvariantCulture),
            CreatedOn: doc.GetStringFieldAsDateTimeOffsetOrNull(C.FieldCreatedOn) ?? DateTimeOffset.MinValue,
            ModifiedOn: doc.GetStringFieldAsDateTimeOffsetOrNull(C.FieldModifiedOn) ?? DateTimeOffset.MinValue,
            StorageLocations: doc.GetFieldValue(C.FieldStorageLocationsJson)
                .Map(x => JsonSerializer.Deserialize<List<SearchGroceryItemsResultItemStorageLocation>>(x) ?? []),
            GroceryStores: doc.GetFieldValue(C.FieldGroceryStoresJson)
                .Map(x => JsonSerializer.Deserialize<List<SearchGroceryItemsResultItemGroceryStore>>(x) ?? []),
            GroceryAisle: doc.GetFieldValue(C.FieldGroceryAisleJson)
                .Map(x => x is null ? null : JsonSerializer.Deserialize<SearchGroceryItemsResultItemGroceryAisle>(x))
        );
    }

    public static SuggestGroceryItemsResultItem ToSuggestResultItem(this Document doc)
    {
        return new SuggestGroceryItemsResultItem
        (
            Id: int.Parse(doc.GetFieldValue(C.FieldId), CultureInfo.InvariantCulture),
            Name: doc.GetFieldValue(C.FieldName)
        );
    }

    public static FacetsConfig FacetsConfig()
    {
        var facetConfig = new FacetsConfig();

        facetConfig.SetMultiValued(C.FieldIsOutOfStock, false);
        facetConfig.SetMultiValued(C.FieldIsUnused, false);

        facetConfig.SetMultiValued(C.FieldStorageLocationIds, true);
        facetConfig.SetMultiValued(C.FieldGroceryStoreIds, true);
        facetConfig.SetMultiValued(C.FieldGroceryAisleId, false);

        return facetConfig;
    }
}

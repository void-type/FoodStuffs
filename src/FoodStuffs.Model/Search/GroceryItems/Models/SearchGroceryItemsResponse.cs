using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Search.GroceryItems.Models;

public record SearchGroceryItemsResponse(
    IItemSet<SearchGroceryItemsResultItem> Results,
    List<SearchFacet> Facets);

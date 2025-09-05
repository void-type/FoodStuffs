using FoodStuffs.Model.Search.GroceryItems.Models;
using VoidCore.Model.Responses.Collections;

namespace FoodStuffs.Model.Search.GroceryItems;

public interface IGroceryItemQueryService
{
    /// <summary>
    /// Search grocery items with facets and pagination.
    /// </summary>
    SearchGroceryItemsResponse Search(SearchGroceryItemsRequest request);

    /// <summary>
    /// Suggest grocery items for autocompletion.
    /// </summary>
    IItemSet<SuggestGroceryItemsResultItem> Suggest(SuggestGroceryItemsRequest request);
}

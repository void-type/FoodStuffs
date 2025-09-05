namespace FoodStuffs.Model.Search.GroceryItems.Models;

public record SearchGroceryItemsResultItem(
    int Id,
    string Name,
    bool IsForMealPlanning,
    int MealPlanningSidesCount,
    DateTimeOffset CreatedOn,
    string Slug,
    List<SearchGroceryItemsResultItemCategory> Categories,
    List<SearchGroceryItemsResultItemGroceryItem> GroceryItems,
    string? Image);

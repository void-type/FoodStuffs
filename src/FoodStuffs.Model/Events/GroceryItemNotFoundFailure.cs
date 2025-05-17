using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events;

public class GroceryItemNotFoundFailure : Failure
{
    public GroceryItemNotFoundFailure()
        : base(errorMessage: "Grocery item not found.", uiHandle: "GroceryItemId")
    {
    }
}

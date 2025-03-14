using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events;

public class ShoppingItemNotFoundFailure : Failure
{
    public ShoppingItemNotFoundFailure()
        : base(errorMessage: "Grocery item not found.", uiHandle: "ShoppingItemId")
    {
    }
}

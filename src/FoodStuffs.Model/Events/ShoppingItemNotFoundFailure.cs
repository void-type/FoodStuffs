using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events;

public class ShoppingItemNotFoundFailure : Failure
{
    public ShoppingItemNotFoundFailure()
        : base(errorMessage: "Shopping item not found.", uiHandle: "ShoppingItemId")
    {
    }
}

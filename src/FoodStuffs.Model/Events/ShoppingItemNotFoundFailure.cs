using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events;

public class ShoppingItemNotFoundFailure : Failure
{
    public ShoppingItemNotFoundFailure()
        : base(errorMessage: "Meal plan not found.", uiHandle: "ShoppingItemId")
    {
    }
}

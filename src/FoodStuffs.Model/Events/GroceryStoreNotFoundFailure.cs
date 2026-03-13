using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events;

public class GroceryStoreNotFoundFailure : Failure
{
    public GroceryStoreNotFoundFailure()
        : base(errorMessage: "Grocery store not found.", uiHandle: "GroceryStoreId")
    {
    }
}

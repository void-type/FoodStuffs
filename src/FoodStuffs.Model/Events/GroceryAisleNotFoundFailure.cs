using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events;

public class GroceryAisleNotFoundFailure : Failure
{
    public GroceryAisleNotFoundFailure()
        : base(errorMessage: "Grocery aisle not found.", uiHandle: "GroceryAisleId")
    {
    }
}

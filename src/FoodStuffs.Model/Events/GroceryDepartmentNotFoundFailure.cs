using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events;

public class GroceryDepartmentNotFoundFailure : Failure
{
    public GroceryDepartmentNotFoundFailure()
        : base(errorMessage: "Grocery aisle not found.", uiHandle: "GroceryDepartmentId")
    {
    }
}

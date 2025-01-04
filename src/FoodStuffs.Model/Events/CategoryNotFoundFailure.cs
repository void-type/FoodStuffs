using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events;

public class CategoryNotFoundFailure : Failure
{
    public CategoryNotFoundFailure()
        : base(errorMessage: "Category not found.", uiHandle: "CategoryId")
    {
    }
}

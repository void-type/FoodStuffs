using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events;

public class RecipeNotFoundFailure : Failure
{
    public RecipeNotFoundFailure()
        : base(errorMessage: "Recipe not found.", uiHandle: "recipeId")
    {
    }
}

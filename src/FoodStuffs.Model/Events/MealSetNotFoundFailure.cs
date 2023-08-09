using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events;

public class MealSetNotFoundFailure : Failure
{
    public MealSetNotFoundFailure() : base(errorMessage: "Meal set not found.", uiHandle: "mealSetId")
    {
    }
}

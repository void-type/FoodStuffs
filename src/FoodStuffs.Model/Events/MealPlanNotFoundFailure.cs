using VoidCore.Model.Functional;

namespace FoodStuffs.Model.Events;

public class MealPlanNotFoundFailure : Failure
{
    public MealPlanNotFoundFailure() : base(errorMessage: "Meal set not found.", uiHandle: "mealPlanId")
    {
    }
}
